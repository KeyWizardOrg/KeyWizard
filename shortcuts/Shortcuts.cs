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

        // Copilot Keys 
        // Could have it made so test if copilot is available first, and if its not, run windows search
        public void CopilotKey()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.WIN);
        }

        public void CopilotKeyDisabled()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.S);
            Keys.Release(Keys.S);
            Keys.Release(Keys.WIN);
        }

        // Text Editing 
        public void ctrlA()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.A);
            Keys.Release(Keys.A);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlB()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlBackspace()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.BACKSPACE);
            Keys.Release(Keys.BACKSPACE);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlC()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlInsert()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlDel()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DELETE);
            Keys.Release(Keys.DELETE);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEnd()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F);
            Keys.Release(Keys.F);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlH()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlHome()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlI()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.I);
            Keys.Release(Keys.I);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftV()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlU()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.U);
            Keys.Release(Keys.U);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlV()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.CTRL);
        }

        public void shiftIns()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.SHIFT);
        }

        public void ctrlX()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.X);
            Keys.Release(Keys.X);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlY()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Y);
            Keys.Release(Keys.Y);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlZ()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Z);
            Keys.Release(Keys.Z);
            Keys.Release(Keys.CTRL);
        }

        public void shiftCtrlDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlEnd()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlHome()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        // Functions without FocusWindowBehind()
        public void Backspace()
        {
            Keys.Press(Keys.BACKSPACE);
        }

        public void del()
        {
            Keys.Press(Keys.DELETE);
        }

        public void delete()
        {
            Keys.Press(Keys.DELETE);
        }

        public void down()
        {
            Keys.Press(Keys.DOWN);
        }

        public void end()
        {
            Keys.Press(Keys.END);
        }

        public void home()
        {
            Keys.Press(Keys.HOME);
        }

        public void left()
        {
            Keys.Press(Keys.LEFT);
        }

        public void pageDown()
        {
            Keys.Press(Keys.PAGEDOWN);
        }

        public void pgDn()
        {
            Keys.Press(Keys.PAGEDOWN);
        }

        public void pageUp()
        {
            Keys.Press(Keys.PAGEUP);
        }

        public void pgUp()
        {
            Keys.Press(Keys.PAGEUP);
        }

        public void rightArrow()
        {
            Keys.Press(Keys.RIGHT);
        }

        public void shiftDown()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftEnd()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftHome()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftLeft()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftPageDown()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.PAGEDOWN);
            Keys.Release(Keys.PAGEDOWN);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftPageUp()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.PAGEUP);
            Keys.Release(Keys.PAGEUP);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftRight()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftUp()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.SHIFT);
        }

        public void tab()
        {
            Keys.Press(Keys.TAB);
        }

        public void up()
        {
            Keys.Press(Keys.UP);
        }
        // Desktop
        public void altA()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.A);
            Keys.Release(Keys.A);
            Keys.Release(Keys.ALT);
        }

        public void altEsc()
        {
            FocusWindowBehind();
        }

        public void altF4()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
            Keys.Release(Keys.ALT);
        }

        public void altF8()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.F8);
            Keys.Release(Keys.F8);
            Keys.Release(Keys.ALT);
        }

        public void altLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.ALT);
        }

        public void altPageDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PAGEDOWN);
            Keys.Release(Keys.PAGEDOWN);
            Keys.Release(Keys.ALT);
        }

        public void altPageUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PAGEUP);
            Keys.Release(Keys.PAGEUP);
            Keys.Release(Keys.ALT);
        }

        public void altPrtScn()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.ALT);
        }

        public void altRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.ALT);
        }

        public void altShiftLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altShiftRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altSpace()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.ALT);
        }

        public void altTab()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
        }

        public void altUnderlinedLetter()
        {
            Keys.Press(Keys.ALT);
            Keys.Release(Keys.ALT); // Followed by the underlined letter manually
        }

        public void ctrlAltDel()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlAltTab()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEsc()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF4()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF5()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F5);
            Keys.Release(Keys.F5);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlR()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.R);
            Keys.Release(Keys.R);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShift()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftEsc()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlSpace()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.CTRL);
        }

        public void esc()
        {
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
        }

        public void Escape()
        {
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
        }

        public void F5()
        {
            Keys.Press(Keys.F5);
            Keys.Release(Keys.F5);
        }

        public void F6()
        {
            Keys.Press(Keys.F6);
            Keys.Release(Keys.F6);
        }

        public void F10()
        {
            Keys.Press(Keys.F10);
            Keys.Release(Keys.F10);
        }

        public void PrtSc()
        {
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
        }


        // Windows Keys 
        public void windowsKey()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyA()
        {
            Process.Start("ms-actioncenter:");
        }

        public void windowsKeyAltB()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltD()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltH()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltK()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.K);
            Keys.Release(Keys.K);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAltUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyComma()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_COMMA);
            Keys.Release(Keys.OEM_COMMA);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlC()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlEnter()
        {
            Process.Start("Narrator.exe");
        }

        public void windowsKeyCtrlF()
        {
            Process.Start("control.exe", "/name Microsoft.NetworkAndSharingCenter");
        }

        public void windowsKeyCtrlQ()
        {
            Process.Start("quickassist:");
        }

        public void windowsKeyCtrlShiftB()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlSpace()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlV()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyD()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyDownArrow()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyE()
        {
            Process.Start("explorer.exe");
        }

        public void windowsKeyEsc()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyF()
        {
            Process.Start("ms-feedback:");
        }

        public void windowsKeyForwardSlash()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_2);
            Keys.Release(Keys.OEM_2);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyG()
        {
            Process.Start("ms-gamebar:");
        }

        public void windowsKeyH()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyHome()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.WIN);
        }

        public int windowsKeyI()
        {
            var process = Process.Start("explorer.exe", "ms-settings:");
            return process.Id;
        }

        public void windowsKeyJ()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.J);
            Keys.Release(Keys.J);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyK()
        {
            Process.Start("ms-settings-connectabledevices:devicediscovery");
        }

        public void windowsKeyL()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.L);
            Keys.Release(Keys.L);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyM()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.M);
            Keys.Release(Keys.M);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyMinus()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_MINUS);
            Keys.Release(Keys.OEM_MINUS);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyN()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.N);
            Keys.Release(Keys.N);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyO()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.O);
            Keys.Release(Keys.O);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyP()
        {
            Process.Start("ms-settings-displays-topology:");
        }

        public void windowsKeyPause()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PAUSE);
            Keys.Release(Keys.PAUSE);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyPeriod()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_PERIOD);
            Keys.Release(Keys.OEM_PERIOD);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyPrtScn()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyQ()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.Q);
            Keys.Release(Keys.Q);
            Keys.Release(Keys.WIN);
        }

        public int windowsKeyR()
        {
            var process = Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
            return process.Id;
        }

        public void windowsKeyRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyS()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.S);
            Keys.Release(Keys.S);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyTab()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyU()
        {
            Process.Start("ms-settings:easeofaccess");
        }

        public void windowsKeyUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyV()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyX()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.X);
            Keys.Release(Keys.X);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyZ()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.Z);
            Keys.Release(Keys.Z);
            Keys.Release(Keys.WIN);
        }


        // Command Prompt
        public void ctrlIns()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlM()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.M);
            Keys.Release(Keys.M);
            Keys.Release(Keys.CTRL);
        }

        public void altSelectionKey()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            // The actual selection key should be pressed manually after this function is called.
            Keys.Release(Keys.ALT);
        }

        public void arrowLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
        }

        public void arrowRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
        }

        public void arrowUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
        }

        public void arrowDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
        }

        public void ctrlHomeMarkMode()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEndMarkMode()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlHomeHistoryNav()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEndHistoryNav()
        {
            FocusWindowBehind();
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
        }

        // Dialog Box
        public void F4()
        {
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
        }

        public void ctrlTab()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftTab()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrl123()
        {
            Keys.Press(Keys.CTRL);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.CTRL);
        }

        public void shiftTab()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.SHIFT);
        }

        public void space()
        {
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
        }

        public void backspace()
        {
            Keys.Press(Keys.BACKSPACE);
            Keys.Release(Keys.BACKSPACE);
        }

        // File Explorer
        public void altD()
        {
            FocusWindowBehind(); // Added where switching or focusing on windows might be needed
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.ALT);
        }

        public void altEnter()
        {
            FocusWindowBehind(); // Added where switching or focusing on windows might be needed
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.ENTER);
            Keys.Release(Keys.ENTER);
            Keys.Release(Keys.ALT);
        }

        public void altMouseDrag()
        {
            Keys.Press(Keys.ALT);
            // Perform mouse drag operation separately
            Keys.Release(Keys.ALT);
        }

        public void altP()
        {
            FocusWindowBehind(); // Added where switching or focusing on windows might be needed
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.P);
            Keys.Release(Keys.P);
            Keys.Release(Keys.ALT);
        }

        public void altShiftP()
        {
            FocusWindowBehind(); // Added where switching or focusing on windows might be needed
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.P);
            Keys.Release(Keys.P);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altUp()
        {
            FocusWindowBehind(); // Added where switching or focusing on windows might be needed
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.ALT);
        }

        public void ctrlD()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlE()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.E);
            Keys.Release(Keys.E);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlL()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.L);
            Keys.Release(Keys.L);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlMouseDrag()
        {
            Keys.Press(Keys.CTRL);
            // Perform mouse drag operation separately
            Keys.Release(Keys.CTRL);
        }

        public void ctrlMouseScroll()
        {
            Keys.Press(Keys.CTRL);
            // Handle mouse scroll action separately
            Keys.Release(Keys.CTRL);
        }

        public void ctrlN()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.N);
            Keys.Release(Keys.N);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlPlus()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ADD);
            Keys.Release(Keys.ADD);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftE()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.E);
            Keys.Release(Keys.E);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftN()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.N);
            Keys.Release(Keys.N);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShift1To9()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlT()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.T);
            Keys.Release(Keys.T);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlW()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.W);
            Keys.Release(Keys.W);
            Keys.Release(Keys.CTRL);
        }

        public void f2()
        {
            Keys.Press(Keys.F2);
            Keys.Release(Keys.F2);
        }

        public void f3()
        {
            Keys.Press(Keys.F3);
            Keys.Release(Keys.F3);
        }

        public void f4()
        {
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
        }

        public void f5()
        {
            Keys.Press(Keys.F5);
            Keys.Release(Keys.F5);
        }

        public void f6()
        {
            Keys.Press(Keys.F6);
            Keys.Release(Keys.F6);
        }

        public void f11()
        {
            Keys.Press(Keys.F11);
            Keys.Release(Keys.F11);
        }

        public void right()
        {
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
        }

        public void shiftDel()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.DELETE);
            Keys.Release(Keys.DELETE);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftF10()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.F10);
            Keys.Release(Keys.F10);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftMouseDrag()
        {
            Keys.Press(Keys.SHIFT);
            // Perform mouse drag operation separately
            Keys.Release(Keys.SHIFT);
        }

        public void shiftMouseRightClick()
        {
            Keys.Press(Keys.SHIFT);
            // Handle mouse right-click separately
            Keys.Release(Keys.SHIFT);
        }

        // Multiple Desktops
        public void windowsKeytab()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlD()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlRight()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlLeft()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }


        public void windowsKeyCtrlF4()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);

            // Bring focus back to the previous window after closing the desktop
            FocusWindowBehind();
        }

        // Taskbar
        public void altShiftUp()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altShiftDown()
        {
            FocusWindowBehind();
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void windowsKeyAltEnter()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.RETURN);
            Keys.Release(Keys.RETURN);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyAlt0_9()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyB()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrl0_9()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlShift0_9()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKey0_9()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyShift0_9()
        {
            FocusWindowBehind();
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.SHIFT);
            ShowNumberPad();
            pressDesiredKey(NumberPadWindow.SelectedNumber);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyT()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.T);
            Keys.Release(Keys.T);
            Keys.Release(Keys.WIN);
        }

        ////public void ctrlSelectGroupApp()
        ////{
        ////    Keys.Press(Keys.CTRL);
        ////    Mouse.Click(); // Assuming this is selecting a grouped app on the taskbar
        ////    Keys.Release(Keys.CTRL);
        ////}

        ////public void ctrlShiftSelectApp()
        ////{
        ////    Keys.Press(Keys.CTRL);
        ////    Keys.Press(Keys.SHIFT);
        ////    Mouse.Click(); // Click the app icon
        ////    Keys.Release(Keys.SHIFT);
        ////    Keys.Release(Keys.CTRL);
        ////}

        ////public void shiftRightClickApp()
        ////{
        ////    Keys.Press(Keys.SHIFT);
        ////    Mouse.RightClick(); // Right-click the app icon
        ////    Keys.Release(Keys.SHIFT);
        ////}

        ////public void rightClickGroupedTaskbar()
        ////{
        ////    Keys.Press(Keys.SHIFT);
        ////    Mouse.RightClick(); // Right-click on a grouped app
        ////    Keys.Release(Keys.SHIFT);
        ////}

        ////public void shiftSelectApp()
        ////{
        ////    Keys.Press(Keys.SHIFT);
        ////    Mouse.Click(); // Click app icon to open another instance
        ////    Keys.Release(Keys.SHIFT);
        ////}

        // Settings 

        public void enter()
        {
            Keys.Press(Keys.ENTER);
            Keys.Release(Keys.ENTER);
        }

        public void pressDesiredKey(int key)
        {
            if (key == 0)
            {
                Keys.Press(Keys.ZERO);
                Keys.Release(Keys.ZERO);

            }
            else if (key == 1)
            {
                Keys.Press(Keys.ONE);
                Keys.Release(Keys.ONE);
            }
            else if (key == 2)
            {
                Keys.Press(Keys.TWO);
                Keys.Release(Keys.TWO);
            }
            else if (key == 3)
            {
                Keys.Press(Keys.THREE);
                Keys.Release(Keys.THREE);
            }
            else if (key == 4)
            {
                Keys.Press(Keys.FOUR);
                Keys.Release(Keys.FOUR);
            }
            else if (key == 5)
            {
                Keys.Press(Keys.FIVE);
                Keys.Release(Keys.FIVE);
            }
            else if (key == 6)
            {
                Keys.Press(Keys.SIX);
                Keys.Release(Keys.SIX);
            }
            else if (key == 7)
            {
                Keys.Press(Keys.SEVEN);
                Keys.Release(Keys.SEVEN);
            }
            else if (key == 8)
            {
                Keys.Press(Keys.EIGHT);
                Keys.Release(Keys.EIGHT);
            }
            else
            {
                Keys.Press(Keys.NINE);
                Keys.Release(Keys.NINE);
            }
        }

        private void ShowNumberPad()
        {
            Console.WriteLine("ShowNumberPad called");

            try
            {
                var numberPadWindow = new NumberPadWindow();

                numberPadWindow.Activate();
                Console.WriteLine("Number Pad window shown.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error showing window: {ex.Message}");
            }
        }
    }
}
