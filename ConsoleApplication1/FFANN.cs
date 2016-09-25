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
        
        //Costruttore: legge da utente i parametri essenziali della Rete Neurale Feed-Forward
        public FFANN()
        {
            do {
                Console.WriteLine("Inserisci il numero di strati della rete (Minimo Due): ");
                NumLayers = (short)int.Parse(Console.ReadLine());
            } while (NumLayers<2);
            NumPercept = new short[NumLayers+1];

            do
            {
                Console.WriteLine("Inserisci il numero di unità di elaborazione per ogni strato: ");
                for (int i = 0; i < NumLayers; i++)
                {
                    Console.WriteLine("Strato " + i + ":");
                    NumPercept[i] = (short)int.Parse(Console.ReadLine());
                }

            } while ((NumPercept[0]==0)||(NumPercept[NumLayers-1]==0));
            NumPercept[NumLayers] = 0;

        }
        //End Costruttore

        public void build()
        {
            Layer[] strat = new Layer[NumLayers];
            for (int i=0; i < NumLayers; i++)
            {
                strat[i] = new Layer((short)i);
            }
        }
        static void Main(string[] args)
        {
            FFANN ffann = new FFANN();
            Layer[] strat = new Layer[ffann.NumLayers];
            Train trainer = new Train();
            trainer.RPropPlus(ffann);

        }
    }

    class Layer : FFANN
    {
        public short N; //numero identificativo layer
        public Layer(short n)
        {
            N = n;
            Perceptron[] percept = new Perceptron[NumPercept[n]];
            for(int i = 0; i<NumPercept[n]; i++)
            {
                percept[i]=new Perceptron(n);
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
            NumSynapses = NumPercept[N+1];
            synapsys = new double[NumSynapses];
            Random rand = new Random();
            for(int i = 0; i < NumSynapses; i++)
            {
                synapsys[i] = rand.Next();
            }
        }

        public void weightrecalc()
        {
            for(int i = 0; i < NumPercept[this.N + 1]; i++)
            {
                synapsys[i] = 0;
            }   
        }

        public void ActionCalc(Perceptron Per)
        {
            action += 4;
        }


    }

    class Activation
    {
        private double Sigmoid(double x)
        {
            x = 1 / (1 - Math.Pow(Math.E, -x));
            return x;
        }

        private double HiperTan(double x)
        {
            x=(Math.Pow(Math.E, x) - Math.Pow(Math.E, -x))/(Math.Pow(Math.E, x) + Math.Pow(Math.E, -x));
            return x;
        }

        private bool HeivisideStep(double x)
        {
            return (x>=0);
        }

        private double[] Softmax(double[] x, FFANN ffann)
        {
            double TotalDivisor = 0;
            for (int i = 0; i < ffann.NumOut; i++)
                TotalDivisor += Math.Pow(Math.E, x[i]);
            for (int i = 0; i < ffann.NumOut; i++)
                x[i] = Math.Pow(Math.E, x[i]) / TotalDivisor;
            return x;
        }
    }

    class Train
    {
        public void RPropPlus(FFANN ffann)
        {
            


        }

        public void Genetic()
        {

        }

        public void Particle_swarm_optimization()
        {

        }


    }
}
