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

namespace Key_Wizard
{
    /// <summary>
    /// Main Window of Key Wizard
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;

            this.AppWindow.MoveAndResize(GetWindowSizeAndPos(0.3, 0.25));
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
            Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
        }
    }
}
