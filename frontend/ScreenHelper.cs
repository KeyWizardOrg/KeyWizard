using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.Graphics;
using WinRT.Interop;

namespace Key_Wizard.screen
{
    internal class ScreenHelper
    {
        // Constants for different aspect ratios
        public const double MAX_HEIGHT = 0.4;
        public const double MAX_WIDTH = 0.35;
        public const double MIN_HEIGHT = 0.06;
        public const double MIN_WIDTH = 0.35;
        public const double ABSOLUTE_MIN_HEIGHT = 50;

        // Aspect ratio reference points
        public const double ULTRAWIDE_RATIO = 1.78; // 16:9
        public const double STANDARD_RATIO = 1.33; // 4:3

        /*
         * Calculates the size and position of the window based on the screen width and height
         */
        public static RectInt32 GetWindowSizeAndPos(MainWindow mainWindow, double widthPercentage, double heightPercentage)
        {
            var hWnd = WindowNative.GetWindowHandle(mainWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            // Calculate aspect ratio of the screen
            double screenAspectRatio = (double)workArea.Width / workArea.Height;

            // Base window size calculation
            var baseWindowWidth = workArea.Width * widthPercentage;
            var baseWindowHeight = workArea.Height * heightPercentage;

            // Adjust dimensions based on aspect ratio
            double windowWidth, windowHeight;

            if (screenAspectRatio > ULTRAWIDE_RATIO) // Ultrawide or wider (> 16:9)
            {
                // For wider screens, reduce the width percentage proportionally
                double adjustPercentage = 1.0 + (screenAspectRatio - ULTRAWIDE_RATIO) * 0.01;
                windowWidth = baseWindowWidth * adjustPercentage;
                windowHeight = baseWindowHeight * adjustPercentage;
            }
            else if (screenAspectRatio < STANDARD_RATIO) // Narrower than 4:3
            {
                // For taller screens, adjust height
                double adjustPercentage = 1.0 + (STANDARD_RATIO - screenAspectRatio) * 0.01;
                windowWidth = baseWindowWidth * adjustPercentage;
                windowHeight = baseWindowHeight * adjustPercentage;
            }
            else
            {
                // For standard aspect ratios (between 4:3 and 16:9)
                windowWidth = baseWindowWidth;
                windowHeight = baseWindowHeight;
            }

            // Calculate base max and min dimensions
            var maxWidth = workArea.Width * MAX_WIDTH;
            var maxHeight = workArea.Height * MAX_HEIGHT;
            var minWidth = workArea.Width * MIN_WIDTH;
            var minHeight = workArea.Height * MIN_HEIGHT;

            // Apply constraints
            windowWidth = Math.Min(windowWidth, maxWidth);
            windowHeight = Math.Min(windowHeight, maxHeight);

            windowWidth = Math.Max(windowWidth, minWidth);
            windowHeight = Math.Max(windowHeight, minHeight);

            windowHeight = Math.Max(windowHeight, ABSOLUTE_MIN_HEIGHT);

            // Centre the window on screen
            int windowX = (int)(workArea.Width - windowWidth) / 2;
            int windowY = (int)(workArea.Height - maxHeight) / 2;

            return new RectInt32(windowX, windowY, (int)windowWidth, (int)windowHeight);
        }
    }
}
