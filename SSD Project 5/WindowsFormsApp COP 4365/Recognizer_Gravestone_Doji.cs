using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Gravestone Doji patterns in candlestick data.
    internal class Recognizer_Gravestone_Doji : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Gravestone Doji" and a pattern length of 1.
        // This setup targets the recognition of Gravestone Doji patterns that involve a single candlestick.
        public Recognizer_Gravestone_Doji() : base("Gravestone Doji", 1)
        {
        }

        // Override the abstract Recognize method to determine if a Gravestone Doji pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Gravestone Doji pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is a Gravestone Doji by evaluating the size of the upper tail and the body.
                // A Gravestone Doji is identified if the upper tail constitutes more than 66% of the total range and the body is very small (less than 3% of the range).
                bool gravestone = scs.upperTail > (scs.range * 0.66m);
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                bool gravestone_doji = gravestone & doji;

                // Add the result of the Gravestone Doji pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, gravestone_doji);

                // Return the result of the Gravestone Doji pattern check.
                return gravestone_doji;
            }
        }
    }
}

