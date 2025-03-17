using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Wizard.search
{
    public class Search
    {
        public static List<ListItem> FuzzySearch(List<ListItem> items, string query, bool caseSensitive = false)
        {
            var normalizedQuery = NormalizeQuery(query, caseSensitive);
            var results = new List<(ListItem Item, int Score)>();

            foreach (var item in items)
            {
                var target = $"{item.Prefix} {item.Suffix}";
                var normalizedTarget = NormalizeQuery(target, caseSensitive);

                if (IsSubsequenceMatch(normalizedQuery, normalizedTarget, out int score))
                {
                    results.Add((item, score));
                }
            }

            // Sort by highest score first (best matches first)
            return results.OrderByDescending(r => r.Score)
                          .Select(r => r.Item)
                          .ToList();
        }

        private static bool IsSubsequenceMatch(string query, string target, out int score)
        {
            score = 0;
            if (query.Length == 0) return true;
            if (query.Length > target.Length) return false;

            int queryIndex = 0;
            int lastMatchIndex = -1;
            int totalDistance = 0;

            for (int i = 0; i < target.Length && queryIndex < query.Length; i++)
            {
                if (target[i] == query[queryIndex])
                {
                    // Bonus for consecutive matches
                    if (lastMatchIndex != -1)
                    {
                        int distance = i - lastMatchIndex;
                        totalDistance += distance;

                        // Add bonus for adjacent matches
                        if (distance == 1) score += 10;
                    }

                    lastMatchIndex = i;
                    queryIndex++;
                    score += 10; // Base score per matched character
                }
            }

            // Calculate final score
            if (queryIndex == query.Length)
            {
                // Penalize spread-out matches
                score -= totalDistance;

                // Bonus for matching full query
                if (queryIndex == target.Length) score += 50;

                return true;
            }

            return false;
        }

        private static string NormalizeQuery(string input, bool caseSensitive)
        {
            // Remove all non-alphanumeric characters and normalize
            var normalized = System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\s]", "");
            normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");

            return caseSensitive ? normalized : normalized.ToLower();
        }
    }
}
