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

            Console.WriteLine("Ciao e benvenuto nella Learning Part di Jarvis 0.1.");
            Console.ReadKey();

            //Segmento di prova per la standardizzazione dei dati in ingresso (Solo numerici e categorici, no audio/immagini)
            string buffer;
            string datasetPath = "C:/Users/Shea/Documents/Visual Studio 2015/Projects/ConsoleApplication1/Dataset.txt";

            Console.WriteLine("Ora leggo il Dataset " + datasetPath + ":");
            Console.ReadKey();

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

            Console.WriteLine("\nDataset letto: \n");
            for (i = 0; i < Dataset.Length; i++) {
                Console.Write("\n");
                for (int j = 0; j < Dataset[0].Length; j++)
                    Console.WriteLine("{0} ", Dataset[i][j]);
            }
            Console.ReadKey();
            Console.WriteLine("\nLettura effettuata con successo. Provvedo ora a standardizzare il dataset...");
            Console.ReadKey();

            Standardizer std = new Standardizer(Dataset, colTypes);
            stdDataset = std.StandardizeAll(Dataset);
            Helpers helper = new Helpers();
            Console.WriteLine("Dati standardizzati:\n");
            helper.ShowMatrix(stdDataset, numSamples, stdDataset[0].Length);
            Console.ReadKey();
            //Fine segmento di Prova
            
            Console.WriteLine("Dati standardizzati con successo. Ora inizializzo la Rete Neurale...");
            Console.ReadKey();

            FFANN ffann = new FFANN(colTypes.Length, stdDataset[0].Length - Dataset[0].Length + 1); //Setting Network parameters & Creating it.

            Console.WriteLine("\nStruttura della Rete Neurale: \n");
            helper.ShowWeights(ffann);
            Console.ReadKey();

            Console.WriteLine("Rete inizializzata correttamente, ora avvìo l'allenamento della Rete Neurale mediante Backpropagation...");
            Console.ReadKey();

            RPropPlus trainer = new RPropPlus(ffann, stdDataset);

            Console.WriteLine("\nNuova struttura della Rete Neurale: \n");
            helper.ShowWeights(ffann);
            Console.ReadKey();

            Console.WriteLine("Allenamento avvenuto con successo, inserire i dati di prova per effettuare una predizione: ");
            string[] buff;
            do {
                buffer = Console.ReadLine();
            } while (buffer.Length == 0);
            buff = buffer.Split(' ');
            data = std.GetStandardRow(buff);
            ffann.PredictShow(data);
            Console.ReadKey();
        }
    }
}

