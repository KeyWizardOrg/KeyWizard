using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Key_Wizard;
using System.Diagnostics;
using System.Threading;

namespace Microsoft_Key_Wizard_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [UITestMethod]
        public void TestActions()
        {
            var window = new Key_Wizard.MainWindow();
            var expectedActions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                {"Open the Run dialog box", window.runDialog},
                {"Switch between open windows", window.altTab },
                {"Capture a full screen screenshot", window.screenshot },
                {"Open the Settings App", window.settings }
            };

            CollectionAssert.AreEquivalent(expectedActions, window.actions);
        }

        [UITestMethod]
        public void TestSettings()
        {
            var window = new Key_Wizard.MainWindow();
            var explorerCount = Process.GetProcessesByName("explorer").Length;
            window.settings();
            Assert.AreEqual(explorerCount + 1, Process.GetProcessesByName("explorer").Length);
        }
    }
}
