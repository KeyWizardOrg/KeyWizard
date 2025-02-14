using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;
using WinRT.Interop;

namespace Key_Wizard
{
    public class ListItem
    {
        public string Prefix { get; set; }  // Bold part
        public string Suffix { get; set; }  // Normal part
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
        private Dictionary<string, Action> actions;
        private Dictionary<string, CreateSections> shortcutDictionary;

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        private const int VK_TAB = 0x09;
        private const int VK_MENU = 0x12; // Alt key
        private const int KEYEVENTF_KEYUP = 0x0002;

        public MainWindow()
        {
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

            actions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                {"Open the Run dialog box", runDialog},
                {"Switch between open windows", altTab },
                {"Capture a full screen screenshot", screenshot },
                {"Open the Settings App", settings }
            };

            AllocConsole();
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

        public void runDialog()
        {
            Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
        }

        public void altTab()
        {
            keybd_event(VK_MENU, 0, 0, IntPtr.Zero);  // Press Alt
            keybd_event(VK_TAB, 0, 0, IntPtr.Zero);   // Press Tab
            keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, IntPtr.Zero);  // Release Tab
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, IntPtr.Zero); // Release Alt
        }

        public void screenshot()
        {
            keybd_event(0x5B, 0, 0, IntPtr.Zero);     // Windows key down
            keybd_event(0x2C, 0, 0, IntPtr.Zero);     // Print Screen
            keybd_event(0x2C, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(0x5B, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        public void settings()
        {
            Process.Start("explorer.exe", "ms-settings:");
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
                    ListItem newItem = new ListItem { Prefix = $"{keyAction.Key}: ", Suffix = $"{keyAction.Value}" };
                    items.Add(newItem);
                }

                sections.Add(new Section { Name = section.Key, Items = items });
            }

            shortcutsList.ItemsSource = sections;

            // Resize the window
            AdjustWindowToContent();
        }
    }
}
