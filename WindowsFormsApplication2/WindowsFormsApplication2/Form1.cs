using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSFileReaderLib;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Stopwatch sw = Stopwatch.StartNew();
        double[][] mass_ratio;
        double[][] R_mass_ratio;
        Boolean flag = true;
        string fldrpath;
        DataGridView grid = new DataGridView();
        List<string> list = new List<string>();
        public Form1(double[][] ms_rat, double[][] R_ms_rat, string fdpth)
        {
            sw.Start();
            InitializeComponent();
            this.Text = fdpth;
            this.Controls.Add(grid);
            this.ResizeEnd += new EventHandler(Form1_ResizeEnd);
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = false;
            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToAddRows = false;
            //this.Cursor = Cursors.Arrow;
            DataGridViewRow row_forheight = this.grid.RowTemplate;
            //double hei = this.grid.Height / (num_rows);
            row_forheight.Height = 11;//Convert.ToInt32(Math.Floor(hei));
            mass_ratio = ms_rat;
            R_mass_ratio = R_ms_rat;
            fldrpath = fdpth;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int loop_num = 0;
            this.grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseClick);
            grid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            grid.Show();
            // To Do
            IEnumerable<string> sample_files = Directory.GetFiles(fldrpath, "*.raw").OrderBy(f => f);
            string one_file = sample_files.First();
            MSFileReader_XRawfile rawfile = new MSFileReader_XRawfile();
            rawfile.Open(one_file);
            rawfile.SetCurrentController(0, 1);
            int last_scan = 0;
            rawfile.GetLastSpectrumNumber(ref last_scan);
            //
            for (int i = 0; i < last_scan; i++)
            {
                grid.Columns.Add("column" + i.ToString(), i.ToString());
                grid.Columns[i].MinimumWidth = 4;
            }
            Color[] colorSet = { Color.Green, Color.Blue, Color.Aqua, Color.Yellow, Color.Orange, Color.Purple, Color.LightSeaGreen };
            //foreach (string file in Directory.EnumerateFiles(fldrpath, "*.raw"))
            foreach (string file in Directory.GetFiles(fldrpath, "*.raw").OrderBy(f => f))
            {
                bool flag = Analytics.analytics(mass_ratio, R_mass_ratio, file);
                while(flag != true)
                {
                    continue;
                }
                string text_file = file.Remove(file.Length - 4);
                list.Add(text_file);
                //int n = TotalLines(text_file+".txt"); // 50 scans in a file :)
                
                int rowId = grid.Rows.Add();
                DataGridViewRow row = grid.Rows[rowId];
                grid.Rows[rowId].MinimumHeight = 2;
                System.Collections.Generic.IEnumerable<String> lines = File.ReadLines(text_file + ".txt");
                //Button[] buttArray = new Button[n];
                int i = 0;
                foreach (string l in lines)
                {
                    char[] delimiterChars = { ' ' };
                    string[] words = l.Split(delimiterChars);
                    //Console.WriteLine(words[1]);
                    if (Int32.Parse(words[1]) == 0)
                        grid.Rows[rowId].Cells[i].Style.BackColor = Color.Red;
                    else
                        grid.Rows[rowId].Cells[i].Style.BackColor = colorSet[Int32.Parse(words[1])-1];
                    i++;

                }
                loop_num++;
                //if (loop_num >= 2) break;
            }
            //Console.WriteLine(loop_num);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            grid.Dock = DockStyle.Fill;

            this.grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Columns[grid.ColumnCount - 1].MinimumWidth = 8;
            grid.CurrentCell.Selected = false;
            sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            //Console.WriteLine(this.Height);

            //grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
                double hei = this.Height / grid.Rows.Count;
                //Console.WriteLine(hei);
                //DataGridViewRow row_forhei = this.grid.RowTemplate;
                //row_forhei.Height = 2;//Convert.ToInt32(Math.Floor(hei)) - 10;
                foreach(DataGridViewRow trow in grid.Rows)
                {
                    trow.Height = Convert.ToInt32(Math.Floor(hei))-1;
                }
                grid.Columns[grid.ColumnCount - 1].MinimumWidth = 4;
                grid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
                grid.Dock = DockStyle.Fill;
                this.grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //Console.WriteLine(grid.Rows[4].Height);
                //Console.WriteLine(grid.RowTemplate.Height);
                flag = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                grid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
                grid.Dock = DockStyle.Fill;
                this.grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.Columns[grid.ColumnCount - 1].MinimumWidth = 8;
                flag = true;
            }
            else if(flag == true)
            {
                double hei = this.Height / grid.Rows.Count;
                //Console.WriteLine(hei);
                //DataGridViewRow row_forhei = this.grid.RowTemplate;
                //row_forhei.Height = 2;//Convert.ToInt32(Math.Floor(hei)) - 10;
                foreach (DataGridViewRow trow in grid.Rows)
                {
                    trow.Height = Convert.ToInt32(Math.Floor(hei)) - 1;
                }
                grid.Columns[grid.ColumnCount - 1].MinimumWidth = 4;
                grid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
                grid.Dock = DockStyle.Fill;
                this.grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //Console.WriteLine(grid.Rows[4].Height);
                //Console.WriteLine(grid.RowTemplate.Height);
                flag = false;
            }
        }

        private void grid_CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            //Console.WriteLine(e.RowIndex.ToString() + " " + e.ColumnIndex.ToString());
            string file = list[e.RowIndex];
            System.Collections.Generic.IEnumerable<String> lines = File.ReadLines(file + ".txt");
            List<string> lines_list = new List<string>(lines);
            char[] delimiterChars = { ' ' };
            string[] words = lines_list[e.ColumnIndex].Split(delimiterChars);
            double[] lineInt = new double[words.Length - 1];
            for (int j = 0; j < words.Length - 1; j++)
            {
              lineInt[j] = double.Parse(words[j]);
            }
            Form2 fm = new Form2(lineInt, file + ".raw");
            fm.Show();
        }

        int TotalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }
    }
}
