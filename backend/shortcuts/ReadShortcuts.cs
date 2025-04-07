using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Media.Core;
using static System.Collections.Specialized.BitVector32;

namespace Key_Wizard.backend.shortcuts
{
    internal class ReadShortcuts
    {
        public static List<Category> ReadBase()
        {
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backend\\shortcuts\\base");
            string[] baseJson = Directory.GetFiles(baseDir, "*.json");
            return Read(baseJson);
        }

        public static List<Category> ReadCustom()
        {
            string customDir = Directory.CreateDirectory(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Key Wizard")).FullName;
            string[] customJson = Directory.GetFiles(customDir, "*.json");
            return Read(customJson);
        }
        private static List<Category> Read(string[] files)
        {
            var shortcuts = new List<Category>();
            foreach (var file in files)
            {
                var fileContents = File.ReadAllText(file);
                Category? category = JsonSerializer.Deserialize<Category>(fileContents);
                if (category != null && category.Shortcuts != null)
                {
                    for (int i = 0; i < category.Shortcuts.Count; i++)
                    {
                        Shortcut? item = category.Shortcuts[i];
                        item.Category = category.Name;
                    }
                    shortcuts.Add(category);
                }
            }

            return shortcuts;
        }
    }
}
