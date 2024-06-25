using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to implement specific logic for identifying bearish patterns in candlestick data.
    internal class Recognizer_Bearish : Recognizer
    {
        // Constructor that initializes the base Recognizer class with the pattern name "Bearish" and a pattern length of 1.
        // This sets up this recognizer for detecting patterns involving only one candlestick.
        public Recognizer_Bearish() : base("Bearish", 1)
        {
        }

        // Override the abstract Recognize method to determine if a candlestick at a given index in a list is bearish.
        // A bearish candlestick is defined as having its opening price greater than its closing price.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the bearish pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is bearish by comparing the open and close prices.
                bool bearish = scs.open > scs.close;

                // Add the result of the bearish pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, bearish);

                // Return the result of the bearish pattern check.
                return bearish;
            }
        }
    }
}


