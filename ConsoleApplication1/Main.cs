using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class Executor
    {
        static void Main(string[] args)
        {
            string[][] DataSet=new string[5][];
            string[] colTypes = new string[5];
            int i = 0;
            Console.WriteLine("Inserisci dati da normalizzare");
            do
            {
                DataSet[i][] = Console.ReadLine();
                i++;
            } while (i < 5);
            Standardizer std = new Standardizer(DataSet, colTypes);
            /* FFANN ffann = new FFANN();
            Layer[] strat = new Layer[ffann.NumLayers];
            int[][] DataSet = new int[2][];
            Train trainer = new Train(ffann);
            ffann.Predict(DataSet);*/
        }
    }
}
