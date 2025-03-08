using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key_Wizard.shortcuts
{
    internal class Keys
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);
        private const int KEYUP = 0x0002;

        public const int BACKSPACE = 0x08;
        public const int TAB = 0x09;
        public const int ENTER = 0x0D;
        public const int SHIFT = 0x10;
        public const int CTRL = 0x11;
        public const int ALT = 0x12;
        public const int PAUSE = 0x13;
        public const int CAPSLOCK = 0x14;
        public const int ESC = 0x1B;
        public const int SPACEBAR = 0x20;
        public const int PAGEUP = 0x21;
        public const int PAGEDOWN = 0x22;
        public const int END = 0x23;
        public const int HOME = 0x24;
        public const int LEFT = 0x25;
        public const int UP = 0x26;
        public const int RIGHT = 0x27;
        public const int DOWN = 0x28;
        public const int SELECT = 0x29;
        public const int PRINT = 0x2A;
        public const int EXECUTE = 0x2B;
        public const int PRTSCR = 0x2C;
        public const int INSERT = 0x2D;
        public const int DELETE = 0x2E;
        public const int HELP = 0x2F;
        public const int WIN = 0x5B;
        public const int WIN_RIGHT = 0x5C;
        public const int CONTEXT_MENU = 0x5D;
        public const int NUMLOCK = 0x90;
        public const int SCROLLLOCK = 0x91;
        public const int F1 = 0x70;
        public const int F2 = 0x71;
        public const int F3 = 0x72;
        public const int F4 = 0x73;
        public const int F5 = 0x74;
        public const int F6 = 0x75;
        public const int F7 = 0x76;
        public const int F8 = 0x77;
        public const int F9 = 0x78;
        public const int F10 = 0x79;
        public const int F11 = 0x7A;
        public const int F12 = 0x7B;

        // Alphabet keys
        public const int A = 0x41;
        public const int B = 0x42;
        public const int C = 0x43;
        public const int D = 0x44;
        public const int E = 0x45;
        public const int F = 0x46;
        public const int G = 0x47;
        public const int H = 0x48;
        public const int I = 0x49;
        public const int J = 0x4A;
        public const int K = 0x4B;
        public const int L = 0x4C;
        public const int M = 0x4D;
        public const int N = 0x4E;
        public const int O = 0x4F;
        public const int P = 0x50;
        public const int Q = 0x51;
        public const int R = 0x52;
        public const int S = 0x53;
        public const int T = 0x54;
        public const int U = 0x55;
        public const int V = 0x56;
        public const int W = 0x57;
        public const int X = 0x58;
        public const int Y = 0x59;
        public const int Z = 0x5A;

        // Number keys
        public const int ZERO = 0x30;
        public const int ONE = 0x31;
        public const int TWO = 0x32;
        public const int THREE = 0x33;
        public const int FOUR = 0x34;
        public const int FIVE = 0x35;
        public const int SIX = 0x36;
        public const int SEVEN = 0x37;
        public const int EIGHT = 0x38;
        public const int NINE = 0x39;

        // Numpad keys
        public const int NUMPAD0 = 0x60;
        public const int NUMPAD1 = 0x61;
        public const int NUMPAD2 = 0x62;
        public const int NUMPAD3 = 0x63;
        public const int NUMPAD4 = 0x64;
        public const int NUMPAD5 = 0x65;
        public const int NUMPAD6 = 0x66;
        public const int NUMPAD7 = 0x67;
        public const int NUMPAD8 = 0x68;
        public const int NUMPAD9 = 0x69;
        public const int MULTIPLY = 0x6A;
        public const int ADD = 0x6B;
        public const int SUBTRACT = 0x6D;
        public const int DECIMAL = 0x6E;
        public const int DIVIDE = 0x6F;

        // Special Characters & OEM Keys
        public const int OEM_COMMA = 0xBC;      // ,
        public const int OEM_PERIOD = 0xBE;     // .
        public const int OEM_MINUS = 0xBD;      // -
        public const int OEM_PLUS = 0xBB;       // +
        public const int OEM_1 = 0xBA;          // ;:
        public const int OEM_2 = 0xBF;          // /?
        public const int OEM_3 = 0xC0;          // `~
        public const int OEM_4 = 0xDB;          // [{
        public const int OEM_5 = 0xDC;          // \|
        public const int OEM_6 = 0xDD;          // ]}
        public const int OEM_7 = 0xDE;          // '"



        public static void Press(byte key)
        {
            keybd_event(key, 0, 0, IntPtr.Zero);
        }

        public static void Release(byte key)
        {
            keybd_event(key, 0, KEYUP, IntPtr.Zero);
        }
    }
}
