using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Key_Wizard
{
    internal class CreateDictionary
    {
        public static Dictionary<string, Dictionary<string, string>> InitList()
        {
            string xmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");

            if (!System.IO.File.Exists(xmlFilePath))
            {
                Console.WriteLine($"Error: {xmlFilePath} not found");
                return null;
            }

            XDocument doc = XDocument.Load(xmlFilePath);
            Console.WriteLine("XML loaded");

            // contains each section name and its associated dictionary of keyAction pairs
            var sections = new Dictionary<string, Dictionary<string, string>>();

            foreach (var section in doc.Descendants("appSettings").Elements())
            {
                var sectionName = section.Name.LocalName;
                var keyActions = section.Elements("add").ToDictionary(
                        add => (string)add.Attribute("key"),
                        add => (string)add.Attribute("action")
                );

                sections[sectionName] = keyActions;
            }

            return sections;
        }
    }
}
