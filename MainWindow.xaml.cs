using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Key_Wizard.search;
using Key_Wizard.shortcuts;
using Key_Wizard.startup;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using Windows.Media.SpeechRecognition;
using Windows.System;
using WinRT.Interop;

namespace Key_Wizard
{
    public class WindowHelper
    {
        private const int SW_MINIMIZE = 6;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void MinimizeWindow(Window window)
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            ShowWindow(hWnd, SW_MINIMIZE);
        }
    }

    public sealed partial class MainWindow : Window
    {
        private List<Category> categories;
        private List<Shortcut> searchList;
        private DispatcherTimer searchDelayTimer;
        private const int SEARCH_DELAY_MS = 200;

        private SpeechRecognizer _speechRecognizer;
        private bool _isListening = false;

        public MainWindow()
        {
            this.InitializeComponent();
            searchDelayTimer = new DispatcherTimer();
            searchDelayTimer.Interval = TimeSpan.FromMilliseconds(SEARCH_DELAY_MS);
            searchDelayTimer.Tick += SearchDelayTimer_Tick;

            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.IsShownInSwitchers = false;

            if (appWindow != null)
            {
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsMaximizable = false;
                    presenter.IsResizable = false;
                    presenter.IsMinimizable = false;
                    presenter.SetBorderAndTitleBar(true, false);
                }
            }

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null);

            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));

            // Add event handler for Window.Closed
            this.Closed += MainWindow_Closed;

            this.categories = LoadShortcuts.Read();
            this.searchList = categories.SelectMany(category => category.Shortcuts).ToList();
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            if (_isListening && _speechRecognizer != null)
            {
                await _speechRecognizer.StopRecognitionAsync();
                _speechRecognizer.Dispose();
                _speechRecognizer = null;
                _isListening = false;
            }
        }

        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void SearchDelayTimer_Tick(object sender, object e)
        {
            searchDelayTimer.Stop();
            string searchQuery = searchTextBox.Text;
            ObservableCollection<Category> display = new ObservableCollection<Category>();
            ObservableCollection<Category> keyDisplay = new ObservableCollection<Category>();

            List<Shortcut> results = Search.Do(searchList, searchQuery);
            results.ForEach((shortcut) => shortcut.SearchResults = GenerateSearchResults(searchQuery, shortcut.Description));
            if (!string.IsNullOrWhiteSpace(searchQuery) && results.Any())
            {
                display.Add(new Category { Name = "Search Results", Shortcuts = new ObservableCollection<Shortcut>(results) });
                keyDisplay.Add(new Category { Name = "", Shortcuts = new ObservableCollection<Shortcut>(results) });
                ResultsBorderBar.Visibility = Visibility.Visible;
            }
            else
            {
                this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
                ResultsBorderBar.Visibility = Visibility.Collapsed;
            }
            shortcutsList.ItemsSource = display;
            keyList.ItemsSource = keyDisplay;
        }

        private void ListView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Space)
            {
                var listView = (ListView)sender;
                var item = (Shortcut)listView.SelectedItem;
                TriggerAction(item);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (Shortcut)e.ClickedItem;
            TriggerAction(item);
        }

        private void TriggerAction(Shortcut shortcut)
        {
            List<byte> keys = [];
            foreach (var key in shortcut.Keys)
            {
                var fieldInfo = typeof(Keys).GetField(key, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (fieldInfo == null)
                {
                    Debug.WriteLine("ERROR: Provided key does not exist.");
                    // TODO: Better error handling here
                } else
                {
                    MainGrid.UpdateLayout();
                    double contentWidth = this.AppWindow.Size.Width;
                    double contentHeight = 0.0;
                    var workArea = DisplayArea.Primary.WorkArea;

                    this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, contentWidth / workArea.Width, contentHeight / workArea.Height));
                    WindowHelper.MinimizeWindow(this);
                    searchTextBox.Text = "";
                    searchTextBox.ClearUndoRedoHistory();

                    keys.Add((byte)fieldInfo.GetValue(null));
                }
            }

            foreach (var key in keys)
            {
                Keys.Press(key);
            }
            keys.Reverse();
            foreach (var key in keys)
            {
                Keys.Release(key);
            }
        }

        private void shortcutsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainGrid.UpdateLayout();
            double contentWidth = this.AppWindow.Size.Width;
            double contentHeight = searchTextBox.ActualHeight + searchTextBox.Margin.Top + searchTextBox.Margin.Bottom +
                                   MainGrid.Margin.Top + MainGrid.Margin.Bottom + e.NewSize.Height;
            var workArea = DisplayArea.Primary.WorkArea;

            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, contentWidth / workArea.Width, contentHeight / workArea.Height));
        }

        private async void VoiceInput(object sender, RoutedEventArgs e)
        {
            if (_isListening)
            {
                // Stop listening
                ListenIcon.Glyph = "\uF781";
                searchTextBox.Text = null;
                _isListening = false;

                if (_speechRecognizer != null)
                {
                    await _speechRecognizer.StopRecognitionAsync();
                    _speechRecognizer.Dispose();
                    _speechRecognizer = null;
                }
            }
            else
            {
                // Start listening
                ListenIcon.Glyph = "\xE720";
                searchTextBox.Text = "Listening...";
                _isListening = true;

                await StartSpeechRecognitionAsync();
            }
        }


        private async Task StartSpeechRecognitionAsync()
        {
            try
            {
                // Ensure the app is visible while listening
                this.AppWindow.Show(true);

                _speechRecognizer = new SpeechRecognizer();
                await _speechRecognizer.CompileConstraintsAsync();

                // Start recognition
                SpeechRecognitionResult result = await _speechRecognizer.RecognizeAsync();

                if (result.Status == SpeechRecognitionResultStatus.Success)
                {
                    searchTextBox.DispatcherQueue.TryEnqueue(() =>
                    {
                        searchTextBox.ClearUndoRedoHistory();
                        searchTextBox.Text = result.Text;
                    });
                }
                else
                {
                    searchTextBox.DispatcherQueue.TryEnqueue(() =>
                    {
                        searchTextBox.Text = "Could not recognize speech.";
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Speech Recognition Error: {ex.Message}");
                searchTextBox.DispatcherQueue.TryEnqueue(() =>
                {
                    searchTextBox.Text = "Speech Recognition not supported.";
                });
            }
            finally
            {
                _isListening = false;
                ListenIcon.Glyph = "\uF781";

                if (_speechRecognizer != null)
                {
                    _speechRecognizer.Dispose();
                    _speechRecognizer = null;
                }
            }
        }

        private void MainGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                this.Close();
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == WindowActivationState.Deactivated)
            {
                this.Close();
            }
        }

        private List<Run> GenerateSearchResults(string a, string b)
        {
            var runs = new List<Run>();

            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                runs.Add(new Run { Text = b });
                return runs;
            }

            string pattern = Regex.Escape(a);
            var matches = Regex.Matches(b, pattern, RegexOptions.IgnoreCase);
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                if (match.Index > lastIndex)
                {
                    runs.Add(new Run { Text = b.Substring(lastIndex, match.Index - lastIndex) });
                }

                runs.Add(new Run
                {
                    Text = match.Value,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold
                });

                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < b.Length)
            {
                runs.Add(new Run { Text = b.Substring(lastIndex) });
            }

            return runs;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is Shortcut shortcut)
            {
                // Check which ListView contains this TextBlock
                bool isInShortcutsList = IsInVisualTree(textBlock, shortcutsList);
                bool isInKeyList = IsInVisualTree(textBlock, keyList);

                // Only add suffix if in the main list
                if (isInShortcutsList)
                {
                    textBlock.Inlines.Clear();
                    foreach (var run in shortcut.SearchResults)
                    {
                        textBlock.Inlines.Add(run);
                    }
                }
                else if (isInKeyList)
                {
                    textBlock.Inlines.Clear();
                    textBlock.Inlines.Add(new Run
                    {
                        Text = shortcut.KeysConcatenation,
                        FontWeight = FontWeights.SemiBold
                    });
                }
            }
        }

        // Helper to check if an element exists in a specific ListView's visual tree
        private bool IsInVisualTree(DependencyObject element, DependencyObject parent)
        {
            while (element != null)
            {
                if (element == parent)
                    return true;
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }
    }
}