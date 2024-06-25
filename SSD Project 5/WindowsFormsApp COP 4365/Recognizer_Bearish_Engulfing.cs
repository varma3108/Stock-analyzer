using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class and specifically identifies bearish engulfing patterns in candlestick data.
    internal class Recognizer_Bearish_Engulfing : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Bearish Engulfing" and a pattern length of 2.
        // This sets up this recognizer for detecting patterns that involve two candlesticks.
        public Recognizer_Bearish_Engulfing() : base("Bearish Engulfing", 2)
        {
        }

        // Override the abstract Recognize method to determine if a bearish engulfing pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Attempt to retrieve a previously calculated result for the bearish engulfing pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;
            }
            else
            {
                // Check if the current index allows for checking a pattern that includes the previous candlestick.
                // Pattern_Length / 2 is used to determine the offset to the previous candlestick involved in the pattern.
                int offset = Pattern_Length / 2;
                if (index < offset)
                {
                    // Add a false entry to the pattern dictionary if it's not possible to check the pattern because it's out of bounds.
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    // Retrieve the previous candlestick to compare with the current one.
                    SmartCandlestick prev = scsList[index - offset];
                    // Determine if the pattern is bearish: the previous candlestick must have closed higher than it opened,
                    // and the current candlestick must close lower than it opened.
                    bool bearish = (prev.open < prev.close) && (scs.close < scs.open);
                    // Determine if the current candlestick engulfs the previous candlestick: the current must open higher and close lower than the previous.
                    bool engulfing = (scs.topPrice > prev.topPrice) && (scs.bottomPrice < prev.bottomPrice);
                    // The bearish engulfing pattern is valid if both the bearish and engulfing conditions are true.
                    bool bearish_engulfing = bearish && engulfing;
                    // Add the result of the bearish engulfing check to the candlestick's pattern dictionary.
                    scs.Dictionary_Pattern.Add(Pattern_Name, bearish_engulfing);
                    return bearish_engulfing;
                }
            }
        }
    }
}

