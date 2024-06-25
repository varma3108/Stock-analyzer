using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Candlestick
    {
        // Properties to hold candlestick data
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public ulong volume { get; set; }
        public DateTime date { get; set; }

        // Default Constructor
        public Candlestick()
        {
            // This constructor doesn't initialize properties with any specific values
        }

        // Constructor that initializes properties from a CSV string
        public Candlestick(string csvLine)
        {
            // Define delimiters to split the CSV line
            char[] separators = new char[] { ',', ' ', '"' };
            // Split the input string into an array, removing empty entries
            string[] subs = csvLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            // Parse the date from the first substring
            string dateString = subs[0];
            date = DateTime.Parse(dateString);

            // Temporary variable to hold the parsed decimal value
            decimal temp;
            // Parse 'open' price from the second substring and assign it if the parse is successful
            bool success = decimal.TryParse(subs[1], out temp);
            if (success) open = temp;

            // Parse 'high' price from the third substring
            success = decimal.TryParse(subs[2], out temp);
            if (success) high = temp;

            // Parse 'low' price from the fourth substring
            success = decimal.TryParse(subs[3], out temp);
            if (success) low = temp;

            // Parse 'close' price from the fifth substring
            success = decimal.TryParse(subs[4], out temp);
            if (success) close = temp;

            // Temporary variable to hold the parsed ulong value for volume
            ulong tempVolume;
            // Parse 'volume' from the seventh substring
            success = ulong.TryParse(subs[6], out tempVolume);
            if (success) volume = tempVolume;
        }
    }
}

