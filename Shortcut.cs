using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

namespace Key_Wizard
{
    public class Shortcut
    {
        public string Category { get; }
        public string Description { get; }
        public List<string> Keys { get; }

        public string KeysConcatenation { get; }

        public List<Run>? SearchResults { get; set; }

        public Shortcut(string Category, string Description, List<string> Keys)
        {
            this.Category = Category;
            this.Description = Description;
            this.Keys = Keys;
            this.KeysConcatenation = ConcatenateList(this.Keys);
        }

        public static string ConcatenateList(List<string> keys)
        {
            StringBuilder c = new StringBuilder();
            foreach (var key in keys)
            {
                c.Append(key);
                c.Append(" + ");
            }
            c.Remove(c.Length - 2, 2);
            return c.ToString();
        } 
    }
}
