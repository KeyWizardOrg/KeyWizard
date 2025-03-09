using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.Graphics;
using WinRT.Interop;

namespace Key_Wizard.startup
{
    internal class Screen
    {
        // Constants for different aspect ratios
        public const Double MAX_HEIGHT = 0.3;
        public const Double MAX_WIDTH = 0.3;
        public const Double MIN_HEIGHT = 0.06;
        public const Double MIN_WIDTH = 0.3;

        // Aspect ratio reference points
        public const Double ULTRAWIDE_RATIO = 1.78; // 16:9
        public const Double STANDARD_RATIO = 1.33; // 4:3

        public static RectInt32 GetWindowSizeAndPos(MainWindow mainWindow, double widthPercentage, double heightPercentage)
        {
            var hWnd = WindowNative.GetWindowHandle(mainWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            // Calculate aspect ratio of the screen
            double screenAspectRatio = (double)workArea.Width / workArea.Height;

            // Base window size calculation
            int baseWindowWidth = (int)(workArea.Width * widthPercentage);
            int baseWindowHeight = (int)(workArea.Height * heightPercentage);

            // Adjust dimensions based on aspect ratio
            int windowWidth, windowHeight;

            if (screenAspectRatio > ULTRAWIDE_RATIO) // Ultrawide or wider (> 16:9)
            {
                // For wider screens, reduce the width percentage proportionally
                double adjustedWidthPercentage = widthPercentage * (ULTRAWIDE_RATIO / screenAspectRatio);
                windowWidth = (int)(workArea.Width * adjustedWidthPercentage);
                windowHeight = baseWindowHeight;
            }
            else if (screenAspectRatio < STANDARD_RATIO) // Narrower than 4:3
            {
                // For taller screens, adjust height
                double adjustedHeightPercentage = heightPercentage * (screenAspectRatio / STANDARD_RATIO);
                windowWidth = baseWindowWidth;
                windowHeight = (int)(workArea.Height * adjustedHeightPercentage);
            }
            else
            {
                // For standard aspect ratios (between 4:3 and 16:9)
                windowWidth = baseWindowWidth;
                windowHeight = baseWindowHeight;
            }

            // Center the window on screen
            int windowX = (workArea.Width - windowWidth) / 2;
            int windowY = (workArea.Height - windowHeight) / 2;

            return new RectInt32(windowX, windowY, windowWidth, windowHeight);
        }

        public static (int width, int height) AdjustForAspectRatio(int baseWidth, int baseHeight, double screenAspectRatio, double maxWidthPercentage, double maxHeightPercentage)
        {
            int width = baseWidth;
            int height = baseHeight;

            var workArea = DisplayArea.Primary.WorkArea;
            int maxWidth = (int)(workArea.Width * maxWidthPercentage);
            int maxHeight = (int)(workArea.Height * maxHeightPercentage);

            // Apply aspect ratio adjustments
            if (screenAspectRatio > ULTRAWIDE_RATIO) // Ultrawide or wider (> 16:9)
            {
                // Scale down width for ultrawide displays
                double scaleFactor = ULTRAWIDE_RATIO / screenAspectRatio;
                width = Math.Min(width, (int)(maxWidth * scaleFactor));
            }
            else if (screenAspectRatio < STANDARD_RATIO) // Narrower than 4:3
            {
                // Scale down height for tall displays
                double scaleFactor = screenAspectRatio / STANDARD_RATIO;
                height = Math.Min(height, (int)(maxHeight * scaleFactor));
            }

            return (width, height);
        }
    }
}
