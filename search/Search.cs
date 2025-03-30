using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Key_Wizard.shortcuts;
using Microsoft.UI.Xaml.Documents;

namespace Key_Wizard.search
{
    public class Search
    {
        public static List<Shortcut> Do(List<Shortcut> items, string query)
        {
            // remove special characters, remove non-essential whitespace, set to lowercase
            var normalizedQuery = SearchLib.NormalizeQuery(query);

            // create max-heap for storing results
            var results = new PriorityQueue<Shortcut, double>(new MaxHeapComparer());

            // only store 10 results at a time to save memory and processing
            int capacity = 10;
            results.EnsureCapacity(capacity);

            // save scores in separate max-heap, this is the most elegant solution bc C# data structures suck
            var scores = new PriorityQueue<double, double>(new MaxHeapComparer());

            var querySizeFactor = (double)normalizedQuery.Length / 10;

            var queries = SearchLib.ExtraQueries(normalizedQuery);
            foreach (var item in items)
            {
                var target = SearchLib.NormalizeQuery(item.Description);

                double distance = 1;
                foreach (var currentQuery in queries)
                {
                    // if query is part o
                    if (target.Contains(currentQuery))
                    {
                        distance = 0.1 + 0.0001 * target.Length;
                        break;
                    }

                    // carry out both algorithms on input query and prefix
                    var jaccard = JaccardSimilarity(currentQuery, target);
                    var damerau = DamerauLevenshteinDistance(currentQuery, target);

                    // calculate value combining jaccard and damerau using weight
                    var weight = 0.5;
                    var current = weight * jaccard + (1 - weight) * damerau;
                    current *= querySizeFactor;
                    if (current < distance)
                    {
                        distance = current;
                    }
                }

                // only add result if distance is reasonable
                if (distance < 0.8)
                {
                    // if less than 10 results, just add result to max-heap
                    if (results.Count < capacity)
                    {
                        results.Enqueue(item, distance);
                        scores.Enqueue(distance, distance);
                    }
                    // else, if distance is in 10 lowest, remove largest item from max-heap and add this item
                    else if (distance < scores.Peek())
                    {
                        results.EnqueueDequeue(item, distance);
                        scores.EnqueueDequeue(distance, distance);
                    }
                    // otherwise the distance is too large and do nothing
                }
            }

            // convert results into a list of the largest 10, then return
            var resultsList = new List<Shortcut>();
            while (results.Count > 0)
            {
                resultsList.Add(results.Dequeue());
            }
            if (resultsList.Count > 0) resultsList.Reverse();
            return resultsList;
        }


        /**
         * Determine the number of changes necessary to travel from a to b.
         * https://en.wikipedia.org/wiki/Damerau-Levenshtein_distance
         */
        private static double DamerauLevenshteinDistance(string a, string b)
        {
            var d = new double[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j <= b.Length; j++)
            {
                d[0, j] = j;
            }

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,                                // deletion
                            d[i, j - 1] + 0.9                               // insertion (slightly deweighted to make search better)
                        ),
                        d[i - 1, j - 1] + (a[i - 1] == b[j - 1] ? 0 : 1)    // substitution
                    );

                    if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                    {
                        d[i, j] = Math.Min(
                            d[i, j],
                            d[i - 2, j - 2] + 1                             // transposition
                        );
                    }

                }
            }

            return d[a.Length, b.Length] / Math.Max(a.Length, b.Length);
        }

        /**
         * Determine the similarity between two sets by dividing their intersection by their union.
         * https://en.wikipedia.org/wiki/Jaccard_index
         */
        private static double JaccardSimilarity(string a, string b)
        {
            HashSet<char> set1 = new(a);
            HashSet<char> set2 = new(b);

            HashSet<char> intersection = new(set1);
            intersection.IntersectWith(set2);

            HashSet<char> union = new(set1);
            union.UnionWith(set2);

            double jaccardIndex = (double)intersection.Count / (1.2 * union.Count);
            return 1 - jaccardIndex;
        }


    }
}

internal class MaxHeapComparer : IComparer<double>
{
    public int Compare(double x, double y)
    {
        return y.CompareTo(x);
    }
}
