using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using static System.Collections.Specialized.BitVector32;

namespace Key_Wizard.startup
{
    internal class LoadShortcuts
    {
        public static Dictionary<string, List<Shortcut>> Read()
        {
            string baseXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shortcuts\\base");
            string[] baseXmlFiles = Directory.GetFiles(baseXml, "*.xml");

            string customXml = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Key Wizard");
            string[] customXmlFiles = Directory.GetFiles(customXml, "*.xml");

            var dictionary = new Dictionary<string, List<Shortcut>>();
            
            foreach (var file in baseXmlFiles.Concat(customXmlFiles).ToArray())
            {
                XDocument doc = XDocument.Load(file);

                // contains each section name and its associated dictionary of keyAction pairs
                var shortcuts = new List<Shortcut>();

                var category = doc.Element("category").Attributes("title").First().Value;
                foreach (var shortcut in doc.Descendants("category").Elements())
                {
                    var description = (string)shortcut.Attribute("description");
                    List<string> keys = [];
                    foreach (var x in shortcut.Elements("key"))
                    {
                        keys.Add(x.Value);
                    }
                    shortcuts.Add(new Shortcut(category, description, keys));
                }

                dictionary.Add(category, shortcuts);
            }

            return dictionary;
        }
    }

    internal class CreateSections
    {
        public Dictionary<string, (string action, string function)> Data { get; set; } = new();
    }
}
