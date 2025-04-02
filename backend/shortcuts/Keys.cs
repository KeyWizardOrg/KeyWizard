using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;

namespace Key_Wizard.backend.shortcuts
{
    internal class Keys
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, nint dwExtraInfo);
        private const byte KEYUP = 0x0002;

        public const byte BACKSPACE = 0x08;
        public const byte TAB = 0x09;
        public const byte ENTER = 0x0D;
        public const byte SHIFT = 0x10;
        public const byte CTRL = 0x11;
        public const byte ALT = 0x12;
        public const byte PAUSE = 0x13;
        public const byte CAPSLOCK = 0x14;
        public const byte ESC = 0x1B;
        public const byte SPACEBAR = 0x20;
        public const byte PAGEUP = 0x21;
        public const byte PAGEDOWN = 0x22;
        public const byte END = 0x23;
        public const byte HOME = 0x24;
        public const byte LEFT = 0x25;
        public const byte UP = 0x26;
        public const byte RIGHT = 0x27;
        public const byte DOWN = 0x28;
        public const byte SELECT = 0x29;
        public const byte PRbyte = 0x2A;
        public const byte EXECUTE = 0x2B;
        public const byte PRTSCN= 0x2C;
        public const byte INSERT = 0x2D;
        public const byte DELETE = 0x2E;
        public const byte HELP = 0x2F;
        public const byte WINDOWS = 0x5B;
        public const byte WIN_RIGHT = 0x5C;
        public const byte CONTEXT_MENU = 0x5D;
        public const byte NUMLOCK = 0x90;
        public const byte SCROLLLOCK = 0x91;
        public const byte F1 = 0x70;
        public const byte F2 = 0x71;
        public const byte F3 = 0x72;
        public const byte F4 = 0x73;
        public const byte F5 = 0x74;
        public const byte F6 = 0x75;
        public const byte F7 = 0x76;
        public const byte F8 = 0x77;
        public const byte F9 = 0x78;
        public const byte F10 = 0x79;
        public const byte F11 = 0x7A;
        public const byte F12 = 0x7B;
        public const byte F23 = 0x76;
        public const byte RETURN = 0x0D;


        // Alphabet keys
        public const byte A = 0x41;
        public const byte B = 0x42;
        public const byte C = 0x43;
        public const byte D = 0x44;
        public const byte E = 0x45;
        public const byte F = 0x46;
        public const byte G = 0x47;
        public const byte H = 0x48;
        public const byte I = 0x49;
        public const byte J = 0x4A;
        public const byte K = 0x4B;
        public const byte L = 0x4C;
        public const byte M = 0x4D;
        public const byte N = 0x4E;
        public const byte O = 0x4F;
        public const byte P = 0x50;
        public const byte Q = 0x51;
        public const byte R = 0x52;
        public const byte S = 0x53;
        public const byte T = 0x54;
        public const byte U = 0x55;
        public const byte V = 0x56;
        public const byte W = 0x57;
        public const byte X = 0x58;
        public const byte Y = 0x59;
        public const byte Z = 0x5A;

        // Number keys
        public const byte ZERO = 0x30;
        public const byte ONE = 0x31;
        public const byte TWO = 0x32;
        public const byte THREE = 0x33;
        public const byte FOUR = 0x34;
        public const byte FIVE = 0x35;
        public const byte SIX = 0x36;
        public const byte SEVEN = 0x37;
        public const byte EIGHT = 0x38;
        public const byte NINE = 0x39;

        // Numpad keys
        public const byte NUMPAD0 = 0x60;
        public const byte NUMPAD1 = 0x61;
        public const byte NUMPAD2 = 0x62;
        public const byte NUMPAD3 = 0x63;
        public const byte NUMPAD4 = 0x64;
        public const byte NUMPAD5 = 0x65;
        public const byte NUMPAD6 = 0x66;
        public const byte NUMPAD7 = 0x67;
        public const byte NUMPAD8 = 0x68;
        public const byte NUMPAD9 = 0x69;
        public const byte MULTIPLY = 0x6A;
        public const byte ADD = 0x6B;
        public const byte SUBTRACT = 0x6D;
        public const byte DECIMAL = 0x6E;
        public const byte DIVIDE = 0x6F;

        // Special Characters & OEM Keys
        public const byte OEM_COMMA = 0xBC;      // ,
        public const byte OEM_PERIOD = 0xBE;     // .
        public const byte OEM_MINUS = 0xBD;      // -
        public const byte OEM_PLUS = 0xBB;       // +
        public const byte OEM_1 = 0xBA;          // ;:
        public const byte OEM_2 = 0xBF;          // /?
        public const byte OEM_3 = 0xC0;          // `~
        public const byte OEM_4 = 0xDB;          // [{
        public const byte OEM_5 = 0xDC;          // \|
        public const byte OEM_6 = 0xDD;          // ]}
        public const byte OEM_7 = 0xDE;          // '"

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private MainWindow mainWindow;
        private const uint GW_HWNDNEXT = 2;

        public static void FocusWindowBehind(MainWindow mainWindow)
        {
            IntPtr hWnd = GetWindow(WindowNative.GetWindowHandle(mainWindow), GW_HWNDNEXT);
            while (hWnd != IntPtr.Zero && !IsWindowVisible(hWnd))
            {
                hWnd = GetWindow(hWnd, GW_HWNDNEXT);
            }
            SetForegroundWindow(hWnd);
        }

        public static void Press(byte key)
        {
            keybd_event(key, 0, 0, nint.Zero);
        }

        public static void Release(byte key)
        {
            keybd_event(key, 0, KEYUP, nint.Zero);
        }
    }
}
