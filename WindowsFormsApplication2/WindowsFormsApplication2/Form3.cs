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
            Form1 frm = new Form1(msratios, path);
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
