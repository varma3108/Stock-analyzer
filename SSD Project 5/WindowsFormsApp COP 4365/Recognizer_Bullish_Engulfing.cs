using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class and specifically identifies bullish engulfing patterns in candlestick data.
    internal class Recognizer_Bullish_Engulfing : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Bullish Engulfing" and a pattern length of 2.
        // This sets up this recognizer for detecting patterns that involve two consecutive candlesticks.
        public Recognizer_Bullish_Engulfing() : base("Bullish Engulfing", 2)
        {
        }

        // Override the abstract Recognize method to determine if a bullish engulfing pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the bullish engulfing pattern from the candlestick's pattern dictionary.
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

                    // Determine if the pattern is bullish: previous candlestick must have closed lower than it opened,
                    // and the current candlestick must close higher than it opened.
                    bool bullsih = (prev.open > prev.close) & (scs.close > scs.open);

                    // Determine if the current candlestick engulfs the previous candlestick: the current must open lower and close higher than the previous.
                    bool engulfing = (scs.topPrice > prev.topPrice) & (scs.bottomPrice < prev.bottomPrice);

                    // The bullish engulfing pattern is valid if both the bullish and engulfing conditions are true.
                    bool bullish_engulfing = bullsih & engulfing;

                    // Add the result of the bullish engulfing check to the candlestick's pattern dictionary.
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_engulfing);

                    return bullish_engulfing;
                }
            }
        }
    }
}

