﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Key_Wizard.backend.search
{
    internal class SearchLib
    {
        /*
         * Remove extraneous whitespace, make lowercase
         */
        internal static string NormalizeQuery(string query)
        {
            return System.Text.RegularExpressions.Regex.Replace(query, @"[^\w\s]+|\s+", " ").ToLower();
        }

        /*
         * Compare each word in the query to a predefined list of synonyms.
         * If a word 'a' matches a query, then the query is replaced with a list of versions
         * of the query where 'a' is substituted for each of its synonyms.
         * The idea here is that if I type "launch foobar", then "start foobar" should also show up.
         */
        internal static List<string> ExtraQueries(string query)
        {
            var queryWords = query.Split(" ");
            var extraQueries = new List<string> { query };

            foreach (var word in queryWords)
            {
                foreach (var synonymList in synonyms)
                {
                    if (synonymList.Contains(word))
                    {
                        foreach (var synonym in synonymList)
                        {
                            if (synonym != word)
                            {
                                var newQuery = string.Join(" ", queryWords.Select(q => q == word ? synonym : q));
                                extraQueries.Add(newQuery);
                            }
                        }
                    }
                }
            }

            return extraQueries;
        }

        internal static List<List<string>> synonyms = new List<List<string>>
            {
                new() { "open", "launch", "start" },
                new() { "remove", "delete" },
                new() { "stop", "close", "exit", "quit" },
                new() { "copy", "duplicate" },
                new() { "paste", "insert" },
                new() { "cut", "trim" },
                new() { "undo", "revert" },
                new() { "redo", "repeat" },
                new() { "save", "store" },
                new() { "find", "search" },
                new() { "select", "highlight" },
                new() { "new", "create" },
                new() { "print", "output" },
                new() { "help", "support" },
                new() { "preferences", "settings", "options" },
                new() { "minimize", "hide" },
                new() { "lock", "secure" },
                new() { "refresh", "reload" },
                new() { "zoom", "enlarge", "magnify" }
            };
    }
}
