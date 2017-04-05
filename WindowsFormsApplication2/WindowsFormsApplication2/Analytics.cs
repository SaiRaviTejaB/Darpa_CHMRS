using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MSFileReaderLib;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public static class Analytics
    {
        public static bool analytics(double[][] m_z, double[][] R_m_z, string flname)
        {
            //Dictionary<double, double> mzlist = new Dictionary<double, double>();
            /*for (int i = 0; i < m_z.GetLength(0); i++)
            {
                mzlist.Add(m_z[i][], 0);
            }*/
            //mzlist.Add(443.33, 0);

            MSFileReader_XRawfile rawfile = new MSFileReader_XRawfile();
            rawfile.Open(flname);
            rawfile.SetCurrentController(0, 1);

            int last_scan = 0;
            rawfile.GetLastSpectrumNumber(ref last_scan);
            string txt_fl = flname.Remove(flname.Length - 4);
            System.IO.StreamWriter file = new System.IO.StreamWriter(txt_fl+".txt");
            for (int ScanNumber = 1; ScanNumber <= last_scan; ScanNumber++)
            {
                double CentroidPeakWidth = 0.0;
                object MassList = null;
                object PeakFlags = null;
                int ArraySize = 0;
                //rawfile.GetMassListFromScanNum(ref ScanNumber, null, 0, 0, 0, 1, ref CentroidPeakWidth, ref MassList, ref PeakFlags, ref ArraySize);
                rawfile.GetMassListFromScanNum(ref ScanNumber, null, 1, 0, 0, 0, ref CentroidPeakWidth, ref MassList, ref PeakFlags, ref ArraySize);

                double[,] mslist = (double[,])MassList;
                
                bool flag = false;
                
                file.Write(ScanNumber + " ");

                int iter = 1;
                bool found = false;
                int row_flag = -1;
                List<double> row_fvalues = new List<double>();

                // For products search
                foreach (double[] row in m_z)
                {
                    for (int lp=0; lp<row.Length+1; lp=lp+2)
                    {
                        if (lp != 0 && row[lp - 1] == 1.0 && found == false)
                        {
                            row_fvalues.Clear();
                            if(row_flag == iter)
                            {
                                FileStream fs = new FileStream(txt_fl + ".txt", FileMode.Open, FileAccess.ReadWrite);
                                fs.SetLength(fs.Length - 2);
                                fs.Close();
                            }
                            break;
                        }
                        if (lp == row.Length) break;
                        found = false;
                        double mzaux = row[lp];
                        for (int j = 1; j < mslist.Length / 2; j++)
                        {
                            double mz = mslist[0, j];
                            double intensity = mslist[1, j];
                            
                            if ((mz > mzaux - 0.05) && (mz < mzaux + 0.05))
                            {
                                if (intensity >= 30)
                                {
                                    if (flag == false)
                                    {
                                        file.Write(iter.ToString() + " ");
                                        flag = true;
                                        row_flag = iter;
                                    }
                                    found = true;
                                    row_fvalues.Add(Math.Round(mz, 2));
                                    row_fvalues.Add(Math.Round(intensity, 2));
                                    //file.Write(Math.Round(mz, 2) + " " + Math.Round(intensity, 2) + " ");
                                    break;
                                }
                            }
                            
                        }
                    }
                    //Write in file
                    foreach(double val in row_fvalues)
                    {
                        file.Write(val + " ");
                    }
                    iter++;
                }
                if (flag == false) file.Write("0" + " ");
                file.Write("1.618" + " ");
                // For Reactants Search
                found = false;
                row_fvalues.Clear();
                foreach (double[] row in R_m_z)
                {
                    for (int lp = 0; lp < row.Length + 1; lp = lp + 2)
                    {
                        if (lp != 0 && row[lp - 1] == 1.0 && found == false)
                        {
                            row_fvalues.Clear();
                            break;
                        }
                        if (lp == row.Length) break;
                        found = false;
                        double mzaux = row[lp];
                        for (int j = 1; j < mslist.Length / 2; j++)
                        {
                            double mz = mslist[0, j];
                            double intensity = mslist[1, j];

                            if ((mz > mzaux - 0.05) && (mz < mzaux + 0.05))
                            {
                                if (intensity >= 30)
                                {
                                    found = true;
                                    row_fvalues.Add(Math.Round(mz, 2));
                                    row_fvalues.Add(Math.Round(intensity, 2));
                                    //file.Write(Math.Round(mz, 2) + " " + Math.Round(intensity, 2) + " ");
                                    break;
                                }
                            }

                        }
                    }
                    //Write in file
                    foreach (double val in row_fvalues)
                    {
                        file.Write(val + " ");
                    }
                }
                file.WriteLine();
            }

            file.Close();
            return true;
        }
    }
}