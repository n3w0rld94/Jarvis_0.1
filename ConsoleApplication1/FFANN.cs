using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANN
{
    class FFANN
    {
        //Parametri della rete
        public short[] NumPercept; //Numero di Percettroni in ciascuno strato
        public short NumLayers; //Numero di strato
        double Bias = 1; //Costante euristica (ricavata in trial & error)
        public Layer[] strat;



        //Costruttore: legge da utente i parametri essenziali della Rete Neurale Feed-Forward
        public FFANN()
        {
            do {
                Console.WriteLine("Inserisci il numero di strati della rete (Minimo Due): ");
                NumLayers = (short)int.Parse(Console.ReadLine());
            } while (NumLayers < 2);
            NumPercept = new short[NumLayers + 1];

            do
            {
                Console.WriteLine("Inserisci il numero di unità di elaborazione per ogni strato: ");
                for (int i = 0; i < NumLayers; i++)
                {
                    Console.WriteLine("Strato " + i + ":");
                    NumPercept[i] = (short)int.Parse(Console.ReadLine());
                }

            } while ((NumPercept[0] == 0) || (NumPercept[NumLayers - 1] == 0));
            NumPercept[NumLayers] = 0;
        }



        //Inizializzo la rete neurale creando i vari strati e percettroni.
        public void build()
        {
            strat = new Layer[NumLayers];
            for (int i = 0; i < NumLayers; i++)
            {
                strat[i] = new Layer((short)i);
            }
        }
        //End Build


        public void Predict(int[][] DataSet)
        {

        }

    }



    public class Executor
    {
        static void Main(string[] args)
        {
            FFANN ffann = new FFANN();
            Layer[] strat = new Layer[ffann.NumLayers];
            int[][] DataSet = new int[2][];
            Train trainer = new Train(ffann);
            ffann.Predict(DataSet);
        }
    }



    class Layer : FFANN
    {
        public short N; //numero identificativo layer
        public Perceptron[] percept;
        public Layer(short n)
        {
            N = n;
            percept = new Perceptron[NumPercept[n]];
            for (int i = 0; i < NumPercept[n]; i++)
            {
                percept[i] = new Perceptron(n);
            }
        }
    }



    sealed class Perceptron : Layer
    {
        double[] synapsys; //Peso di ciascun collegamento neurone-neurone fra strati consecutivi
        double action = 0; //Valore passato durante Train/Prediction
        int NumSynapses;

        public Perceptron(short n) : base(n)
        {
            NumSynapses = NumPercept[N + 1];
            synapsys = new double[NumSynapses];
            Random rand = new Random();
            for (int i = 0; i < NumSynapses; i++)
            {
                synapsys[i] = rand.Next();
            }
        }

        public void weightrecalc()
        {
            for (int i = 0; i < NumPercept[this.N + 1]; i++)
            {
                synapsys[i] = 0;
            }
        }

        public void ActionCalc(Perceptron Per)
        {
            action += 4;
        }


    }

    
    
    //Questa classe astratta contiene tutti i metodi corrispondenti alle funzioni di attivazione principali,
    //Semplici formule matematiche

    abstract class Activation
    {
        public double LogisticSigmoid(double x)
        {
            x = 1 / (1 - Math.Pow(Math.E, -x));
            return x;
        }

        public double HiperTan(double x)
        {
            x = (Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x));
            return x;
        }

        public bool HeivisideStep(double x)
        {
            return (x >= 0);
        }

        public double[] Softmax(double[] x, FFANN ffann)
        {
            double TotalDivisor = 0;
            for (int i = 0; i < ffann.NumPercept[ffann.NumLayers]; i++)
                TotalDivisor += Math.Pow(Math.E, x[i]);
            for (int i = 0; i < ffann.NumPercept[ffann.NumLayers]; i++)
                x[i] = Math.Pow(Math.E, x[i]) / TotalDivisor;
            return x;
        }
    }



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

        public double[] GetStandardRow(string[] tuple) {



        }

        private int IndexOf(int col, string catValue) {


        }
        public double[][] StandardizeAll(string[][] rawData) {



        }
    }



    class Train
    {
        public Train trainer;
        int[][] DataSet;
        public Train(FFANN ffann)
        {
            Console.WriteLine("\nSeleziona Algoritmo di allenamento. 0(RProp+)  1(GA)  2(PSO): ");
            short m = (short)int.Parse(Console.ReadLine());
            switch (m)
            {
                case 0:
                    trainer = new RPropPlus(ffann);
                    break;
                case 1:
                    trainer = new Genetic(ffann);
                    break;
                case 2:
                    trainer = new Particle_swarm_optimization(ffann);
                    break;
            }
            PreTrain(trainer);
            TrainOnSet(trainer, DataSet);
        }

        //Effettua il training sul training set
        private void TrainOnSet(Train Alg, int[][] DataSet)
        {

        }

        private void PreTrain(Train trainer)
        {

        }

    }



    class RPropPlus : Train
    {
            const double LearnRatePlus = 1.2, LearnRateMinus = 0.5;

        public RPropPlus(FFANN ffann): base(ffann)
        {
            for (int i = 0; i < ffann.NumLayers; i++)
            {
                Console.WriteLine(ffann.strat[i].percept.Equals(LearnRateMinus));
                Console.WriteLine(ffann.strat[i].percept.Equals(LearnRatePlus));
            }
        }

    }



    class Genetic : Train
    {
        double Fitness;

        public Genetic(FFANN ffann): base(ffann)
        {
            Random Rand = new Random();
            Fitness = Rand.NextDouble();
        }


        private void Cross(FFANN ffann)
        {

        }

        private void Mutate(FFANN ffann)
        {

        }

        private void Reproduce()
        {

        }

    }



    class Particle_swarm_optimization : Train
    {
        double Cohesion, Separation;

        public Particle_swarm_optimization(FFANN ffann): base(ffann)
        {
            Random Rand = new Random();
            Cohesion = Rand.NextDouble();
            Separation = Rand.NextDouble();
            TrainOnSet();
        }

        private void TrainOnSet()
        {

        }

        public void PreTrain()
        {

        }
    
    }

    

}
