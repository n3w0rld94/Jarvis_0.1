using System;
using System.IO;

namespace ANN
{
    public class Executor
    {
        static void Main(string[] args)
        {
            string[][] Dataset;
            double[][] stdDataset;
            double[] data;
            string[] colTypes;
            
            //Segmento di prova per la standardizzazione dei dati in ingresso (Solo numerici e categorici, no audio/immagini)
            string buffer;
            string datasetPath = "C:/Users/Shea/Documents/Visual Studio 2015/Projects/ConsoleApplication1/Dataset.txt";

            int numSamples;
            int i = 0;

            StreamReader reader = new StreamReader(datasetPath);
            colTypes = reader.ReadLine().Split(' ');
            numSamples = File.ReadAllLines(datasetPath).Length - 1;
            Dataset = new string[numSamples][];
            data = new double[colTypes.Length];

            while ((buffer = reader.ReadLine()) != null)
            {
                Dataset[i++] = buffer.Split(' ');
            };

            Standardizer std = new Standardizer(Dataset, colTypes);
            stdDataset = std.StandardizeAll(Dataset);
            Helpers helper = new Helpers();
            helper.ShowMatrix(stdDataset, numSamples, stdDataset[0].Length);
            Console.ReadKey();
            //Fine segmento di Prova


            FFANN ffann = new FFANN(colTypes.Length, stdDataset[0].Length - Dataset[0].Length + 1); //Setting Network parameters.
            ffann.build();  //Create & initialize the Network.
            RPropPlus trainer = new RPropPlus(ffann, stdDataset);
            helper.ShowNetwork(ffann);
            Console.WriteLine("Inserire i dati di prova");
            string[] buff = Console.ReadLine().Split(' ');
            data = std.GetStandardRow(buff);
            ffann.PredictShow(data);
            Console.ReadKey();
        }
    }
}

