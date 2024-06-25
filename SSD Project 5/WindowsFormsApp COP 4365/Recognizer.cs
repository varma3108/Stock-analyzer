using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    // Abstract class 'Recognizer' defines the structure for classes that recognize patterns in candlestick data.
    internal abstract class Recognizer
    {
        // Properties to store the name and length of the pattern. These must be set by derived classes.
        public string Pattern_Name;
        public int Pattern_Length;

        // Constructor for the Recognizer class. Initializes pattern name and length.
        protected Recognizer(string pN, int pL)
        {
            Pattern_Name = pN;
            Pattern_Length = pL;
        }

        // Abstract method that must be implemented by derived classes to recognize specific patterns
        // within a list of candlesticks starting at a specific index.
        // Returns True if the pattern is recognized; otherwise, false.
        public abstract bool Recognize(List<SmartCandlestick> scsList, int index);

        // Concrete method that applies the recognition logic to all candlesticks in the list.
        // This method iteratively applies the abstract Recognize method to each candlestick.
        public void Recognize_All(List<SmartCandlestick> scsList)
        {
            for (int i = 0; i < scsList.Count; i++)
            {
                // Calls the abstract Recognize method which must be implemented by the derived class
                Recognize(scsList, i);
            }
        }
    }
}
