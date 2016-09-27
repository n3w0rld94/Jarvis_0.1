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
        public string[] colTypes; // Indicazione generica sul formato dei dati (Categorial o Numeric).
        public string[] subTypes; //Specifica il formato dei dati in colTypes.
        public string[][] distinctValues;// Matrice di vettori di stringhe distinte, ha senso solo per informazioni già tipo stringa.
        public double[] means; // media aritmetica, ha senso solo per informazioni già numeriche.
        public double[] stdDevs; // Deviazione standard, ha senso solo per informazioni già numeriche.
        public int numStandardCols; // Numero di colonne dati che si stampano a schermo alla fine del procedimento di standardizzazione.

        public Standardizer(string[][] rawData, string[] colTypes)
        {
            this.colTypes = new string[colTypes.Length];
            Array.Copy(colTypes, this.colTypes, colTypes.Length);

            // get distinct values in each col. Conta valori distinti in colonne categoric.
            int numCols = rawData[0].Length;
            this.distinctValues = new string[numCols][];
            for (int j = 0; j < numCols; ++j)
            {
                if (colTypes[j] == "numeric")
                {
                    distinctValues[j] = new string[] { "na" }; //Esclude i dati numeric.
                }
                else
                {
                    //Determina il numero di alternative distinte per ciascuna colonna categorica all'interno del Dataset fornito.
                    Dictionary<string, bool> values = new Dictionary<string, bool>();
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
                    this.means[j] = -1.0; // dummy values, Esclude valori Categorical.
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
            double[] result = new double[this.numStandardCols];
            int p = 0; // ptr into result data
            for (int j = 0; j < tuple.Length; ++j)
            {
                if (this.subTypes[j] == "numericX")
                {
                    double v = double.Parse(tuple[j]);
                    result[p++] = (v - this.means[j]) / this.stdDevs[j];
                }
                else if (this.subTypes[j] == "numericY")
                {
                    double v = double.Parse(tuple[j]);
                    result[p++] = v; // leave alone (regression problem)
                }
                else if (this.subTypes[j] == "binaryX")
                {
                    string v = tuple[j];
                    int index = IndexOf(j, v);
                    if (index == 0)
                        result[p++] = -1.0;
                    else
                        result[p++] = 1.0;
                }
                else if (this.subTypes[j] == "binaryY")
                {
                    string v = tuple[j];
                    int index = IndexOf(j, v);
                    if (index == 0) { result[p++] = 0.0; result[p++] = 1.0; }
                    else { result[p++] = 1.0; result[p++] = 0.0; }
                }
                else if (this.subTypes[j] == "categoricalX")
                {
                    string v = tuple[j];
                    int ct = distinctValues[j].Length;
                    double[] tmp = new double[ct - 1];
                    int index = IndexOf(j, v);
                    if (index == ct - 1)
                    {
                        for (int k = 0; k < tmp.Length; ++k)
                            tmp[k] = -1.0;
                    }
                    else
                    {
                        for (int k = 0; k < tmp.Length; ++k)
                            tmp[k] = 0.0; // not necessary in C# . . 
                        tmp[ct - index - 2] = 1.0;
                    }
                    for (int k = 0; k < tmp.Length; ++k)
                        result[p++] = tmp[k];
                }
                else if (this.subTypes[j] == "categoricalY")
                {
                    string v = tuple[j];
                    int ct = distinctValues[j].Length;
                    double[] tmp = new double[ct];
                    int index = IndexOf(j, v);
                    for (int k = 0; k < tmp.Length; ++k)
                        tmp[k] = 0.0; // not necessary in C# . . 
                    tmp[ct - index - 1] = 1.0;
                    for (int k = 0; k < tmp.Length; ++k)
                        result[p++] = tmp[k];
                }
            } // each j col
            return result;
        }

        private int IndexOf(int col, string catValue)
        {
            for (int k = 0; k < this.distinctValues[col].Length; ++k)
                if (distinctValues[col][k] == catValue)
                    return k;
            return -1; // fatal error
        }

        public double[][] StandardizeAll(string[][] rawData)
        {
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                double[] stdRow = this.GetStandardRow(rawData[i]);
                result[i] = stdRow;
            }
            return result;
        }

        
    }
    public class Helpers
    {
        public void ShowMatrix(double[][] Matrix, int numSamples, int numCols)
        {
            for (int i = 0; i < numSamples; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    Console.Write(Matrix[i][j] + " ");
                }
                Console.Write("\n");
            }
        }

    }

}
