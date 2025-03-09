using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Wizard.search
{
    internal class SearchLib
    {
        internal static string NormalizeQuery(string query)
        {
            return System.Text.RegularExpressions.Regex.Replace(query, @"[^\w\s]+|\s+", " ").ToLower();
        }
    }
}
