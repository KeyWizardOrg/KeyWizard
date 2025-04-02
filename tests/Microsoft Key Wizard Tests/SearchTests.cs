using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key_Wizard.backend.search;
using Key_Wizard.backend.shortcuts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft_Key_Wizard_Tests
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void TestFuzzySearchOnNoMatch()
        {
            var items = new List<Shortcut>
            {
                new Key_Wizard.shortcuts.Shortcut("Test", ["A", "B", "C"])
            };
            var results = Search.Do(items, "ThisWillNotMatch");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestFuzzySearchOnOneMatch()
        {
            var items = new List<Shortcut>
            {
                new Key_Wizard.shortcuts.Shortcut("Test", ["A", "B", "C"])
            };
            var results = Search.Do(items, "Test");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Test", results[0].Description);
        }

        [TestMethod]
        public void TestFuzzySearchOnMultipleMatches()
        {
            var items = new List<Shortcut>
            {
                new Key_Wizard.shortcuts.Shortcut("Test", ["A", "B", "C"]),
                new Key_Wizard.shortcuts.Shortcut("Testing 123", ["D", "E"]),
            };
            var results = Search.Do(items, "Test");
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("Test", results[0].Description);
            Assert.AreEqual("Testing 123", results[1].Description);
        }

        [TestMethod]
        public void TestFuzzySearchWithTypo()
        {
            var items = new List<Shortcut>
            {
                new Key_Wizard.shortcuts.Shortcut("Test", ["A", "B", "C"]),
            };
            var results = Search.Do(items, "Tset");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Test", results[0].Description);
        }
    }
}
