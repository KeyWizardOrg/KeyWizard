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
        public const int C = 0x43;
        public const int D = 0x44;
        public const int F = 0x46;
        public const int Q = 0x51;
        public const int WIN = 0x5B;
        public const int SHIFT = 0xA0;
<<<<<<< HEAD
        public const int ENTER = 0x0D;
        public const int Spacebar = 0x20;
=======
        public const int K = 0x4B;
        public const int H = 0x48;
        public const int UP = 0x26;
        public const int COMMA = 0xBC;
        public const int C = 0x43;

>>>>>>> 5eacf4c4a4359b84b376bec35e03166f82cb76dd

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
