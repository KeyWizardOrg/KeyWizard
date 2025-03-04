using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Key_Wizard.search;
using Key_Wizard.shortcuts;
using Key_Wizard.startup;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics;
using Windows.System;
using WinRT.Interop;

namespace Key_Wizard
{
    public class ListItem
    {
        public string Section { get; set; }  // Section
        public string Prefix { get; set; }  // Bold part
        public string Suffix { get; set; }  // Normal part
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

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public MainWindow()
        {
            // Uncomment the below line to spawn a console window
            // AllocConsole();

            this.InitializeComponent();

            // Get the AppWindow for the current window
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Set the presenter to OverlappedPresenter and disable the maximize, minimize, and close buttons
            if (appWindow != null)
            {
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsMaximizable = false; // Disable the maximize button
                    presenter.IsResizable = true;  // Disable resizing
                    presenter.IsMinimizable = false; // Disable the minimize button
                }
            }

            // Extend the client area into the title bar
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null); // Set to null to remove the default title bar

            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
            shortcutDictionary = CreateDictionary.InitList();
            searchList = CreateDictionary.InitSearch(shortcutDictionary);
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = searchTextBox.Text;
            ObservableCollection<Section> display = new ObservableCollection<Section>();

            List<ListItem> results = Search.FuzzySearch(searchList, searchQuery);
            if (!string.IsNullOrWhiteSpace(searchQuery) && results.Any())
            {
                display.Add(new Section { Name = "Search Results", Items = new ObservableCollection<ListItem>(results) });
            }
            else
            {
                this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
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
            var methodInfo = typeof(Shortcuts).GetMethod(item.Action, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (methodInfo != null)
            {
                var result = methodInfo.Invoke(null, null);
                if (result is Action action)
                {
                    action.Invoke();
                }
                this.Close();
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
    }
}

