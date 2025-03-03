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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private MainWindow mainWindow;
        private const uint GW_HWNDNEXT = 2;
        public Shortcuts(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void FocusWindowBehind()
        {
            IntPtr hWnd = GetWindow(WindowNative.GetWindowHandle(mainWindow), GW_HWNDNEXT);
            while (hWnd != IntPtr.Zero && !IsWindowVisible(hWnd))
            {
                hWnd = GetWindow(hWnd, GW_HWNDNEXT);
            }
            SetForegroundWindow(hWnd);
        }
        public int windowsKeyR()
        {
            var process = Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
            return process.Id;
        }
        public int windowsKeyI()
        {
            var process = Process.Start("explorer.exe", "ms-settings:");
            return process.Id;
        }

        public void altTab()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
        }

        public void PrtSc()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.WIN);
        }
       
        public void ctrlAltTab()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.CTRL);
        }

        public void windowsKey()
        {
            Keys.Press(Keys.WIN);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyA()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.A);
            Keys.Release(Keys.A);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltB()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltD()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltDown()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
<<<<<<< HEAD

        public void ctrlC()
        {
            FocusWindowBehind();
=======
        //Jan:<add key="Windows key + Alt + H" action="When voice typing is open, set the focus to the keyboard" function ="windowsKeyAltH"/>
            //<add key = "Windows key + Alt + K" action="Mute or unmute the microphone in supported apps" function ="windowsKeyAltK"/>
            //<add key = "Windows key + Alt + Up arrow" action="Snap the active window to the top half of the screen" function ="windowsKeyAltUp"/>
            //<add key = "Windows key + comma (,)" action="Temporarily peek at the desktop" function ="windowsKeyComma"/>
            //<add key = "Windows key + Ctrl + C" action="If turned on in settings, enable or disable color filters" function ="windowsKeyCtrlC"/> 
        public static void windowsKeyAltH()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
        public static void windowsKeyAltK()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.K);
            Keys.Release(Keys.K);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
        public static void windowsKeyAltUp()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
        public static void windowsKeyComma()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.COMMA);
            Keys.Release(Keys.COMMA);
            Keys.Release(Keys.WIN);
        }
        public static void windowsKeyCtrlC()
        {
            Keys.Press(Keys.WIN);
>>>>>>> 5eacf4c4a4359b84b376bec35e03166f82cb76dd
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
<<<<<<< HEAD
        }
        public void windowsKeyCtrlEnter()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ENTER);
            Keys.Release(Keys.ENTER);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }
        public void windowsKeyCtrlF()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F);
            Keys.Release(Keys.F);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }
        public void windowsKeyCtrlQ()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Q);
            Keys.Release(Keys.Q);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }
        public void windowsKeyCtrlShiftB()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Release(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }
        public void windowsKeyCtrlSpacebar()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Spacebar);
            Keys.Release(Keys.Spacebar);
            Keys.Release(Keys.CTRL);
=======
>>>>>>> 5eacf4c4a4359b84b376bec35e03166f82cb76dd
            Keys.Release(Keys.WIN);
        }
    }
}
