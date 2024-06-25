using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp_COP_4365
{
    public partial class Form_StockViewer : Form
    {
        //List of all candlesticks read from file
        private List<SmartCandlestick> candlesticks = null;
        //Binding list of candlesticks bound to DataGridView 
        private BindingList<SmartCandlestick> boundCandlesticks = null;
        //Variable to store starting date
        private DateTime startDate = new DateTime(2022, 1, 1);
        //Variable to store ending date
        private DateTime endDate = DateTime.Now;
        //Dictionary to store all Recognizers
        private Dictionary<string, Recognizer> Dictionary_Recognizer;
        //Highest total chart value
        private double chartMax;
        //Lowest total chart value
        private double chartMin;

        /// <summary>
        /// Base Form Constructor
        /// </summary>
        public Form_StockViewer()
        {
            InitializeComponent();
            InitializeRecognizer();

            //Construct list of candlesticks with size 1024
            candlesticks = new List<SmartCandlestick>(1024);
            //Pre-set the date time picker to the specified start and current time end
            dateTimePicker_startDate.Value = startDate;
            dateTimePicker_endDate.Value = endDate;
        }

        /// <summary>
        /// Child Form Constructor
        /// </summary>
        /// <param name="stockPath">"File path of the child form (2 - n file selection)"</param>
        /// <param name="start">"Starting date of parent passed to child"</param>
        /// <param name="end">"Ending date of parent passed to child"</param>
        public Form_StockViewer(string stockPath, DateTime start, DateTime end)
        {
            InitializeComponent();
            InitializeRecognizer();

            //Set date from parent form
            dateTimePicker_startDate.Value = startDate = start;
            dateTimePicker_endDate.Value = endDate = end;
            //Read file of child
            candlesticks = goReadFile(stockPath);
            //Filter list of child
            filterList();
            //Display data of child
            displayCandlesticks();
        }

        /// <summary>
        /// Open stock file button event to open file explorer on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_openFile_Click(object sender, EventArgs e)
        {
            //On button click change text of window form
            Text = "Opening File...";
            //On button click display the file explorer
            openFileDialog_stockPick.ShowDialog();
        }

        /// <summary>
        /// Update button event to update displayed date to be within specified date range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Update_Click(object sender, EventArgs e)
        {
            //Check if no data to filter or if dates are invalid format (start date is after end date)
            if ((candlesticks.Count != 0) & (startDate <= endDate))
            {
                //Filter list
                filterList();
                //Display data
                displayCandlesticks();
            }
        }

        /// <summary>
        /// Open file dialog event to open selected file,
        /// Then read data from file into list and pre-set date,
        /// Lastly bind and display data to data grid and chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog_stockPick_FileOk(object sender, CancelEventArgs e)
        {
            //Store number of files selected
            int numberOfFiles = openFileDialog_stockPick.FileNames.Count();
            //Go through each selected filename in the open file dialog
            for (int i = 0; i < numberOfFiles; ++i)
            {
                //Get the pathname of current file
                string pathName = openFileDialog_stockPick.FileNames[i];
                string ticker = Path.GetFileNameWithoutExtension(pathName);

                //Create form to view
                Form_StockViewer form_StockViewer;
                //If first form then set to parent
                if (i == 0)
                {
                    //Read the file and display the stock
                    form_StockViewer = this;
                    readAndDisplayStock();
                    form_StockViewer.Text = "Parent: " + ticker;
                }
                else
                {
                    //Instantiate new form using parameter constructor
                    form_StockViewer = new Form_StockViewer(pathName, startDate, endDate);
                    form_StockViewer.Text = "Child: " + ticker;
                }

                //Display the new form
                form_StockViewer.Show();
                form_StockViewer.BringToFront();
            }
        }

        /// <summary>
        /// Method that reads csv data from passed file, store in candlestick list, then return list
        /// </summary>
        /// <param name="filename">"Name of the file"</param>
        /// <returns></returns>
        private List<SmartCandlestick> goReadFile(string filename)
        {
            //Display filename
            this.Text = Path.GetFileName(filename);
            //Initialize string for reference to starting line of opened CSV file
            const string referenceString = "Date,Open,High,Low,Close,Adj Close,Volume";

            //Construct list
            List<SmartCandlestick> list = new List<SmartCandlestick>();
            //Pass file path and filename to StreamReader constructor
            using (StreamReader sr = new StreamReader(filename))
            {
                //Read first line from new file
                string line = sr.ReadLine();
                //If starting line of file equals reference line: OK
                if (line == referenceString)
                {
                    //Read until end of file
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Instantiate Candlestick represented by string
                        SmartCandlestick cs = new SmartCandlestick(line);
                        list.Add(cs);
                    }
                }
                //Changes form text to error representing bad file opened
                else
                { Text = "Bad File: " + Path.GetFileName(filename); }
            }

            //Run all Recognizers on list
            foreach (Recognizer r in Dictionary_Recognizer.Values)
            {
                //Adds dictionary entries for every pattern on every candlestick
                r.Recognize_All(list);
            }

            return list;
        }

        /// <summary>
        /// Overload of goReadFile
        /// </summary>
        private void goReadFile()
        {
            //Go read data from file into candlesticks list
            candlesticks = goReadFile(openFileDialog_stockPick.FileName);
            //Bind list to binding list
            boundCandlesticks = new BindingList<SmartCandlestick>(candlesticks);
        }

        /// <summary>
        /// Method that parses through candlesticks,
        /// Add candlesticks within date range from candlestick list into new list,
        /// Return filtered list
        /// </summary>
        /// <param name="list">"Candlesticks list containing all data"</param>
        /// <returns></returns>
        private List<SmartCandlestick> filterList(List<SmartCandlestick> list, DateTime start, DateTime end)
        {
            List<SmartCandlestick> filter = new List<SmartCandlestick>(list.Count);
            //Iterate through each candlestick in list
            foreach (SmartCandlestick cs in list)
            {
                //Check if date is inclusively within range and store in filtered list
                if ((cs.date >= start) & (cs.date <= end))
                { filter.Add(cs); }
            }
            return filter;
        }

        /// <summary>
        /// Overload of filterList method
        /// </summary>
        private void filterList()
        {
            //Go filter the candlesticks list and place in temp variable called filter
            List<SmartCandlestick> filterCandlesticks = filterList(candlesticks, startDate, endDate);
            //Bind filtered list to binding list
            boundCandlesticks = new BindingList<SmartCandlestick>(filterCandlesticks);
            //Update Combo Box after setting bound data
        }

        /// <summary>
        /// Method that displays the data to a chart after normalizing the Y axis
        /// </summary>
        /// <param name="bindList">"Binding list containing the data that needs to be displayed"</param>
        private void displayCandlesticks(BindingList<SmartCandlestick> bindList)
        {
            //Dyanmically set the Y Axis of the chart to normalize chart size
            normalizeChart();

            //Clear annotations for new chart (new form or new file)
            chart_OHLCV.Annotations.Clear();

            //Display data by binding list of data to chart
            chart_OHLCV.DataSource = bindList;
            chart_OHLCV.DataBind();
        }

        /// <summary>
        /// Overload of displayCandlesticks
        /// </summary>
        private void displayCandlesticks()
        {
            //Go set the data source of grid and chart to binding list and normalize chart
            displayCandlesticks(boundCandlesticks);
        }

        /// <summary>
        /// Normalizes the chart by finding the lowest and highest value in total data
        /// Sets the Y axis to 2% more and less than the highest and lowest value respectively
        /// </summary>
        /// <param name="bindList">"Binding list containing the data that needs to be displayed"</param>
        private void normalizeChart(BindingList<SmartCandlestick> bindList)
        {
            //Set starting conditions for min and max variables
            decimal min = 1000000000, max = 0;
            //Iterate through each candle stick in list
            foreach (SmartCandlestick c in bindList)
            {
                //Check for greatest value (Ymax) and lowest value (Ymin)
                if (c.low < min) { min = c.low; }
                if (c.high > max) { max = c.high; }
            }
            //Set the Y axis of the chart area to (+-)2% of the ranges rounded to 2 decimal places
            chartMin = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = Math.Floor(Decimal.ToDouble(min) * 0.98);
            chartMax = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = Math.Ceiling(Decimal.ToDouble(max) * 1.02);
        }

        /// <summary>
        /// Overload of normalizeChart
        /// </summary>
        private void normalizeChart()
        {
            //Go find the minimum and maximum low and high values from all of the candlesticks
            normalizeChart(boundCandlesticks);
        }

        /// <summary>
        /// Reads the data from the file, filters the list by dates, and displays data to chart
        /// </summary>
        private void readAndDisplayStock()
        {
            //Read file
            goReadFile();
            //Filter list
            filterList();
            //Display data
            displayCandlesticks();
        }

        /// <summary>
        /// Method that initializes every pattern Recognizer class and stores them in a dictionary keyed by the pattern name
        /// Uses dictionary keys to fill combo box items
        /// </summary>
        private void InitializeRecognizer()
        {
            Dictionary_Recognizer = new Dictionary<string, Recognizer>();

            //Bullish Recognizer
            Recognizer r = new Recognizer_Bullish();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Bearish Recognizer
            r = new Recognizer_Bearish();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Neutral Recognizer
            r = new Recognizer_Neutral();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Marubozu Recognizer
            r = new Recognizer_Marubozu();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Hammer Recognizer
            r = new Recognizer_Hammer();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Doji Recognizer
            r = new Recognizer_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Dragonfly Doji Recognizer
            r = new Recognizer_Dragonfly_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Gravestone Doji Recognizer
            r = new Recognizer_Gravestone_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Bullish Engulfing Recognizer
            r = new Recognizer_Bullish_Engulfing();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Bearish Engulfing Recognizer
            r = new Recognizer_Bearish_Engulfing();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Bullish Harami Recognizer
            r = new Recognizer_Bullish_Harami();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Bearish Harami Recognizer
            r = new Recognizer_Bearish_Harami();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Peak Recognizer
            r = new Recognizer_Peak();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);
            //Valley Recognizer
            r = new Recognizer_Valley();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            //Initialize Combo Box
            comboBox_Patterns.Items.AddRange(Dictionary_Recognizer.Keys.ToArray());
        }

        /// <summary>
        /// Date time picker event to update starting date when value of picker changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker_startDate_ValueChanged(object sender, EventArgs e)
        {
            //Store starting date
            startDate = dateTimePicker_startDate.Value;
        }

        /// <summary>
        /// Date time picker event to update ending date when value of picker changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker_endDate_ValueChanged(object sender, EventArgs e)
        {
            //Store ending date
            endDate = dateTimePicker_endDate.Value;
        }

        /// <summary>
        /// Combo box event to update the annotations on chart when selected item of box is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Patterns_SelectedIndexChanged(object sender, EventArgs e) //IN PROGRESS CURRENTLY BEING CHANGED
        {
            //ARROW FOR SINGLE RECTANGLE FOR DOUBLE
            //Clear annotations for combo box item change
            chart_OHLCV.Annotations.Clear();
            if (boundCandlesticks != null)
            {
                //Iterate through displayed data
                for (int i = 0; i < boundCandlesticks.Count; i++)
                {
                    //Create smart candlestick for current indexed candlestick
                    SmartCandlestick scs = boundCandlesticks[i];
                    //Set data point of current candlestick from chart
                    DataPoint point = chart_OHLCV.Series[0].Points[i];

                    string selected = comboBox_Patterns.SelectedItem.ToString();    //Store string of pattern name
                    //Displays annotation for current candlestick if selected pattern from dictionary is true
                    if (scs.Dictionary_Pattern[selected])
                    {
                        int length = Dictionary_Recognizer[selected].Pattern_Length;    //Store length of pattern
                        //Annotate candlesticks for multi-candlestick patterns
                        if (length > 1)
                        {
                            //Skip indexes that cause out of bounds error
                            if (i == 0 | ((i == boundCandlesticks.Count() - 1) & length == 3))
                            {
                                continue;
                            }
                            //Initialize rectangle annotation
                            RectangleAnnotation rectangle = new RectangleAnnotation();
                            rectangle.SetAnchor(point);

                            double Ymax, Ymin;
                            double width = (90.0 / boundCandlesticks.Count()) * length; //Scale width to number of candlesticks
                            //Find the min and max between every candlestick in pattern
                            if (length == 2)    //Even number pattern
                            {
                                Ymax = (int)(Math.Max(scs.high, boundCandlesticks[i - 1].high));
                                Ymin = (int)(Math.Min(scs.low, boundCandlesticks[i - 1].low));
                                rectangle.AnchorOffsetX = ((width / length) / 2 - 0.25) * (-1);  //Offset even pattern for previous candlestick
                            }
                            else    //Odd number pattern
                            {
                                Ymax = (int)(Math.Max(scs.high, Math.Max(boundCandlesticks[i + 1].high, boundCandlesticks[i - 1].high)));
                                Ymin = (int)(Math.Min(scs.low, Math.Min(boundCandlesticks[i + 1].low, boundCandlesticks[i - 1].low)));
                            }
                            double height = 40.0 * (Ymax - Ymin) / (chartMax - chartMin); ; //Scale height to chart bounds
                            rectangle.Height = height; rectangle.Width = width;             //Set width and hight
                            rectangle.Y = Ymax;                                             //Set Y to highest Y value for candlesticks
                            rectangle.BackColor = Color.Transparent;                        //Set area to transparent to see chart
                            rectangle.LineWidth = 2;                                        //Set perimeter width
                            rectangle.LineDashStyle = ChartDashStyle.Dash;                  //Set perimeter style to dashed
                            //Add annotation to chart
                            chart_OHLCV.Annotations.Add(rectangle);
                        }

                        //Initilialize arrow annotation
                        ArrowAnnotation arrow = new ArrowAnnotation();
                        //Set arrow annotation properties
                        arrow.AxisX = chart_OHLCV.ChartAreas[0].AxisX;
                        arrow.AxisY = chart_OHLCV.ChartAreas[0].AxisY;
                        arrow.Width = 0.5;
                        arrow.Height = 0.5;
                        //Annotate single pattern and main candlestick for multi-candlesticks
                        arrow.SetAnchor(point);
                        chart_OHLCV.Annotations.Add(arrow);
                    }
                }
            }
        }

        private void Form_Project_2_Load(object sender, EventArgs e)
        {

        }
    }
}
