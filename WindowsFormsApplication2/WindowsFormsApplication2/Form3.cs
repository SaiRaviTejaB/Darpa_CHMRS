using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace WindowsFormsApplication2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string path;

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //start button
        {
            // have code to check if the file is currently being read or not
            string input_msratio = textBox2.Text;
            char[] delimiterChars = { ' ', ',', };
            string[] indmsratios = input_msratio.Split(delimiterChars);
            double[] msratios = Array.ConvertAll(indmsratios, double.Parse);
            double[][] msrt = new double[3][];
            msrt[0] = new double[] { 196.5, 1.0, 197.5, 1.0 };
            msrt[1] = new double[] { 425.55, 0.0, 443.330, 1.0};
            msrt[2] = new double[] { 332.33, 1.0 };
            double[][] R_msrt = new double[3][];
            R_msrt[0] = new double[] { 407.5, 1.0};
            R_msrt[1] = new double[] { 225.55, 0.0, 243.330, 1.0 };
            R_msrt[2] = new double[] { 390.33, 1.0 };
            //msrt[3] = new double[] { 25.5, 0, 25.5, 1 };
            //msrt[4] = new double[] { };
            //Form1 frm = new Form1(msratios, path);
            Form1 frm = new Form1(msrt, R_msrt, path);
            frm.Show();
        }

        private void button1_Click_1(object sender, EventArgs e) //browse button
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please Select the folder that contains the .raw files to be analysed";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath + "\\";
                textBox1.Text = path;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
