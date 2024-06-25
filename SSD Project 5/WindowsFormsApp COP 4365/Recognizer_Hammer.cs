using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Hammer candlestick patterns in financial data.
    internal class Recognizer_Hammer : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Hammer" and a pattern length of 1.
        // This sets up this recognizer for detecting Hammer patterns that involve a single candlestick.
        public Recognizer_Hammer() : base("Hammer", 1)
        {
        }

        // Override the abstract Recognize method to determine if a Hammer pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Hammer pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is a Hammer by evaluating the proportions of its body and tail.
                // A Hammer is typically identified by a body that is small (20% to 33% of the total range) and a long lower tail (at least 66% of the range).
                bool hammer = ((scs.range * 0.20m) < scs.bodyRange) && (scs.bodyRange < (scs.range * 0.33m)) && (scs.lowerTail > scs.range * 0.66m);

                // Add the result of the Hammer pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, hammer);

                // Return the result of the Hammer pattern check.
                return hammer;
            }
        }
    }
}

