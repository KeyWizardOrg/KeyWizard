using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Key_Wizard.backend.search;
using Key_Wizard.backend.shortcuts;
using Key_Wizard.screen;
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
    /**
     * This is the window the user sees and interacts with
     */
    public sealed partial class MainWindow : Window
    {
        // used by the MinimizeWindow() function
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MINIMIZE = 6;

        // used to control window resizing
        private const int MAX_VISIBLE_ITEMS = 10;
        private const double ITEM_HEIGHT = 40;

        // used to handle search
        private List<Category> categories;
        private List<Shortcut> searchList;
        private DispatcherTimer searchDelayTimer;
        private const int SEARCH_DELAY_MS = 200;

        // used for voice control
        private SpeechRecognizer? _speechRecognizer;
        private bool _isListening = false;

        public MainWindow()
        {
            this.InitializeComponent();

            // set up search timer
            searchDelayTimer = new DispatcherTimer();
            searchDelayTimer.Interval = TimeSpan.FromMilliseconds(SEARCH_DELAY_MS);
            searchDelayTimer.Tick += SearchDelayTimer_Tick;

            // get AppWindow() object for manipulation
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // do not allow alt-tabbing into key wizard
            appWindow.IsShownInSwitchers = false;

            // set up presenter for app window
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

            // remove title bar
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null);

            // resize window to match screen
            this.AppWindow.MoveAndResize(ScreenHelper.GetWindowSizeAndPos(this, ScreenHelper.MIN_WIDTH, ScreenHelper.MIN_HEIGHT));

            // add event handler for when window is closed
            this.Closed += MainWindow_Closed;

            // load categories list and shortcuts list
            this.categories = ReadShortcuts.Read();
            this.searchList = categories.SelectMany(category => category.Shortcuts).ToList();
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            // discard speech recognition info
            if (_isListening && _speechRecognizer != null)
            {
                await _speechRecognizer.StopRecognitionAsync();
                _speechRecognizer.Dispose();
                _speechRecognizer = null;
                _isListening = false;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // show/hide clear button based on whether there's text
            ClearSearchButton.Visibility = string.IsNullOrWhiteSpace(searchTextBox.Text)
                ? Visibility.Collapsed
                : Visibility.Visible;

            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            // clear search box text
            searchTextBox.Text = string.Empty;
        }

        /*
         * Run when search delay timer has elapsed, i.e. it has been long enough
         * since the user entered an input, that we feel comfortable searching.
         */
        private void SearchDelayTimer_Tick(object? sender, object? e)
        {
            searchDelayTimer.Stop();
            string searchQuery = searchTextBox.Text;
            ObservableCollection<Category> display = new ObservableCollection<Category>();
            ObservableCollection<Category> keyDisplay = new ObservableCollection<Category>();

            // obtain results from shortcut query
            List<Shortcut> results = Search.Do(searchList, searchQuery);
            // highlight in bold matching sections
            results.ForEach((shortcut) => shortcut.SearchResults = GenerateSearchResults(searchQuery, shortcut.Description));

            // if there are results
            if (!string.IsNullOrWhiteSpace(searchQuery) && results.Count != 0)
            {
                // display the results
                display.Add(new Category { Name = "Search Results", Shortcuts = new ObservableCollection<Shortcut>(results) });
                keyDisplay.Add(new Category { Name = "", Shortcuts = new ObservableCollection<Shortcut>(results) });
                ResultsBorderBar.Visibility = Visibility.Visible;

                // calculate visible count, ensuring there are no more than the max
                int visibleCount = Math.Min(results.Count, MAX_VISIBLE_ITEMS);
                ResizeWindowForVisibleItems(visibleCount);
            }
            // else if there are no search results, collapse list
            else
            {
                keyDisplay.Add(new Category { Name = "", Shortcuts = new ObservableCollection<Shortcut>() });
                this.AppWindow.MoveAndResize(ScreenHelper.GetWindowSizeAndPos(this, ScreenHelper.MIN_WIDTH, ScreenHelper.MIN_HEIGHT));
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
                // if the current inner ListView is on its first item, focus the search textbox
                if (listView.SelectedIndex == 0)
                {
                    searchTextBox.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // when item is clicked, trigger action
            var item = (Shortcut)e.ClickedItem;
            TriggerAction(item);
        }

        private void TriggerAction(Shortcut shortcut)
        {
            // iterate through keys, adding each to a list of corresponding codes
            List<byte> keys = new();
            foreach (var key in shortcut.Keys)
            {
                var fieldInfo = typeof(Keys).GetField(key, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (fieldInfo == null)
                {
                    searchTextBox.Text = $"Unable to find key '{key}'";
                    return;
                }

                var keyByte = fieldInfo.GetValue(null);
                if (keyByte == null)
                {
                    searchTextBox.Text = "Unknown error";
                    return;
                }

                keys.Add((byte)keyByte);
            }

            // minimise window, clear history
            backend.shortcuts.Keys.FocusWindowBehind(this);
            MainGrid.UpdateLayout();
            double contentWidth = this.AppWindow.Size.Width;
            double contentHeight = 0.0;
            var workArea = DisplayArea.Primary.WorkArea;
            this.AppWindow.MoveAndResize(ScreenHelper.GetWindowSizeAndPos(this, contentWidth / workArea.Width, contentHeight / workArea.Height));
            MinimizeWindow();
            searchTextBox.Text = "";
            searchTextBox.ClearUndoRedoHistory();

            // press each key
            foreach (var key in keys)
            {
                Keys.Press(key);
            }
            // release each key in reverse order
            keys.Reverse();
            foreach (var key in keys)
            {
                Keys.Release(key);
            }
        }

        /**
         * Toggle status of voice input
         */
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


        // Add this modification to your StartSpeechRecognitionAsync method
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
                    string recognizedText = result.Text;

                    searchTextBox.DispatcherQueue.TryEnqueue(() =>
                    {
                        searchTextBox.ClearUndoRedoHistory();
                        searchTextBox.Text = recognizedText;

                        // Check for exact match with any shortcut description
                        var exactMatch = searchList.FirstOrDefault(s =>
                            string.Equals(s.Description, recognizedText, StringComparison.OrdinalIgnoreCase));

                        if (exactMatch != null)
                        {
                            // Auto-trigger the shortcut
                            TriggerAction(exactMatch);
                        }
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

        /**
         * Makes matching search results display with the matching parts bolded.
         * 
         * WinUI provides no method of doing this declaratively, instead we need to use
         * this function to inject the search results.
         */
        private List<Run> GenerateSearchResults(string a, string b)
        {
            var runs = new List<Run>();

            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                runs.Add(new Run { Text = b });
                return runs;
            }

            // get matching sections
            string pattern = Regex.Escape(a);
            var matches = Regex.Matches(b, pattern, RegexOptions.IgnoreCase);

            // hold the index of the end of the last matching substring
            int lastIndex = 0;

            // for each instance where the query is found in a list item
            foreach (Match match in matches)
            {
                // add text before the match
                if (match.Index > lastIndex)
                {
                    runs.Add(new Run { Text = b.Substring(lastIndex, match.Index - lastIndex) });
                }

                // add the match, displayed in bold
                runs.Add(new Run
                {
                    Text = match.Value,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold
                });

                // move the index to the end of current match 
                lastIndex = match.Index + match.Length;
            }

            // add text after the match
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
                // check which ListView contains this TextBlock
                bool isInShortcutsList = IsInVisualTree(textBlock, shortcutsList);
                bool isInKeysList = IsInVisualTree(textBlock, keyList);

                // only add suffix if in the main list
                if (isInShortcutsList)
                {
                    textBlock.Inlines.Clear();

                    // add category if it exists
                    if (!string.IsNullOrEmpty(shortcut.Category))
                    {
                        textBlock.Inlines.Add(new Run
                        {
                            Text = $"{shortcut.Category}: ",
                            FontWeight = FontWeights.SemiBold,
                            Foreground = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseMediumLowBrush"]
                        });
                    }

                    // add the description (with search highlighting if available)
                    if (shortcut.SearchResults != null)
                    {
                        foreach (var run in shortcut.SearchResults)
                        {
                            textBlock.Inlines.Add(run);
                        }
                    }
                    else
                    {
                        textBlock.Inlines.Add(new Run { Text = shortcut.Description });
                    }
                }
                else if (isInKeysList)
                {
                    if (textBlock.Text == "")
                    {
                        FrameworkElement? parentElement = textBlock.Parent as FrameworkElement;
                        if (parentElement != null)
                        {
                            parentElement.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        /**
         * Helper to check if an element exists in a specific ListView's visual tree
         */
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

        /**
         * Helper method for recursive Visual Tree search.
         */
        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                {
                    return correctlyTyped;
                }
                T? childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        /** 
         * Resize the window based on the number of visible items.
         */
        private void ResizeWindowForVisibleItems(int itemsCount)
        {
            // ensure the layout is updated so we have current ActualWidth values.
            MainGrid.UpdateLayout();

            var workArea = DisplayArea.Primary.WorkArea;

            // use the predefined ITEM_HEIGHT for calculating the height of the items.
            double listHeight = itemsCount * ITEM_HEIGHT;

            double extraHeight = searchTextBox.ActualHeight
                                 + searchTextBox.Margin.Top
                                 + searchTextBox.Margin.Bottom
                                 + MainGrid.Margin.Top
                                 + MainGrid.Margin.Bottom;
            double newHeight = extraHeight + listHeight;

            // use the actual width of MainGrid to adjust the window width.
            double newWidth = MainGrid.ActualWidth;
            if (newWidth <= 0)
            {
                // fallback to the current width if ActualWidth is not available.
                newWidth = this.AppWindow.Size.Width;
            }

            this.AppWindow.MoveAndResize(ScreenHelper.GetWindowSizeAndPos(this, newWidth / workArea.Width, newHeight / workArea.Height));
        }

        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down && shortcutsList.Items.Count > 0)
            {
                ListViewItem? firstCategoryContainer = shortcutsList.ContainerFromIndex(0) as ListViewItem;
                if (firstCategoryContainer != null)
                {
                    // find the inner (nested) ListView within the first category's visual tree.
                    ListView? innerListView = FindVisualChild<ListView>(firstCategoryContainer);
                    if (innerListView != null && innerListView.Items.Count > 0)
                    {
                        // get the container for the top item in the inner ListView.
                        ListViewItem? topItem = innerListView.ContainerFromIndex(0) as ListViewItem;
                        // set focus to the top item.
                        topItem?.Focus(FocusState.Programmatic);
                    }
                }
            }
        }

        private void MinimizeWindow()
        {
            var hWnd = WindowNative.GetWindowHandle(this);
            ShowWindow(hWnd, SW_MINIMIZE);
        }
    }
}