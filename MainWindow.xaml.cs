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
        private const Double MAX_HEIGHT = 0.3;
        private const Double MAX_WIDTH = 0.4;
        private const Double MIN_HEIGHT = 0.06;
        private const Double MIN_WIDTH = 0.3;
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

            this.AppWindow.MoveAndResize(GetWindowSizeAndPos(MIN_WIDTH, MIN_HEIGHT));
            shortcutDictionary = CreateDictionary.InitList();
            searchList = CreateDictionary.InitSearch(shortcutDictionary);
        }

        private RectInt32 GetWindowSizeAndPos(double widthPercentage, double heightPercentage)
        {
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            int windowWidth = (int)(workArea.Width * widthPercentage);
            int windowHeight = (int)(workArea.Height * heightPercentage);

            int windowX = (workArea.Width - windowWidth) / 2;
            int windowY = (workArea.Height - windowHeight) / 2;

            return new RectInt32(windowX, windowY, windowWidth, windowHeight);
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
                this.AppWindow.MoveAndResize(GetWindowSizeAndPos(MIN_WIDTH, MIN_HEIGHT));
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

            // Ensure window doesn't exceed screen bounds
            var maxWidth = workArea.Width * MAX_WIDTH;
            var maxHeight = workArea.Height * MAX_HEIGHT;
            var minWidth = workArea.Width * MIN_WIDTH;
            var minHeight = workArea.Height * MIN_HEIGHT;

            contentWidth = (int)Math.Min(contentWidth, maxWidth);
            contentHeight = (int)Math.Min(contentHeight, maxHeight);

            contentWidth = (int)Math.Max(contentWidth, minWidth);
            contentHeight = (int)Math.Max(contentHeight, minHeight);

            this.AppWindow.MoveAndResize(new RectInt32(
                (workArea.Width - contentWidth) / 2,
                (workArea.Height - contentHeight) / 2,
                contentWidth,
                contentHeight
            ));
        }
    }
}

