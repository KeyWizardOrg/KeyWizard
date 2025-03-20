using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.ApplicationModel.DataTransfer;

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

        [TestMethod]
        public void TestWindowsKeyU()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyU();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyA()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyA();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyCtrlEnter()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyCtrlEnter();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyCtrlF()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyCtrlF();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyCtrlQ()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyCtrlQ();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyE()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyE();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyF()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyF();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyG()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyG();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyK()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyK();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void TestWindowsKeyP()
        {
            var shortcuts = new Key_Wizard.shortcuts.Shortcuts(null);
            var processId = shortcuts.windowsKeyP();
            Thread.Sleep(500);
            var process = System.Diagnostics.Process.GetProcessById(processId);
            Assert.IsNotNull(process);
        }



    }
}
