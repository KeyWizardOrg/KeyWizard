using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Key_Wizard.screen;
using Key_Wizard.search;
using Key_Wizard.shortcuts;
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

        // Constants for controlling resizing
        private const int MAX_VISIBLE_ITEMS = 10;
        private const double ITEM_HEIGHT = 40;

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

            this.categories = ReadShortcuts.Read();
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
            // Show/hide clear button based on whether there's text
            ClearSearchButton.Visibility = string.IsNullOrWhiteSpace(searchTextBox.Text)
                ? Visibility.Collapsed
                : Visibility.Visible;

            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = string.Empty;
            // Also stop voice recognition if active
            
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

                // Calculate visible count based on MAX_VISIBLE_ITEMS
                int visibleCount = Math.Min(results.Count, MAX_VISIBLE_ITEMS);
                ResizeWindowForVisibleItems(visibleCount);
            }
            else
            {
                keyDisplay.Add(new Category { Name = "", Shortcuts = new ObservableCollection<Shortcut>() });
                this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
                ResultsBorderBar.Visibility = Visibility.Collapsed;
            }

            shortcutsList.ItemsSource = display;
            keyList.ItemsSource = keyDisplay;
        }

        private void ListView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var listView = (ListView)sender;
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Space)
            {
                var item = (Shortcut)listView.SelectedItem;
                TriggerAction(item);
            }
            else if (e.Key == VirtualKey.Up)
            {
                // If the current inner ListView is on its first item, focus the search textbox.
                if (listView.SelectedIndex == 0)
                {
                    searchTextBox.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (Shortcut)e.ClickedItem;
            TriggerAction(item);
        }

        private void TriggerAction(Shortcut shortcut)
        {
            List<byte> keys = new();
            foreach (var key in shortcut.Keys)
            {
                var fieldInfo = typeof(Keys).GetField(key, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (fieldInfo == null)
                {
                    Debug.WriteLine("ERROR: Provided key does not exist.");
                    // TODO: Better error handling here
                }
                else
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

        // Removed shortcutsList_SizeChanged method

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
                bool isInKeysList = IsInVisualTree(textBlock, keyList);

                // Only add suffix if in the main list
                if (isInShortcutsList)
                {
                    textBlock.Inlines.Clear();
                    foreach (var run in shortcut.SearchResults)
                    {
                        textBlock.Inlines.Add(run);
                    }
                }
                else if (isInKeysList)
                {
                    if (textBlock.Text == "")
                    {
                        FrameworkElement parentElement = textBlock.Parent as FrameworkElement;
                        parentElement.Visibility = Visibility.Collapsed;
                    }
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

        // Helper method for recursive Visual Tree search.
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                {
                    return correctlyTyped;
                }
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        // Resize the window based on the number of visible items.
        private void ResizeWindowForVisibleItems(int itemsCount)
        {
            // Ensure the layout is updated so we have current ActualWidth values.
            MainGrid.UpdateLayout();

            var workArea = DisplayArea.Primary.WorkArea;

            // Use the predefined ITEM_HEIGHT for calculating the height of the items.
            double listHeight = itemsCount * ITEM_HEIGHT;

            double extraHeight = searchTextBox.ActualHeight
                                 + searchTextBox.Margin.Top
                                 + searchTextBox.Margin.Bottom
                                 + MainGrid.Margin.Top
                                 + MainGrid.Margin.Bottom;
            double newHeight = extraHeight + listHeight;

            // Use the actual width of MainGrid to adjust the window width.
            double newWidth = MainGrid.ActualWidth;
            if (newWidth <= 0)
            {
                // Fallback to the current width if ActualWidth is not available.
                newWidth = this.AppWindow.Size.Width;
            }

            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, newWidth / workArea.Width, newHeight / workArea.Height));
        }

        private void searchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down && shortcutsList.Items.Count > 0)
            {
                ListViewItem firstCategoryContainer = shortcutsList.ContainerFromIndex(0) as ListViewItem;
                if (firstCategoryContainer != null)
                {
                    // Find the inner (nested) ListView within the first category's visual tree.
                    ListView innerListView = FindVisualChild<ListView>(firstCategoryContainer);
                    if (innerListView != null && innerListView.Items.Count > 0)
                    {
                        // Get the container for the top item in the inner ListView.
                        ListViewItem topItem = innerListView.ContainerFromIndex(0) as ListViewItem;
                        // Set focus to the top item.
                        topItem?.Focus(FocusState.Programmatic);
                    }
                }
            }
        }
    }
}