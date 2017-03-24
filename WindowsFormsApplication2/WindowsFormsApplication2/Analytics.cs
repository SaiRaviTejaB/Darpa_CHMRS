using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MSFileReaderLib;

namespace WindowsFormsApplication2
{
    public static class Analytics
    {
        public static bool analytics(double[] m_z, string flname)
        {
            //System.IO.StreamWriter bebe = new System.IO.StreamWriter("test.bebe");
            //bebe.Write("m_z {0} {1}\n", m_z, flname);
            //bebe.Close();

            Dictionary<double, double> mzlist = new Dictionary<double, double>();
            for (int i = 0; i < m_z.Length; i++)
            {
                mzlist.Add(m_z[i], 0);
            }
            //mzlist.Add(443.33, 0);

            MSFileReader_XRawfile rawfile = new MSFileReader_XRawfile();
            rawfile.Open(flname);
            rawfile.SetCurrentController(0, 1);

            int last_scan = 0;
            rawfile.GetLastSpectrumNumber(ref last_scan);
            string txt_fl = flname.Remove(flname.Length - 4);
            System.IO.StreamWriter file = new System.IO.StreamWriter(txt_fl+".txt");
            //System.IO.StreamWriter baba = new System.IO.StreamWriter(txt_fl + ".baba");
            //baba.Write("m_z {0}\n", m_z);
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
                for (int j = 1; j < mslist.Length / 2 ; j++)
                {

                    double mz = mslist[0, j];
                    double intensity = mslist[1, j];
                    int iter = 1;
                    foreach (double mzaux in mzlist.Keys.ToList())
                    {
                        if ((mz > mzaux - 0.05) && (mz < mzaux + 0.05))
                        {
                            //baba.Write("{0},{1},{2} --> {3}\n", ScanNumber, mz, intensity, mzaux);
                            if (intensity >= 30)
                            {
                                if (flag == false)
                                {
                                    file.Write(iter.ToString()+" ");
                                    flag = true;
                                }
                                //baba.Write("{0},{1},{2}\n", ScanNumber, mz, intensity);
                                mzlist[mzaux] += intensity;
                                file.Write(Math.Round(mz, 2) + " " + Math.Round(intensity, 2) + " ");
                            }
                        }
                        iter++;
                    }
                }
                if (flag == true) file.WriteLine();
                else file.WriteLine("0");

            }

            file.Close();
            //baba.Close();
            return true;
        }
    }
}