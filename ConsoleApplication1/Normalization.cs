using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Questa classe si occupa di gestire la normalizzazione dei dati in entrata nella rete.

namespace ANN
{
    public class Standardizer
    {
        public string[] colTypes;
        public string[] subTypes;
        public string[][] distinctValues;
        public double[] means;
        public double[] stdDevs;
        public int numStandardCols;

        public Standardizer(string[][] rawData, string[] colTypes)
        {
            this.colTypes = new string[colTypes.Length];
            Array.Copy(colTypes, this.colTypes, colTypes.Length);

            // get distinct values in each col.
            int numCols = rawData[0].Length;
            this.distinctValues = new string[numCols][];
            for (int j = 0; j < numCols; ++j)
            {
                if (colTypes[j] == "numeric")
                {
                    distinctValues[j] = new string[] { "na" };
                }
                else
                {
                    Dictionary<string, bool> values =
                      new Dictionary<string, bool>();
                    for (int i = 0; i < rawData.Length; ++i)
                    {
                        string v = rawData[i][j];
                        if (values.ContainsKey(v) == false)
                            values.Add(v, true);
                    }
                    distinctValues[j] = new string[values.Count];
                    int k = 0;
                    foreach (string s in values.Keys)
                    {
                        distinctValues[j][k] = s;
                        ++k;
                    }
                }
            }

            // compute means of numeric cols
            this.means = new double[numCols];
            for (int j = 0; j < numCols; ++j)
            {
                if (colTypes[j] == "categorical")
                {
                    this.means[j] = -1.0; // dummy values
                }
                else
                {
                    double sum = 0.0;
                    for (int i = 0; i < rawData.Length; ++i)
                    {
                        double v = double.Parse(rawData[i][j]);
                        sum += v;
                    }
                    this.means[j] = sum / rawData.Length;
                }
            }

            // compute standard deviations of numeric cols
            this.stdDevs = new double[numCols];
            for (int j = 0; j < numCols; ++j)
            {
                if (colTypes[j] == "categorical")
                {
                    this.stdDevs[j] = -1.0; // dummy
                }
                else
                {
                    double ssd = 0.0; // sum of squared deviations
                    for (int i = 0; i < rawData.Length; ++i)
                    {
                        double v = double.Parse(rawData[i][j]);
                        ssd += (v - this.means[j]) * (v - this.means[j]);
                    }
                    this.stdDevs[j] = Math.Sqrt(ssd / rawData.Length);
                }
            }

            // compute column subTypes
            this.subTypes = new string[numCols];
            for (int j = 0; j < numCols; ++j)
            {
                if (colTypes[j] == "numeric" && j != numCols - 1)
                    this.subTypes[j] = "numericX";
                else if (colTypes[j] == "numeric" && j == numCols - 1)
                    this.subTypes[j] = "numericY";
                else if (colTypes[j] == "categorical" && j != numCols - 1 &&
                  distinctValues[j].Length == 2)
                    this.subTypes[j] = "binaryX";
                else if (colTypes[j] == "categorical" && j == numCols - 1 &&
                  distinctValues[j].Length == 2)
                    this.subTypes[j] = "binaryY";
                else if (colTypes[j] == "categorical" && j != numCols - 1 &&
                  distinctValues[j].Length >= 3)
                    this.subTypes[j] = "categoricalX";
                else if (colTypes[j] == "categorical" && j == numCols - 1 &&
                  distinctValues[j].Length >= 3)
                    this.subTypes[j] = "categoricalY";
            }

            // compute number of columns of standardized data
            int ct = 0;
            for (int j = 0; j < numCols; ++j)
            {
                if (this.subTypes[j] == "numericX")
                    ++ct;
                else if (this.subTypes[j] == "numericY")
                    ++ct;
                else if (this.subTypes[j] == "binaryX")
                    ++ct;
                else if (this.subTypes[j] == "binaryY")
                    ct += 2;
                else if (this.subTypes[j] == "categoricalX")
                    ct += distinctValues[j].Length - 1;
                else if (this.subTypes[j] == "categoricalY")
                    ct += distinctValues[j].Length;
            }
            this.numStandardCols = ct;
        }

        public double[] GetStandardRow(string[] tuple)
        {
            double[] d=new double[5];

            return d;
        }

        private int IndexOf(int col, string catValue)
        {

            return col;
        }
        public double[][] StandardizeAll(string[][] rawData)
        {
            double[][] c= new double[6][];

            return c;
        }
    }
}
