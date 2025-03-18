using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Key_Wizard.search;
using Key_Wizard.shortcuts;
using Key_Wizard.startup;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics;
using Windows.Media.SpeechRecognition;
using Windows.System;
using WinRT.Interop;

namespace Key_Wizard
{
    public class ListItem
    {
        public string Section { get; set; }  // Section
        public string Prefix { get; set; }  // Bold part
        public string Suffix { get; set; }  // Normal part
        public List<Run> HighlightedRuns { get; set; } // Highlighted search result
        public string Action { get; set; }  // Function to be triggered on click
    }

    public class Section
    {
        public string Name { get; set; }
        public ObservableCollection<ListItem> Items { get; set; }
    }
    public sealed partial class MainWindow : Window
    {
        private Dictionary<string, CreateSections> shortcutDictionary;
        private List<ListItem> searchList;
        private DispatcherTimer searchDelayTimer;
        private const int SEARCH_DELAY_MS = 300; // 300ms delay, adjust as needed

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public MainWindow()
        {
            // Uncomment the below line to spawn a console window
            // AllocConsole();

            this.InitializeComponent();

            // Initialize the search delay timer
            searchDelayTimer = new DispatcherTimer();
            searchDelayTimer.Interval = TimeSpan.FromMilliseconds(SEARCH_DELAY_MS);
            searchDelayTimer.Tick += SearchDelayTimer_Tick;

            // Get the AppWindow for the current window
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Hide the window from the taskbar and Alt+Tab
            appWindow.IsShownInSwitchers = false;

            // Set the presenter to OverlappedPresenter and disable the maximize, minimize, and close buttons
            if (appWindow != null)
            {
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsMaximizable = false; // Disable the maximize button
                    presenter.IsResizable = false;  // Disable resizing
                    presenter.IsMinimizable = false; // Disable the minimize button
                    presenter.SetBorderAndTitleBar(true, false);
                 }
            }

            // Extend the client area into the title bar
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null); // Set to null to remove the default title bar

            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
            shortcutDictionary = CreateDictionary.InitList();
            searchList = CreateDictionary.InitSearch(shortcutDictionary);
        }
        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Reset and restart the timer
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void SearchDelayTimer_Tick(object sender, object e)
        {
            // Stop the timer
            searchDelayTimer.Stop();

            // Now perform the search
            string searchQuery = searchTextBox.Text;
            ObservableCollection<Section> display = new ObservableCollection<Section>();

            List<ListItem> results = NewSearch.Search(searchList, searchQuery);
            results.ForEach((item) => item.HighlightedRuns = GenerateHighlightedSuffixes(searchQuery, item.Suffix));
            if (!string.IsNullOrWhiteSpace(searchQuery) && results.Any())
            {
                display.Add(new Section { Name = "Search Results", Items = new ObservableCollection<ListItem>(results) });
                ResultsBorderBar.Visibility = Visibility.Visible;
            }
            else
            {
                this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
                ResultsBorderBar.Visibility = Visibility.Collapsed;
            }

            shortcutsList.ItemsSource = display;
        }
        private void ListView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Space)
            {
                var listView = (ListView)sender;
                var item = (ListItem)listView.SelectedItem;
                TriggerAction(item);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (ListItem)e.ClickedItem;
            TriggerAction(item);
        }

        private void TriggerAction(ListItem item)
        {
            try
            {
                var shortcuts = new Shortcuts(this);

                var methodInfo = typeof(Shortcuts).GetMethod(item.Action,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(shortcuts, null);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
            // Right now there is no error handling for if the action doesn't exist as we do not want any
            // errors during the demo. We will add some error handling once we have all the actions implemented.
        }


        private void shortcutsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Force UI update
            MainGrid.UpdateLayout();

            // Calculate total content size
            int contentWidth = this.AppWindow.Size.Width;
            int contentHeight = (int)(searchTextBox.ActualHeight + searchTextBox.Margin.Top + searchTextBox.Margin.Bottom +
                                      MainGrid.Margin.Top + MainGrid.Margin.Bottom + e.NewSize.Height);

            // Get display information
            var workArea = DisplayArea.Primary.WorkArea;
            double screenAspectRatio = (double)workArea.Width / workArea.Height;

            // Calculate base max and min dimensions
            var maxWidth = workArea.Width * Screen.MAX_WIDTH;
            var maxHeight = workArea.Height * Screen.MAX_HEIGHT;
            var minWidth = workArea.Width * Screen.MIN_WIDTH;
            var minHeight = workArea.Height * Screen.MIN_HEIGHT;

            // Adjust for aspect ratio
            var (adjustedMaxWidth, adjustedMaxHeight) = Screen.AdjustForAspectRatio(
                (int)maxWidth,
                (int)maxHeight,
                screenAspectRatio,
                Screen.MAX_WIDTH,
                Screen.MAX_HEIGHT);

            var (adjustedMinWidth, adjustedMinHeight) = Screen.AdjustForAspectRatio(
                (int)minWidth,
                (int)minHeight,
                screenAspectRatio,
                Screen.MIN_WIDTH,
                Screen.MIN_HEIGHT);

            // Apply constraints
            contentWidth = Math.Min(contentWidth, adjustedMaxWidth);
            contentHeight = Math.Min(contentHeight, adjustedMaxHeight);

            contentWidth = Math.Max(contentWidth, adjustedMinWidth);
            contentHeight = Math.Max(contentHeight, adjustedMinHeight);

            // Center the window
            this.AppWindow.MoveAndResize(new RectInt32(
                (workArea.Width - contentWidth) / 2,
                (workArea.Height - contentHeight) / 2,
                contentWidth,
                contentHeight
            ));
        }
        private async void VoiceInput(object sender, RoutedEventArgs e)
        {

            //ListenIcon.Glyph = "\xEC71";

            if (ListenIcon.Glyph == "\xEC71")
            {
                ListenIcon.Glyph = "\xE720";
            }
            else
            {
               ListenIcon.Glyph = "\xEC71";
            }

            /*
            try
            {
                using (SpeechRecognizer speechRecognizer = new SpeechRecognizer())
                {
                    // Compile the recognizer
                    await speechRecognizer.CompileConstraintsAsync();

                    // Update UI to indicate listening
                    searchTextBox.Text = "Listening...";

                    // Start listening
                    SpeechRecognitionResult result = await speechRecognizer.RecognizeAsync();

                    if (result.Status == SpeechRecognitionResultStatus.Success)
                    {
                        searchTextBox.ClearUndoRedoHistory();
                        searchTextBox.Text = result.Text;  // Update textbox with recognized text
                    }
                    else
                    {
                        searchTextBox.Text = "Could not recognize speech.";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Speech Recognition Error: {ex.Message}");
                searchTextBox.Text = "Speech Recognition not supported.";
            }
            */
        }

        private void MainGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                this.Close(); // Close the app when ESC is pressed
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == WindowActivationState.Deactivated)
            {
                this.Close(); // Close the app when focus is lost
            }
        }

        private List<Run> GenerateHighlightedSuffixes(string a, string b)
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
                // text before the match
                if (match.Index > lastIndex)
                {
                    runs.Add(new Run { Text = b.Substring(lastIndex, match.Index - lastIndex) });
                }

                // matched text
                runs.Add(new Run
                {
                    Text = match.Value,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold
                });

                lastIndex = match.Index + match.Length;
            }

           // text after the match
            if (lastIndex < b.Length)
            {
                runs.Add(new Run { Text = b.Substring(lastIndex) });
            }

            return runs;
        }

        // Manually inject the suffixes into the text box
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is ListItem listItem)
            {
                foreach (var run in listItem.HighlightedRuns)
                {
                    textBlock.Inlines.Add(run);
                }
            }
        }
    }
}

