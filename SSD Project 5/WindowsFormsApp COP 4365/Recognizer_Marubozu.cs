using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify Marubozu patterns in candlestick data.
    internal class Recognizer_Marubozu : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Marubozu" and a pattern length of 1.
        // This setup targets the recognition of Marubozu patterns that involve a single candlestick, showing strong buying or selling pressure.
        public Recognizer_Marubozu() : base("Marubozu", 1)
        {
        }

        // Override the abstract Recognize method to determine if a Marubozu pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the Marubozu pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Calculate whether the candlestick is a Marubozu by comparing the body range to the total range.
                // A Marubozu is identified if the body (difference between open and close) encompasses more than 96% of the total range (high to low),
                // indicating no or very small shadows.
                bool marubozu = scs.bodyRange > (scs.range * 0.96m);

                // Add the result of the Marubozu pattern check to the candlestick's pattern dictionary for future reference.
                scs.Dictionary_Pattern.Add(Pattern_Name, marubozu);

                // Return the result of the Marubozu pattern check.
                return marubozu;
            }
        }
    }
}

