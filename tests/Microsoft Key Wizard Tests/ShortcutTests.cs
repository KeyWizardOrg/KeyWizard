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
    public class TestShortcuts
    {
        [TestMethod]
        public void TestWindowsKeyI()
        {
            var processId = Key_Wizard.shortcuts.Shortcuts.windowsKeyI();
            Thread.Sleep(2000);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }
    }
}
