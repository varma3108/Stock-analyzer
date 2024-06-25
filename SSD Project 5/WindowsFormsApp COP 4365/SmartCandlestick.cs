using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Inherits from the base Candlestick class and adds functionality to compute and store various technical indicators and patterns.
    internal class SmartCandlestick : Candlestick
    {
        // Properties for detailed candlestick analysis
        public decimal range { get; set; }         // Total price movement within the candle
        public decimal topPrice { get; set; }      // Higher price of the open or close
        public decimal bottomPrice { get; set; }   // Lower price of the open or close
        public decimal bodyRange { get; set; }     // The size of the main body of the candle
        public decimal upperTail { get; set; }     // Distance from high to top of the body
        public decimal lowerTail { get; set; }     // Distance from low to bottom of the body
        // Dictionary to store the true/false values of each identified pattern
        public Dictionary<string, bool> Dictionary_Pattern = new Dictionary<string, bool>();

        // Constructor that accepts a CSV line and initializes the candlestick from it.
        public SmartCandlestick(string csvLine) : base(csvLine)
        {
            ComputeExtraProperties();
            ComputePatternProperties();
        }

        // Conversion constructor that transforms a basic Candlestick into a SmartCandlestick
        public SmartCandlestick(Candlestick cs)
        {
            date = cs.date;
            open = cs.open;
            close = cs.close;
            high = cs.high;
            low = cs.low;
            volume = cs.volume;
            ComputeExtraProperties();
            ComputePatternProperties();
        }

        /// <summary>
        /// Computes additional properties related to the candlestick geometry.
        /// </summary>
        private void ComputeExtraProperties()
        {
            range = high - low;                            // Calculate total price movement range
            topPrice = Math.Max(open, close);              // Determine the top price of the body
            bottomPrice = Math.Min(open, close);           // Determine the bottom price of the body
            bodyRange = topPrice - bottomPrice;            // Calculate the body range
            upperTail = high - topPrice;                   // Calculate the upper tail length
            lowerTail = bottomPrice - low;                 // Calculate the lower tail length
        }

        /// <summary>
        /// Computes and stores whether certain candlestick patterns are true or false.
        /// </summary>
        private void ComputePatternProperties()
        {
            // Add bullish pattern based on close and open prices
            Dictionary_Pattern.Add("Bullish", close > open);
            // Add bearish pattern based on open and close prices
            Dictionary_Pattern.Add("Bearish", open > close);
            // Neutral pattern where the body is less than 3% of the range
            Dictionary_Pattern.Add("Neutral", bodyRange < (range * 0.03m));
            // Marubozu pattern where the body is more than 96% of the range
            Dictionary_Pattern.Add("Marubozu", bodyRange > (range * 0.96m));
            // Hammer pattern calculation
            Dictionary_Pattern.Add("Hammer", ((range * 0.20m) < bodyRange) && (bodyRange < (range * 0.33m)) && (lowerTail > range * 0.66m));
            // Doji pattern where the body is less than 3% of the range
            Dictionary_Pattern.Add("Doji", bodyRange < (range * 0.03m));
            // Dragonfly Doji pattern where the lower tail is more than 66% of the range and meets Doji criteria
            Dictionary_Pattern.Add("Dragonfly Doji", bodyRange < (range * 0.03m) && (lowerTail > range * 0.66m));
            // Gravestone Doji pattern where the upper tail is more than 66% of the range and meets Doji criteria
            Dictionary_Pattern.Add("Gravestone Doji", bodyRange < (range * 0.03m) && (upperTail > range * 0.66m));
        }
    }
}

