using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using static System.Collections.Specialized.BitVector32;

namespace Key_Wizard.startup
{
    internal class CreateDictionary
    {
        public static Dictionary<string, CreateSections> InitList()
        {
            string xmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");

            if (!System.IO.File.Exists(xmlFilePath))
            {
                Console.WriteLine($"Error: {xmlFilePath} not found");
                return null;
            }

            XDocument doc = XDocument.Load(xmlFilePath);

            // contains each section name and its associated dictionary of keyAction pairs
            var sections = new Dictionary<string, CreateSections>();

            foreach (var section in doc.Descendants("appSettings").Elements())
            {
                var keyActions = new Dictionary<string, (string action, string function)>();
                //String function = string.Empty;

                var sectionName = section.Name.LocalName;

                foreach (var Element in section.Elements("add"))
                {
                    var key = (string)Element.Attribute("key");
                    var action = (string)Element.Attribute("action");
                    var function = (string)Element.Attribute("function");

                    if (key != null && action != null)
                    {
                        keyActions[key] = (action, function);
                    }
                }

                sections[sectionName] = new CreateSections()
                {
                    Data = keyActions
                };
            }
            // Test the output
            //foreach (var section in sections)
            //{
            //    Console.WriteLine($"Section: {section.Key}");
            //    foreach (var kvp in section.Value.Data)
            //    {
            //        Console.WriteLine($"  Key: {kvp.Key}");
            //        Console.WriteLine($"    Action: {kvp.Value.action}");
            //        Console.WriteLine($"    Function: {kvp.Value.function}()");
            //    }
            //    Console.WriteLine();
            //}
            return sections;
        }

        public static List<ListItem> InitSearch (Dictionary<string, CreateSections> shortcutDictionary)
        {
            List<ListItem> searchList = new List<ListItem>();
            // Populate the ListView with the key-action pairs
            foreach (var section in shortcutDictionary)
            {
                String sectionName = section.Key.Replace('_', ' ');

                foreach (var keyAction in section.Value.Data)
                {
                    ListItem newItem = new ListItem { Prefix = $"{keyAction.Value.action}: ", Suffix = $"{keyAction.Key}", Action = $"{keyAction.Value.function}" };
                    searchList.Add(newItem);
                }
            }
            return searchList;
        }
    }

    internal class CreateSections
    {
        public Dictionary<string, (string action, string function)> Data { get; set; } = new();
    }
}
