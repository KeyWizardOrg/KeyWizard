using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

namespace Key_Wizard
{
    public class Shortcut
    {
        public string Description { get; }
        public List<string> Keys { get; }
        public List<string> DisplayKeys { get; }
        public List<Run>? SearchResults { get; set; }

        public Shortcut(string Description, List<string> Keys)
        {
            this.Description = Description;
            this.Keys = Keys;
            this.DisplayKeys = ConvertKeys(this.Keys);
        }

        public static List<string> ConvertKeys(List<string> keys)
        {
            List<String> displayKeys = new List<string>();
            foreach (var key in keys)
            {
                switch (key.ToUpper())
                {
                    // Windows and System Keys
                    case "WINDOWS":
                        displayKeys.Add("⊞ WIN");
                        break;
                    case "SHIFT":
                        displayKeys.Add("⇧ SHIFT");
                        break;
                    // Navigation and Arrow Keys
                    case "TAB":
                        displayKeys.Add("↹ TAB");
                        break;
                    case "LEFT":
                        displayKeys.Add("←");
                        break;
                    case "RIGHT":
                        displayKeys.Add("→");
                        break;
                    case "UP":
                        displayKeys.Add("↑");
                        break;
                    case "DOWN":
                        displayKeys.Add("↓");
                        break;
                    case "HOME":
                        displayKeys.Add("⇤ HOME");
                        break;
                    case "END":
                        displayKeys.Add("⇥ END");
                        break;
                    case "PAGE UP":
                        displayKeys.Add("↑ PG UP");
                        break;
                    case "PAGE DOWN":
                        displayKeys.Add("↓ PG DN");
                        break;

                    // Editing Keys
                    case "ENTER":
                        displayKeys.Add("⏎ ENTER");
                        break;
                    case "BACKSPACE":
                        displayKeys.Add("BACKSPACE ⌫");
                        break;
                    case "DELETE":
                        displayKeys.Add("⌦ DEL");
                        break;
                    case "SEMICOLON":
                        displayKeys.Add(";");
                        break;
                    case "COLON":
                        displayKeys.Add(":");
                        break;
                    case "COMMA":
                        displayKeys.Add(",");
                        break;
                    case "PLUS":
                        displayKeys.Add("+");
                        break;
                    // Default case - use the key as-is with some formatting
                    default:
                        // Capitalize the key for consistency
                        displayKeys.Add(key.ToUpper());
                        break;
                }
            }

            // Ensure the list always has at least 3 elements
            while (displayKeys.Count < 3)
            {
                displayKeys.Add("");
            }

            return displayKeys;
        }
    }
    public class Category
    {
        public string Name { get; set; }
        public ObservableCollection<Shortcut> Shortcuts { get; set; }
    }
}
