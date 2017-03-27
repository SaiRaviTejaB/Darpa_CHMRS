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
                //  (rdn.Next(0, 10), rdn.Next(0, 10));
            }
            for (int j = 2; j < Values.Length; j = j+2)
            {
                //chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X").IsValueShownAsLabel = true; // = "("+Values[2*j+2]+", "+Values[2*j+3]+")";
                DataPoint dtp = chart1.Series["MassSpectrum"].Points.Aggregate((x, y) => Math.Abs(x.XValue - Values[j]) < Math.Abs(y.XValue - Values[j]) ? x : y);
                //DataPoint dtp = chart1.Series["MassSpectrum"].Points.FindByValue(found[j], "X");
                double temp = new double();
                temp = Math.Round(dtp.XValue, 2);
                dtp.Label = temp.ToString();               //"(" + Values[2 * j + 2] + ", " + Values[2 * j + 3] + ")";
                
            }
            //chart1.Series["Series2"].Enabled = false;
            //chart1.Series["Series2"].IsValueShownAsLabel = true;
            
            chart1.Series["MassSpectrum"].Color = Color.Blue;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)        {

            this.chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            this.chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }
    }
}
