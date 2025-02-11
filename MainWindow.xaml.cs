using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using System.Runtime.InteropServices;


namespace Key_Wizard
{
    /// <summary>
    /// Main Window of Key Wizard
    /// ...
    /// </summary>
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
            ExtendsContentIntoTitleBar = true;

            this.AppWindow.MoveAndResize(GetWindowSizeAndPos(0.3, 0.25));

            actions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
            {"Open the Run dialog box", runDialog},
            {"Switch between open windows", altTab },
            {"Capture a full screen screenshot", screenshot },
            {"Open the Settings App", settings }
            };
            AllocConsole();
            shortcutsList.Items.Clear();
            shortcutsList.Items.Add("Test Test");
        }

        private RectInt32 GetWindowSizeAndPos(double widthPercentage, double heightPercentage)
        {
            // Get the window handle
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

            // Get the display area (screen resolution)
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            // Calculate the window size based on the percentage
            int windowWidth = (int)(workArea.Width * widthPercentage);
            int windowHeight = (int)(workArea.Height * heightPercentage);

            // Calculate the window position to center it on the screen
            int windowX = (workArea.Width - windowWidth) / 2;
            int windowY = (workArea.Height - windowHeight) / 2;
            return new RectInt32(windowX, windowY, windowWidth, windowHeight);
        }
        
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            shortcutsList.Visibility = Visibility.Visible;
            myButton.Content = "Clicked";
            string userInput = searchTextBox.Text;
            
            if (actions.TryGetValue(userInput, out Action action))
            {
                action();
            }
            else
            {
                Console.WriteLine("Unknown command.");
            }

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

        private void shortcutsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = shortcutsList.SelectedItem as string;
            System.Diagnostics.Debug.WriteLine(selectedItem);
        }
    }
}
