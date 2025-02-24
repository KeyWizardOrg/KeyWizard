using System;
using System.Collections.Generic;
using System.Linq;
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

        // DEFINE KEYS HERE
        public const int TAB = 0x09;
        public const int CTRL = 0x11;
        public const int ALT = 0x12;
        public const int DOWN = 0x28;
        public const int PRTSCR = 0x2C;
        public const int A = 0x41;
        public const int B = 0x42;
        public const int D = 0x44;
        public const int F = 0x46;
        public const int Q = 0x51;
        public const int WIN = 0x5B;
        public const int SHIFT = 0xA0;
        public const int ENTER = 0x0D;
        public const int Spacebar = 0x20;

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
