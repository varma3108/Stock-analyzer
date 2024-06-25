using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify bullish patterns in candlestick data.
    internal class Recognizer_Bullish : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Bullish" and a pattern length of 1.
        // This setup targets the recognition of bullish patterns that involve a single candlestick.
        public Recognizer_Bullish() : base("Bullish", 1)
        {
        }

        // Override the abstract Recognize method to determine if a candlestick at a given index in a list is bullish.
        // A bullish candlestick is defined as having its closing price higher than its opening price.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the bullish pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is bullish by comparing the open and close prices.
                bool bullish = scs.close > scs.open;

                // Add the result of the bullish pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, bullish);

                // Return the result of the bullish pattern check.
                return bullish;
            }
        }
    }
}

