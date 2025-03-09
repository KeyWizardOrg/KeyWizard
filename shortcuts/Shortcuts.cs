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
        public void Backspace()
        {
            Keys.Press(Keys.BACKSPACE);
        }

        public void ctrlA()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.A);
            Keys.Release(Keys.A);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlB()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlBackspace()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.BACKSPACE);
            Keys.Release(Keys.BACKSPACE);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlC()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlInsert()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlDel()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DELETE);
            Keys.Release(Keys.DELETE);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlDown()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEnd()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F);
            Keys.Release(Keys.F);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlH()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlHome()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlI()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.I);
            Keys.Release(Keys.I);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlLeft()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlRight()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftV()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlU()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.U);
            Keys.Release(Keys.U);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlUp()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlV()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.CTRL);
        }

        public void shiftIns()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.SHIFT);
        }

        public void ctrlX()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.X);
            Keys.Release(Keys.X);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlY()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Y);
            Keys.Release(Keys.Y);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlZ()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.Z);
            Keys.Release(Keys.Z);
            Keys.Release(Keys.CTRL);
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

        public void shiftCtrlDown()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlEnd()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlHome()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlLeft()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlRight()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
        }

        public void shiftCtrlUp()
        {
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.SHIFT);
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
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
            Keys.Release(Keys.ALT);
        }

        public void altF8()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.F8);
            Keys.Release(Keys.F8);
            Keys.Release(Keys.ALT);
        }

        public void altLeft()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.ALT);
        }

        public void altPageDown()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PAGEDOWN);
            Keys.Release(Keys.PAGEDOWN);
            Keys.Release(Keys.ALT);
        }

        public void altPageUp()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PAGEUP);
            Keys.Release(Keys.PAGEUP);
            Keys.Release(Keys.ALT);
        }

        public void altPrtScn()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.ALT);
        }

        public void altRight()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.ALT);
        }

        public void altShiftLeft()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altShiftRight()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altSpace()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.ALT);
        }

        public void altTab()
        {
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
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.CTRL);
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

        public void ctrlEsc()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF4()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F4);
            Keys.Release(Keys.F4);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlF5()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.F5);
            Keys.Release(Keys.F5);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlR()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.R);
            Keys.Release(Keys.R);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShift()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }
        public void ctrlShiftLeft()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlShiftRight()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }


        public void ctrlShiftEsc()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.ESC);
            Keys.Release(Keys.ESC);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlSpace()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.CTRL);
        }

        //public void ctrlY()
        //{
        //    Keys.Press(Keys.CTRL);
        //    Keys.Press(Keys.Y);
        //    Keys.Release(Keys.Y);
        //    Keys.Release(Keys.CTRL);
        //}

        //public void ctrlZ()
        //{
        //    Keys.Press(Keys.CTRL);
        //    Keys.Press(Keys.Z);
        //    Keys.Release(Keys.Z);
        //    Keys.Release(Keys.CTRL);
        //}

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
            Keys.Press(Keys.WIN);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyA()
        {
            Process.Start("ms-actioncenter:");
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
<<<<<<< HEAD
=======

        public void ctrlC()
        {
            FocusWindowBehind();
=======
>>>>>>> 365afb1e286e6b2d1fdc65d635f265f5b414165d
        //Jan:<add key="Windows key + Alt + H" action="When voice typing is open, set the focus to the keyboard" function ="windowsKeyAltH"/>
            //<add key = "Windows key + Alt + K" action="Mute or unmute the microphone in supported apps" function ="windowsKeyAltK"/>
            //<add key = "Windows key + Alt + Up arrow" action="Snap the active window to the top half of the screen" function ="windowsKeyAltUp"/>
            //<add key = "Windows key + comma (,)" action="Temporarily peek at the desktop" function ="windowsKeyComma"/>
            //<add key = "Windows key + Ctrl + C" action="If turned on in settings, enable or disable color filters" function ="windowsKeyCtrlC"/> 
        public static void windowsKeyAltH()
=======

        public void windowsKeyAltH()
>>>>>>> main
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
<<<<<<< HEAD
        public static void windowsKeyAltK()
=======

        public void windowsKeyAltK()
>>>>>>> main
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.K);
            Keys.Release(Keys.K);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
<<<<<<< HEAD
        public static void windowsKeyAltUp()
=======

        public void windowsKeyAltUp()
>>>>>>> main
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
<<<<<<< HEAD
        public static void windowsKeyComma()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.COMMA);
            Keys.Release(Keys.COMMA);
            Keys.Release(Keys.WIN);
        }
        public static void windowsKeyCtrlC()
=======

        public void windowsKeyComma()
        {
            Keys.Press(Keys.WIN);
<<<<<<< HEAD
            Keys.Press(Keys.OEM_COMMA);
            Keys.Release(Keys.OEM_COMMA);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlC()
>>>>>>> main
        {
            Keys.Press(Keys.WIN);
=======
>>>>>>> 5eacf4c4a4359b84b376bec35e03166f82cb76dd
>>>>>>> 365afb1e286e6b2d1fdc65d635f265f5b414165d
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.C);
            Keys.Release(Keys.C);
            Keys.Release(Keys.CTRL);
<<<<<<< HEAD
            Keys.Release(Keys.WIN);
=======
<<<<<<< HEAD
>>>>>>> 365afb1e286e6b2d1fdc65d635f265f5b414165d
        }
<<<<<<< HEAD
=======

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
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.SPACEBAR);
            Keys.Release(Keys.SPACEBAR);
            Keys.Release(Keys.CTRL);
<<<<<<< HEAD
=======
=======
>>>>>>> 5eacf4c4a4359b84b376bec35e03166f82cb76dd
>>>>>>> 365afb1e286e6b2d1fdc65d635f265f5b414165d
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlV()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyD()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyDownArrow()
        {
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
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.H);
            Keys.Release(Keys.H);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyHome()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyI()
        {
            Process.Start("explorer.exe", "ms-settings:");
        }

        public void windowsKeyJ()
        {
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
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.L);
            Keys.Release(Keys.L);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyLeft()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyM()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.M);
            Keys.Release(Keys.M);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyMinus()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_MINUS);
            Keys.Release(Keys.OEM_MINUS);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyN()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.N);
            Keys.Release(Keys.N);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyO()
        {
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
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PAUSE);
            Keys.Release(Keys.PAUSE);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyPeriod()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.OEM_PERIOD);
            Keys.Release(Keys.OEM_PERIOD);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyPrtScn()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyQ()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.Q);
            Keys.Release(Keys.Q);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyR()
        {
            Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
        }

        public void windowsKeyRight()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyS()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.S);
            Keys.Release(Keys.S);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyTab()
        {
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
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyV()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.V);
            Keys.Release(Keys.V);
            Keys.Release(Keys.WIN);
        }

        public void ctrlIns()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.INSERT);
            Keys.Release(Keys.INSERT);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlM()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.M);
            Keys.Release(Keys.M);
            Keys.Release(Keys.CTRL);
        }

        public void altSelectionKey()
        {
            Keys.Press(Keys.ALT);
            // The actual selection key should be pressed manually after this function is called.
            Keys.Release(Keys.ALT);
        }
        public void arrowLeft()
        {
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
        }

        public void arrowRight()
        {
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
        }

        public void arrowUp()
        {
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
        }

        public void arrowDown()
        {
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
        }

        public void ctrlHomeMarkMode()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEndMarkMode()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.END);
            Keys.Release(Keys.END);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlHomeHistoryNav()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.HOME);
            Keys.Release(Keys.HOME);
            Keys.Release(Keys.CTRL);
        }

        public void ctrlEndHistoryNav()
        {
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
            // This function would need additional logic to press a specific number key (e.g., 1, 2, 3).
            // Example: Pressing "CTRL + 1"
            // Keys.Press(Keys.CTRL);
            // Keys.Press(Keys.D1);  // Replace with D2, D3, etc., for different numbers.
            // Keys.Release(Keys.D1);
            // Keys.Release(Keys.CTRL);

            // Do we want?
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

        public void ArrowUp()
        {
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
        }

        public void ArrowDown()
        {
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
        }

        public void ArrowLeft()
        {
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
        }

        public void ArrowRight()
        {
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
        }

        // File Explorer
        public void altD()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.ALT);
        }

        public void altEnter()
        {
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
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.P);
            Keys.Release(Keys.P);
            Keys.Release(Keys.ALT);
        }

        public void altShiftP()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.SHIFT);
            Keys.Press(Keys.P);
            Keys.Release(Keys.P);
            Keys.Release(Keys.SHIFT);
            Keys.Release(Keys.ALT);
        }

        public void altUp()
        {
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
            // Handle pressing a number key separately  // Need 1-3
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

        public void windowsKeyCtrlD()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlRight()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
            Keys.Release(Keys.CTRL);
            Keys.Release(Keys.WIN);
        }

        public void windowsKeyCtrlLeft()
        {
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



        // NEED TO ADD

        // Settings 
        public void arrowKeys()
        {
            Keys.Press(Keys.UP);
            Keys.Release(Keys.UP);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Press(Keys.LEFT);
            Keys.Release(Keys.LEFT);
            Keys.Press(Keys.RIGHT);
            Keys.Release(Keys.RIGHT);
        }

        public void enter()
        {
            Keys.Press(Keys.ENTER);
            Keys.Release(Keys.ENTER);
        }

>>>>>>> main
    }
}
