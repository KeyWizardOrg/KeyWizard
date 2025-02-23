using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key_Wizard.shortcuts
{
    public class Shortcuts
    {
        public static int windowsKeyR()
        {
            var process = Process.Start("explorer.exe", "shell:::{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}");
            return process.Id;
        }
        public static int windowsKeyI()
        {
            var process = Process.Start("explorer.exe", "ms-settings:");
            return process.Id;
        }

        public static void altTab()
        {
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
        }

        public static void PrtSc()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.PRTSCR);
            Keys.Release(Keys.PRTSCR);
            Keys.Release(Keys.WIN);
        }
       
        public static void ctrlAltTab()
        {
            Keys.Press(Keys.CTRL);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.TAB);
            Keys.Release(Keys.TAB);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.CTRL);
        }

        public static void windowsKey()
        {
            Keys.Press(Keys.WIN);
            Keys.Release(Keys.WIN);
        }

        public static void windowsKeyA()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.A);
            Keys.Release(Keys.A);
            Keys.Release(Keys.WIN);
        }

        public static void windowsKeyAltB()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.B);
            Keys.Release(Keys.B);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public static void windowsKeyAltD()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.D);
            Keys.Release(Keys.D);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }

        public static void windowsKeyAltDown()
        {
            Keys.Press(Keys.WIN);
            Keys.Press(Keys.ALT);
            Keys.Press(Keys.DOWN);
            Keys.Release(Keys.DOWN);
            Keys.Release(Keys.ALT);
            Keys.Release(Keys.WIN);
        }
    }
}
