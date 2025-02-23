using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft_Key_Wizard_Tests
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void TestFuzzySearchOnNoMatch()
        {
            var items = new List<Key_Wizard.ListItem>
            {
                new Key_Wizard.ListItem { Prefix = "Test", Suffix = "Item", Action = "TestAction" }
            };
            var results = Key_Wizard.search.Search.FuzzySearch(items, "NoMatch");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestFuzzySearchOnOneMatch()
        {
            var items = new List<Key_Wizard.ListItem>
            {
                new Key_Wizard.ListItem { Prefix = "Test", Suffix = "Item", Action = "TestAction" }
            };
            var results = Key_Wizard.search.Search.FuzzySearch(items, "Test");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Test Item", results[0].Prefix + " " + results[0].Suffix);
        }

        [TestMethod]
        public void TestFuzzySearchOnMultipleMatches()
        {
            var items = new List<Key_Wizard.ListItem>
            {
                new Key_Wizard.ListItem { Prefix = "Test", Suffix = "Item", Action = "TestAction" },
                new Key_Wizard.ListItem { Prefix = "Another", Suffix = "Item", Action = "AnotherAction" }
            };
            var results = Key_Wizard.search.Search.FuzzySearch(items, "Item");
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("Test Item", results[0].Prefix + " " + results[0].Suffix);
            Assert.AreEqual("Another Item", results[1].Prefix + " " + results[1].Suffix);
        }

        [TestMethod]
        public void TestFuzzySearchWithTypo()
        {
            var items = new List<Key_Wizard.ListItem>
            {
                new Key_Wizard.ListItem { Prefix = "Testing", Suffix = "Item", Action = "TestAction" }
            };
            var results = Key_Wizard.search.Search.FuzzySearch(items, "Tstng");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Testing Item", results[0].Prefix + " " + results[0].Suffix);
        }
    }
}
