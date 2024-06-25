using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the abstract Recognizer class to specifically identify valley patterns in candlestick data.
    internal class Recognizer_Valley : Recognizer
    {
        // Constructor inherits from the base Recognizer class with the pattern name "Valley" and a pattern length of 3.
        // This setup targets the recognition of patterns that involve three consecutive candlesticks, where the middle one might be a valley.
        public Recognizer_Valley() : base("Valley", 3)
        {
        }

        // Override the abstract Recognize method to determine if a valley pattern occurs at a given index in a list of candlesticks.
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Retrieve the candlestick at the specified index from the list to analyze.
            SmartCandlestick scs = scsList[index];

            // Attempt to retrieve a previously calculated result for the valley pattern from the candlestick's pattern dictionary.
            // This avoids recalculating the result if it has already been determined.
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                // Return the previously calculated value if it exists.
                return value;
            }
            else
            {
                // Check if there are enough candlesticks before and after the current one to check for a valley.
                // This ensures the pattern calculation does not go out of bounds.
                int offset = Pattern_Length / 2;
                if ((index < offset) || (index == scsList.Count() - offset))
                {
                    // Add a false entry to the pattern dictionary if it's not possible to check the pattern due to boundary constraints.
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;
                }
                else
                {
                    // Retrieve the previous and next candlesticks to compare with the current one.
                    SmartCandlestick prev = scsList[index - offset];
                    SmartCandlestick next = scsList[index + offset];

                    // Determine if the current candlestick forms a valley: it should have a lower low than both the previous and next candlesticks.
                    bool valley = (scs.low < prev.low) && (scs.low < next.low);

                    // Add the result of the valley pattern check to the candlestick's pattern dictionary.
                    scs.Dictionary_Pattern.Add(Pattern_Name, valley);

                    // Return the result of the valley pattern check.
                    return valley;
                }
            }
        }
    }
}

