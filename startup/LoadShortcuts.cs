using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using static System.Collections.Specialized.BitVector32;

namespace Key_Wizard.startup
{
    internal class LoadShortcuts
    {
        public static List<Category> Read()
        {
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shortcuts\\base");
            string[] baseJson = Directory.GetFiles(baseDir, "*.json");

            string customDir = System.IO.Directory.CreateDirectory(String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Key Wizard")).FullName;
            string[] customJson = Directory.GetFiles(customDir, "*.json");
            
            string[] files = [.. baseJson, .. customJson];
            
            var shortcuts = new List<Category>();
            foreach (var file in files)
            {
                var fileContents = File.ReadAllText(file);
                Category? category = JsonSerializer.Deserialize<Category>(fileContents);
                foreach (var item in category.Shortcuts)
                {
                    item.Category = category.Name;
                }
                shortcuts.Add(category);
            }

            return shortcuts;
        }
    }
}
