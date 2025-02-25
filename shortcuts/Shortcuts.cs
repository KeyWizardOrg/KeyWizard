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

        public void ctrlC()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
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
            Keys.Release(Keys.WIN);
        }
    }
}
