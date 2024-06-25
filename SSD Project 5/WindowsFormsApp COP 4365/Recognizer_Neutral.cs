using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify neutral patterns in candlestick data.
    internal class Recognizer_Neutral : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Neutral" and a pattern length of 1.
        // This setup targets the recognition of neutral patterns that involve a single candlestick.
        public Recognizer_Neutral() : base("Neutral", 1)
        {
        }

        // Override the abstract Recognize method to determine if a neutral pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the neutral pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is neutral by comparing the body range to the total range.
                // A neutral candlestick is identified if the body (difference between open and close) is less than 3% of the total range (high to low).
                bool neutral = scs.bodyRange < (scs.range * 0.03m);

                // Add the result of the neutral pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, neutral);

                // Return the result of the neutral pattern check.
                return neutral;
            }
        }
    }
}

