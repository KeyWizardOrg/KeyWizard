using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft_Key_Wizard_Tests
{
    [TestClass]
    public class ShortcutTests
    {
        [TestMethod]
        public void TestWindowsKeyI()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyI();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyR()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyR();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }
    }
}
