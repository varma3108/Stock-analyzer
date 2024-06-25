using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Doji candlestick patterns in financial data.
    internal class Recognizer_Doji : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Doji" and a pattern length of 1.
        // This sets up this recognizer for detecting single candlestick Doji patterns.
        public Recognizer_Doji() : base("Doji", 1)
        {
        }

        // Override the abstract Recognize method to determine if a Doji pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Doji pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is a Doji by comparing the body range to the total range.
                // A Doji is identified if the body (difference between open and close) is less than 3% of the total range (high to low).
                bool doji = scs.bodyRange < (scs.range * 0.03m);

                // Add the result of the Doji pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, doji);

                // Return the result of the Doji pattern check.
                return doji;
            }
        }
    }
}

