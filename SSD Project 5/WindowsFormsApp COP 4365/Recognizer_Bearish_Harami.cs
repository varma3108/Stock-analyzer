using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Bearish Harami patterns in candlestick data.
    internal class Recognizer_Bearish_Harami : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Bearish Harami" and a pattern length of 2.
        // This sets up this recognizer for detecting patterns that involve two consecutive candlesticks.
        public Recognizer_Bearish_Harami() : base("Bearish Harami", 2)
        {
        }

        // Override the abstract Recognize method to determine if a Bearish Harami pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Bearish Harami pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            else
            {
                // Ensure there is a previous candlestick to compare with; if not, return false and store this result.
                int offset = Pattern_Length / 2;  // Calculate offset for checking the previous candlestick.
                if (index < offset)
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    // Retrieve the previous candlestick to compare with the current one.
                    SmartCandlestick prev = scsList[index - offset];
                    // A Bearish Harami pattern occurs when a smaller black/red candlestick is completely enclosed by the body of a larger white/green candlestick.
                    // First check if the previous candlestick closed higher than it opened (indicative of a bullish candle) and the current candlestick closed lower than it opened (indicative of a bearish candle).
                    bool bearish = (prev.open < prev.close) & (scs.close < scs.open);
                    // Second, check if the current candlestick's top and bottom prices are within the previous candlestick's top and bottom prices.
                    bool harami = (scs.topPrice < prev.topPrice) & (scs.bottomPrice > prev.bottomPrice);
                    // Combine the bearish and harami conditions to determine if a Bearish Harami pattern is present.
                    bool bearish_harami = bearish & harami;
                    // Add the result of the Bearish Harami check to the candlestick's pattern dictionary.
                    scs.Dictionary_Pattern.Add(Pattern_Name, bearish_harami);
                    return bearish_harami;
                }
            }
        }
    }
}

