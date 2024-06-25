using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Dragonfly Doji patterns in candlestick data.
    internal class Recognizer_Dragonfly_Doji : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Dragonfly Doji" and a pattern length of 1.
        // This sets up this recognizer for detecting patterns that involve a single candlestick.
        public Recognizer_Dragonfly_Doji() : base("Dragonfly Doji", 1)
        {
        }

        // Override the abstract Recognize method to determine if a Dragonfly Doji pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Dragonfly Doji pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate if the lower tail is significantly longer than the body indicating a Dragonfly Doji.
                // A Dragonfly Doji occurs when the lower tail makes up at least 66% of the total range and the body is very small (less than 3% of the range).
                bool dragonfly = scs.lowerTail > (scs.range * 0.66m);
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                bool dragonfly_doji = dragonfly & doji;

                // Add the result of the Dragonfly Doji check to the candlestick's pattern dictionary.
                scs.Dictionary_Pattern.Add(Pattern_Name, dragonfly_doji);

                // Return the result of the Dragonfly Doji pattern check.
                return dragonfly_doji;
            }
        }
    }
}

