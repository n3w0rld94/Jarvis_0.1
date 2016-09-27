using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANN
{
    class FFANN
    {
        //Parametri della rete
        public short[] NumPercept;  //Numero di Percettroni in ciascuno strato
        public short NumLayers = 2; //Numero di strato
        double Bias = 1;            //Costante euristica (ricavata in trial & error)
        public Layer[] strat;



        //Costruttore: legge da utente i parametri essenziali della Rete Neurale Feed-Forward
        public FFANN(int numCols, int numOut)
        {
            /*do
            {
                Console.WriteLine("Inserisci il numero di strati della rete (Minimo Due): ");
                NumLayers = (short)int.Parse(Console.ReadLine());
            } while (NumLayers < 2);*/

            NumPercept = new short[NumLayers + 1];

            /*do
            {
                Console.WriteLine("Inserisci il numero di unità di elaborazione per ogni strato: ");
                for (int i = 0; i < NumLayers; i++)
                {
                    Console.WriteLine("Strato " + i + ":");
                    NumPercept[i] = (short)int.Parse(Console.ReadLine());
                }

            } while ((NumPercept[0] == 0) || (NumPercept[NumLayers - 1] == 0));*/
            
            NumPercept[0] = (short)numCols;
            NumPercept[1] = (short)numOut;
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


        public void Predict(double[][] DataSet)
        {

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

}


   