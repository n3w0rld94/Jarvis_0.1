﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANN
{
    public class FFANN
    {
        //Parametri della rete
        public short NumLayers = 2; //Numero di strati.
        public short[] NumPercept;  //Numero di Percettroni in ciascuno strato (in ordine da strato 0 a numLayers-1).
        Random rand = new Random(); 
        const double Bias = 1;      //Costante euristica (ricavata in trial & error)
        public Layer[] layer;

        //Costruttore, imposta i parametri essenziali della Rete Neurale.

        public FFANN(int numCols, int numOut)
        {
            NumPercept = new short[NumLayers];
            
            NumPercept[0] = (short)(numCols-1);
            //NumPercept[1] = 2;
            NumPercept[NumLayers - 1] = (short)numOut;
            build();
        }
        
        //Inizializzo la rete neurale creando i vari strati e percettroni.
        private void build()
        {
            layer = new Layer[NumLayers];
            for (int i = 0; i < NumLayers; i++)
            {
                layer[i] = new Layer((short)i, this, rand);
            }
        }
        //End Build
        
        //Dati i primi n-1 parametri, predicono l'ultimo.
        public void PredictShow(double[] data)
        {
            readData(data);
            Activation act = new Activation();
            for(int i = 0; i < NumLayers - 1; i++)
                propagate(i, act);
            layer[NumLayers - 1].response(); 
        }

        public void Predict(double[] data)
        {
            readData(data);
            Activation act = new Activation();
            for (int i = 0; i < NumLayers - 1; i++)
                propagate(i, act);
        }
        //End Prediction.

        //inserisce n-1 parametri nel potenziale d'azione dei neuroni di input
        private void readData(double[] data)
        {
            for(int i = 0; i < NumPercept[0]; i++)
                layer[0].perceptron[i].setAction(data[i]);
        }

        //Dato lo strato di partenza, propaga il potenziale d'azione nello strato successivo.
        private void propagate(int i, Activation act)
        {
            int j, k;
            double sum;                             //Accumulatore.
            for (j = 0; j < NumPercept[i + 1]; j++) //Scorro i percettroni dello strato successivo.
            {
                sum = 0; //Azzero l'accumulatore
                for (k = 0; k < NumPercept[i]; k++) //Scorro i percettroni dello strato di partenza.
                    sum += layer[i].perceptron[k].getAction() * layer[i].perceptron[k].getSynapsys(j); /* moltiplica potenziale d'azione 
                                                                                                        * per peso sinaptico della connessione 
                                                                                                        * fra k e j.*/
                sum += Bias;//Se non lo capisci non continuare a leggere e ristudiati la teoria prima.
                layer[i + 1].perceptron[j].setAction(act.LogisticSigmoid(sum)); /* Applica la funzione di costo e assegna il nuovo 
                                                                                 * potenziale d'azione.*/
            }
            /*per applicare il softmax, decommenta questo segmento.*/
            /*if(i == NumLayers - 2)
                act.Softmax(layer[i+1]);*/
        }
    }



    public class Layer
    {
        public short N; //numero identificativo layer
        public Perceptron[] perceptron;
        public FFANN ffann;

        public Layer(short n, FFANN parent, Random rand)
        {
            N = n;
            ffann = parent;
            perceptron = new Perceptron[ffann.NumPercept[N]];
            for (int i = 0; i < ffann.NumPercept[N]; i++)
                perceptron[i] = new Perceptron(i, this, rand);
        }

        public void showLayerAction()
        {
            Console.WriteLine("Layer " +  N + ":\n");
            for(int i = 0; i < ffann.NumPercept[N]; i++)
                Console.WriteLine(perceptron[i].getAction() + "\n");
        }
        public void response()
        {
            string response;
            if (perceptron[1].getAction() > perceptron[0].getAction())
                response = "Poor";
            else response = "Rich";
            Console.Write(response);
        }
        public void setLayerAction(Layer layer)
        {
            for (int i = 0; i < ffann.NumPercept[N]; i++)
                perceptron[i].setAction(layer.perceptron[i].getAction());
        }
    }



    sealed public class Perceptron
    {
        Layer layer;
        double[] synapsys; //Peso di ciascun collegamento neurone-neurone fra strati consecutivi
        double action = 0; //Valore passato durante Train/Prediction
        public int NumSynapses;
        int nOrder; //Indica l'indice del percettrone nel vettore perceptron del corrispettivo layer.
        

        //Metodi protezione dati
        public double getSynapsys(int i)
        {
            return synapsys[i];
        }

        public void setSynapsys(double weight, int i)
        {
            synapsys[i] = weight;
        }

        public void setAction(double data)
        {
            action = data;
        }

        public double getAction()
        {
            return action;
        }

        public int getNOrder()
        {
            return nOrder;
        }
        //end protection methods
        
        //Costruttore di percettroni, inizializza i pesi sinaptici con valori casuali
        public Perceptron(int i, Layer parent, Random rand)
        {
            
            layer = parent;

            if (layer.N < layer.ffann.NumLayers - 1)
            {
                NumSynapses = layer.ffann.NumPercept[layer.N + 1];
                synapsys = new double[NumSynapses];
            }
            else
                NumSynapses = 0;

            nOrder = i;
            for (int j = 0; j < NumSynapses; j++)
                synapsys[j] = rand.NextDouble();
        }
    }
}