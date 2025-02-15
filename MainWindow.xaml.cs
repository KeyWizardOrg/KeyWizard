using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        private Dictionary<string, CreateSections> shortcutDictionary;

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

            this.AppWindow.MoveAndResize(GetWindowSizeAndPos(0.3, 0.06));

            shortcutDictionary = new Dictionary<string, CreateSections>();
            shortcutDictionary = CreateDictionary.InitList();
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
        private void AdjustWindowToContent()
        {
            // Force UI update
            MainGrid.UpdateLayout();

            // Calculate total content size
            int contentWidth = this.AppWindow.Size.Width;
            int contentHeight = (int)(shortcutsList.ActualHeight + shortcutsList.Margin.Top + shortcutsList.Margin.Bottom +
                                searchTextBox.ActualHeight + searchTextBox.Margin.Top + searchTextBox.Margin.Bottom +
                                MainGrid.Margin.Top + MainGrid.Margin.Bottom);

            // Get display information
            var workArea = DisplayArea.Primary.WorkArea;

            // Ensure window doesn't exceed screen bounds
            var maxWidth = workArea.Width * MAX_WIDTH; // 10px margin on each side
            var maxHeight = workArea.Height * MAX_HEIGHT;

            contentWidth = (int)Math.Min(contentWidth, maxWidth);
            contentHeight = (int)Math.Min(contentHeight, maxHeight);

            this.AppWindow.MoveAndResize(new RectInt32(
                (workArea.Width - contentWidth) / 2,
                (workArea.Height - contentHeight) / 2,
                contentWidth,
                contentHeight
            ));
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            shortcutsList.Visibility = Visibility.Visible;

            ObservableCollection<Section> sections = new ObservableCollection<Section>();

            // Populate the ListView with the key-action pairs
            foreach (var section in shortcutDictionary)
            {
                ObservableCollection<ListItem> items = new ObservableCollection<ListItem>();

                foreach (var keyAction in section.Value.Data)
                {
                    ListItem newItem = new ListItem { Prefix = $"{keyAction.Key}: ", Suffix = $"{keyAction.Value.action}", Action = $"{keyAction.Value.function}" };
                    items.Add(newItem);
                }

                sections.Add(new Section { Name = section.Key, Items = items });
            }

            shortcutsList.ItemsSource = sections;

            // Resize the window
            AdjustWindowToContent();
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
                Action action = (Action)Delegate.CreateDelegate(typeof(Action), methodInfo);
                action.Invoke();
                this.Close();
            }
            // Right now there is no error handling for if the action doesn't exist as we do not want any
            // errors during the demo. We will add some error handling once we have all the actions implemented.
        }

        
    }
}
