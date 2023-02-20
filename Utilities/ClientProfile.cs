using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace TemplateGenerator.Utilities
{
    public class ClientProfile
    {
        // Implement the ranges and results from XAML for each client
        public static void GenerateClientProfile(TextBox input0, TextBox input1, params TextBox[] results)
        {
            HashSet<int> hashTable = new();
            GenerateAndDisplayRandomNumbers(input0, input1, results, hashTable);
        }

        // Order each range and generates the random IDs
        public static void GenerateAndDisplayRandomNumbers(TextBox input0, TextBox input1, TextBox[] results, HashSet<int> hashTable)
        {

            int minRangeInput = Math.Min(int.Parse(input0.Text), int.Parse(input1.Text));
            int maxRangeInput = Math.Max(int.Parse(input0.Text), int.Parse(input1.Text));

            Random rng = new();

            while (hashTable.Count < results.Length)
            {
                Random _rng = new();
                hashTable.Add(_rng.Next(minRangeInput, maxRangeInput + 1));
            }

            List<int> uniqueRandomNumbers = hashTable.ToList();
            uniqueRandomNumbers = uniqueRandomNumbers.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < hashTable.Count; i++)
                results[i].Text = uniqueRandomNumbers[i].ToString();
        }
    }
}
