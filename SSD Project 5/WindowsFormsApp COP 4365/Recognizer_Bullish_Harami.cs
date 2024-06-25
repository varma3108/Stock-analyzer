using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify bullish harami patterns in candlestick data.
    internal class Recognizer_Bullish_Harami : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Bullish Harami" and a pattern length of 2.
        // This sets up this recognizer for detecting patterns that involve two consecutive candlesticks.
        public Recognizer_Bullish_Harami() : base("Bullish Harami", 2)
        {
        }

        // Override the abstract Recognize method to determine if a bullish harami pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the bullish harami pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            else
            {
                // Check if the current index allows for checking a pattern that includes the previous candlestick.
                // If not, return false and store this result to prevent out-of-bounds errors.
                int offset = Pattern_Length / 2; // Calculate offset for checking the previous candlestick.
                if (index < offset)
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    // Retrieve the previous candlestick to compare with the current one.
                    SmartCandlestick prev = scsList[index - offset];

                    // Determine if the pattern is bullish: the previous candlestick must have closed lower than it opened,
                    // and the current candlestick must close higher than it opened.
                    bool bullish = (prev.open > prev.close) & (scs.close > scs.open);

                    // Determine if the current candlestick's body is completely inside the previous candlestick's body,
                    // which is indicative of a bullish harami.
                    bool harami = (scs.topPrice < prev.topPrice) & (scs.bottomPrice > prev.bottomPrice);

                    // The bullish harami pattern is valid if both the bullish and harami conditions are true.
                    bool bullish_harami = bullish & harami;

                    // Add the result of the bullish harami check to the candlestick's pattern dictionary.
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_harami);

                    return bullish_harami;
                }
            }
        }
    }
}
