using System;
using System.Collections.Generic;
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
    public sealed partial class MainWindow : Window
    {
        public Dictionary<string, Action> actions;

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

<<<<<<< Updated upstream
            this.AppWindow.MoveAndResize(GetWindowSizeAndPos(0.3, 0.06));
=======
            this.AppWindow.MoveAndResize(Screen.GetWindowSizeAndPos(this, Screen.MIN_WIDTH, Screen.MIN_HEIGHT));
            //searchTextBox.Text = "Voice input";
>>>>>>> Stashed changes

            actions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                {"Open the Run dialog box", runDialog},
                {"Switch between open windows", altTab },
                {"Capture a full screen screenshot", screenshot },
                {"Open the Settings App", settings }
            };

            AllocConsole();
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

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            shortcutsList.Visibility = Visibility.Visible;
            myButton.Content = "Clicked";

            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                // Get the current window size
                var currentSize = appWindow.Size;

                // triple the height of the window
                var newHeight = currentSize.Height * 3;

                // Resize the window
                appWindow.Resize(new Windows.Graphics.SizeInt32(currentSize.Width, newHeight));
            }

<<<<<<< Updated upstream
            // Load the XML data
            var sections = CreateDictionary.InitList();

            // Clear the existing items in the ListView
            shortcutsList.Items.Clear();
=======
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add this line to show/hide clear button
            ClearSearchButton.Visibility = string.IsNullOrEmpty(searchTextBox.Text) ? Visibility.Collapsed : Visibility.Visible;

            // Keep existing code
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "";
            ClearSearchButton.Visibility = Visibility.Collapsed;
            searchTextBox.Focus(FocusState.Programmatic);
        }

        private void SearchDelayTimer_Tick(object sender, object e)
        {
            searchDelayTimer.Stop();
            string searchQuery = searchTextBox.Text;
            ObservableCollection<Category> display = new ObservableCollection<Category>();
            ObservableCollection<Category> keyDisplay = new ObservableCollection<Category>();
>>>>>>> Stashed changes

            // Populate the ListView with the key-action pairs
            foreach (var section in sections)
            {
<<<<<<< Updated upstream
                foreach (var keyAction in section.Value)
=======
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the search text to "KeyWizard" - this will automatically trigger search via TextChanged event
            searchTextBox.Text = "KeyWizard";

            // Focus the search box and place cursor at end
            searchTextBox.Focus(FocusState.Programmatic);
            searchTextBox.Select(searchTextBox.Text.Length, 0);
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
>>>>>>> Stashed changes
                {
                    shortcutsList.Items.Add($"{keyAction.Key}: {keyAction.Value}");
                }
            }
        }

        public void runDialog()
        {
<<<<<<< Updated upstream
            Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
=======
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                this.Close();
            }

>>>>>>> Stashed changes
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
    }
}