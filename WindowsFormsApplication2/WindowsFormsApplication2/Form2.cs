using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MSFileReaderLib;


namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        List<double> data = new List<double>();
        public Form2(double[] Values, string flname)
        {
            InitializeComponent();
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            foreach (var item in Values)
            {
                Console.Write(item.ToString()+" ");
            }
            Console.WriteLine();
            this.Text = flname;
            TextBox t = new TextBox();
            t.Text = "Spot Number: " + (Values[0]).ToString();
            t.Location = new Point(15, 15);
            this.Controls.Add(t);
            chart1.Series["MassSpectrum"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["MassSpectrum"].SmartLabelStyle.Enabled = true;
            chart1.Series["MassSpectrum"].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            chart1.Series["MassSpectrum"].SmartLabelStyle.IsMarkerOverlappingAllowed = false;
            chart1.Series["MassSpectrum"].SmartLabelStyle.MovingDirection = LabelAlignmentStyles.TopRight;
            chart1.Series["MassSpectrum"].SmartLabelStyle.IsOverlappedHidden = true;
            chart1.Series["MassSpectrum"].SmartLabelStyle.MinMovingDistance = 4;
            t.BringToFront();
            int[,] sample = new int[2,10];

            MSFileReader_XRawfile rawfile = new MSFileReader_XRawfile();
            rawfile.Open(flname);
            rawfile.SetCurrentController(0, 1);
            int ScanNumber = Convert.ToInt32(Values[0]);

            double Centroid_Peak_Width = 0.0;
            object Mass_List = null;
            object Peak_Flags = null;
            int Array_Size = 0;
            rawfile.GetMassListFromScanNum(ref ScanNumber, null, 1, 0, 0, 0, ref Centroid_Peak_Width, ref Mass_List, ref Peak_Flags, ref Array_Size);
            double[,] ms_list = (double[,])Mass_List;
            //Random rdn = new Random();
            //chart1.Series.Add("Series2");
            double[] found = new double[(Values.Length-2)/2];
            //int start = 2;
            double old_val = 1000;
            for (int i = 0; i < ms_list.Length/2; i++)
            {
                /*for(int j = start; j < Values.Length; j = j + 2)
                {
                    if ((ms_list[0,i] > Values[j]-0.005) && (ms_list[0, i] < Values[j] + 0.005))
                    {
                        found[(j - 2) / 2] = ms_list[0, i];
                        start = j + 2;
                    }
                }*/
                DataPoint dp2 = new DataPoint(ms_list[0, i], ms_list[1, i]);
                chart1.Series["MassSpectrum"].Points.Add(dp2);
                if (i < 10)
                {
                    Insert(ms_list[1, i]);
                }
                else
                {
                    if(ms_list[1,i] > data[0] && Math.Abs(old_val-ms_list[0,i]) > 0.5)
                    {
                        data[0] = ms_list[1, i];
                        MinHeapify(0);
                        old_val = ms_list[0, i];
                    }
                }
            }
            for (int q = 0; q < data.Count; q++)
            {
                DataPoint tempo = chart1.Series["MassSpectrum"].Points.FindByValue(data[q], "Y");
                tempo.Label = (Math.Round(tempo.XValue, 2)).ToString();
                tempo.Font = new Font("Arial", 11, FontStyle.Regular);
                tempo.LabelForeColor = Color.DarkOrange;
            }
            int j;
            for (j = 2; j < Values.Length; j = j+2)
            {
                if (Values[j] == 1.618) break;
                //chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X").IsValueShownAsLabel = true; // = "("+Values[2*j+2]+", "+Values[2*j+3]+")";
                DataPoint dtp = chart1.Series["MassSpectrum"].Points.Aggregate((x, y) => Math.Abs(x.XValue - Values[j]) < Math.Abs(y.XValue - Values[j]) ? x : y);
                //DataPoint dtp = chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X");
                double temp = new double();
                temp = Math.Round(dtp.XValue, 2);
                dtp.LabelForeColor = Color.DarkGreen;
                dtp.Font = new Font("Arial", 11, FontStyle.Bold);
                dtp.Label = temp.ToString();               //"(" + Values[2 * j + 2] + ", " + Values[2 * j + 3] + ")";
                
            }
            for (j = j+1; j < Values.Length; j = j + 2)
            {
                //chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X").IsValueShownAsLabel = true; // = "("+Values[2*j+2]+", "+Values[2*j+3]+")";
                DataPoint dtp = chart1.Series["MassSpectrum"].Points.Aggregate((x, y) => Math.Abs(x.XValue - Values[j]) < Math.Abs(y.XValue - Values[j]) ? x : y);
                //DataPoint dtp = chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X");
                double temp = new double();
                temp = Math.Round(dtp.XValue, 2);
                dtp.LabelForeColor = Color.Maroon;
                dtp.Font = new Font("Arial", 11, FontStyle.Bold);
                dtp.Label = temp.ToString();               //"(" + Values[2 * j + 2] + ", " + Values[2 * j + 3] + ")";

            }
            
            //chart1.Series["Series2"].Enabled = false;
            //chart1.Series["Series2"].IsValueShownAsLabel = true;

            chart1.Series["MassSpectrum"].Color = Color.Blue;
            chart1.Series["MassSpectrum"].IsVisibleInLegend = false;


            LegendItem item1 = new LegendItem();
            item1.ImageStyle = LegendImageStyle.Rectangle;
            item1.Color = Color.Maroon;
            item1.BorderColor = Color.Maroon;
            item1.Cells.Add(LegendCellType.SeriesSymbol, "", ContentAlignment.MiddleCenter);
            item1.Cells.Add(LegendCellType.Text, "Reactants", ContentAlignment.MiddleLeft);
            chart1.Legends[0].CustomItems.Add(item1);

            LegendItem item2 = new LegendItem();
            item2.ImageStyle = LegendImageStyle.Rectangle;
            item2.Color = Color.DarkGreen;
            item2.BorderColor = Color.DarkGreen;
            item2.Cells.Add(LegendCellType.SeriesSymbol, "", ContentAlignment.MiddleCenter);
            item2.Cells.Add(LegendCellType.Text, "Products", ContentAlignment.MiddleLeft);
            chart1.Legends[0].CustomItems.Add(item2);

            LegendItem item3 = new LegendItem();
            item3.ImageStyle = LegendImageStyle.Rectangle;
            item3.Color = Color.DarkOrange;
            item3.BorderColor = Color.DarkOrange;
            item3.Cells.Add(LegendCellType.SeriesSymbol, "", ContentAlignment.MiddleCenter);
            item3.Cells.Add(LegendCellType.Text, "10 Highest Intensiity Peaks", ContentAlignment.MiddleLeft);
            chart1.Legends[0].CustomItems.Add(item3);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public void Insert(double o)
        {
            data.Add(o);

            int i = data.Count - 1;
            while (i > 0)
            {
                int j = (i + 1) / 2 - 1;

                // Check if the invariant holds for the element in data[i]  
                double v = data[j];
                if (v.CompareTo(data[i]) < 0 || v.CompareTo(data[i]) == 0)
                {
                    break;
                }

                // Swap the elements  
                double tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;

                i = j;
            }
        }


        private void MinHeapify(int i)
        {
            int smallest;
            int l = 2 * (i + 1) - 1;
            int r = 2 * (i + 1) - 1 + 1;

            if (l < data.Count && (data[l].CompareTo(data[i]) < 0))
            {
                smallest = l;
            }
            else
            {
                smallest = i;
            }

            if (r < data.Count && (data[r].CompareTo(data[smallest]) < 0))
            {
                smallest = r;
            }

            if (smallest != i)
            {
                double tmp = data[i];
                data[i] = data[smallest];
                data[smallest] = tmp;
                this.MinHeapify(smallest);
            }
        }

        private void button1_Click(object sender, EventArgs e)        {

            this.chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            this.chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }
    }
}
