using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;

namespace Key_Wizard.shortcuts
{
    public class Shortcuts
    {

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int VK_TAB = 0x09;
        private const int VK_MENU = 0x12; // Alt key
        const byte VK_LSHIFT = 0xA0; // Left Shift key
        const byte VK_CONTROL = 0x11;  // Ctrl key
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int VK_C = 0x43; // C

        private const uint GW_HWNDNEXT = 2;

        private static void FocusWindowBehind(MainWindow window)
        {
            SetForegroundWindow(GetWindow(WindowNative.GetWindowHandle(window), GW_HWNDNEXT));
        }

        // Opens Run dialog
        public static void windowsKeyR(MainWindow window)
        {
            Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
        }

        // Switches to previous window
        public static void altTab(MainWindow window)
        {
            keybd_event(VK_MENU, 0, 0, IntPtr.Zero);  // Press Alt
            keybd_event(VK_TAB, 0, 0, IntPtr.Zero);   // Press Tab
            keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, IntPtr.Zero);  // Release Tab
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, IntPtr.Zero); // Release Alt
        }

        // Opens screenshot tool
        public static void PrtSc(MainWindow window)
        {
            keybd_event(0x5B, 0, 0, IntPtr.Zero);     // Windows key down
            keybd_event(0x2C, 0, 0, IntPtr.Zero);     // Print Screen
            keybd_event(0x2C, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(0x5B, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        // Opens the Settings menu
        public static void windowsKeyI(MainWindow window)
        {
            Process.Start("explorer.exe", "ms-settings:");
        }
        public static void ctrlAltTab(MainWindow window)
        {
            keybd_event(VK_CONTROL, 0, 0, IntPtr.Zero);//Press ctrl
            keybd_event(VK_MENU, 0, 0, IntPtr.Zero);// Press Shift
            keybd_event(VK_TAB, 0, 0, IntPtr.Zero);//Press Tab

            // Release 
            keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        public static void ctrlC(MainWindow window)
        {
            FocusWindowBehind(window);

            keybd_event(VK_CONTROL, 0, 0, IntPtr.Zero);//Press ctrl
            keybd_event(VK_C, 0, 0, IntPtr.Zero);// Press C

            // Release 
            keybd_event(VK_C, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
    }
}
