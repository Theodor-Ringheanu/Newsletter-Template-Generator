using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TemplateGenerator.Utilities
{
    public class InputBoxes : MainWindow
    {
        // Set default ranges based on the stored files
        public static void SetDefaultRanges(List<int> defaultIDs, TextBox input0, TextBox input1)
        {
            if (defaultIDs.Count > 1)
            {
                int minRangeInput = Math.Min(defaultIDs[0], defaultIDs[^1]);
                int maxRangeInput = Math.Max(defaultIDs[0], defaultIDs[^1]);
                input0.Text = minRangeInput.ToString();
                input1.Text = maxRangeInput.ToString();
            }
        }

        // Check if one or more of the ranges can generate the desire number of resulting IDs 
        public static bool IsValidRange(TextBox input0, TextBox input1, int resultsCount)
        {
            int minRangeInput = Math.Min(int.Parse(input0.Text), int.Parse(input1.Text));
            int maxRangeInput = Math.Max(int.Parse(input0.Text), int.Parse(input1.Text));
            if (maxRangeInput - minRangeInput < resultsCount - 1)
            {
                MessageBox.Show($"At least one of the ranges requires a greater difference to generate the proper number of results." +
                    $"\n\nFor example: 3 - 4 cannot result in 3 unique numbers. Meanwhile, a range of 3 - 5 or a range of 2 - 4 can.");
                return false;
            }
            return true;
        }
    }
}
