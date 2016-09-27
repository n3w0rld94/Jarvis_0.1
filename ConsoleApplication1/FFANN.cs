using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANN
{
    public class FFANN
    {
        //Parametri della rete
        public short[] NumPercept;  //Numero di Percettroni in ciascuno strato (in ordine da strato 0 a numLayers-1).
        public short NumLayers = 2; //Numero di strati.
        const double Bias = 1;      //Costante euristica (ricavata in trial & error)
        public Layer[] layer;
        public Random rand = new Random();

        //Costruttore standard, di comodo... per scrivere costruttore layer senza numCols e numOut e base(numCols, numOut)
        public FFANN()
        {

        }

        //Costruttore: legge da utente i parametri essenziali della Rete Neurale Feed-Forward

        public FFANN(int numCols, int numOut)
        {
            NumPercept = new short[NumLayers + 1];
            
            NumPercept[0] = (short)numCols;
            NumPercept[1] = (short)numOut;
            NumPercept[NumLayers] = 0;
        }
        
        //Inizializzo la rete neurale creando i vari strati e percettroni.
        public void build()
        {
            layer = new Layer[NumLayers];
            for (int i = 0; i < NumLayers; i++)
            {
                layer[i] = new Layer((short)i);
            }
        }
        //End Build
        
        //Dati i primi n-1 parametri, predice l'ultimo.
        public void Predict(double[] data)
        {
            readData(data);
            Activation act = new Activation();
            for(int i = 0; i < NumLayers-1; i++)
            {
                propagate(i, act);
            }
            layer[NumLayers - 1].showLayerAction();
            
        }

        //inserisce n-1 parametri nel 1o strato della rete (nel potenziale d'azione dei neuroni di input)
        private void readData(double[] data)
        {
            for(int i = 0; i < data.Length; i++)
            {
                layer[0].perceptron[i].setAction(data[i]);
            }
        }

        //Dato lo strato di partenza, propaga il potenziale d'azione nello strato successivo.
        private void propagate(int i, Activation act)
        {
            int j, k;
            double sum = 0;                         //Accumulatore.
            for (j = 0; j < NumPercept[i + 1]; j++) //Scorro i percettroni dello strato successivo.
            { 
                for (k = 0; k < NumPercept[i]; k++) //Scorro i percettroni dello strato di partenza.
                    sum += layer[i].perceptron[k].getAction() * layer[i].perceptron[k].getSynapsys(j); /* moltiplica potenziale d'azione 
                                                                                                        * per peso sinaptico della connessione 
                                                                                                        * col neurone j dello strato successivo.*/
                sum += Bias; //Se non lo capisci non continuare a leggere e ristudiati la teoria prima.
                layer[i + 1].perceptron[j].setAction(act.LogisticSigmoid(sum)); /* Applica la funzione di costo e assegna il nuovo 
                                                                                 * potenziale d'azione.*/
                                                                               
            }
             /*per applicare il softmax, decommenta questo segmento.
             layer[i+1].setLayerAction(softmax(layer[i+1]));
             */
        }
    }



    public class Layer : FFANN
    {
        public short N; //numero identificativo layer
        public Perceptron[] perceptron;
        public Layer()
        {

        }
        public Layer(short n)
        {
            N = n;
            perceptron = new Perceptron[NumPercept[n]];
            for (int i = 0; i < NumPercept[n]; i++)
            {
                perceptron[i] = new Perceptron();
            }
        }
        public void showLayerAction()
        {
            Console.WriteLine("Layer {D}", N, ":\n");
            for(int i =0; i<NumPercept[N]; i++)
                Console.WriteLine(perceptron[i].getAction() + "\n");
        }

        public void setLayerAction(Layer layer)
        {
            for (int i = 0; i < NumPercept[N]; i++)
                perceptron[i].setAction(layer.perceptron[i].getAction());
        }
    }



    sealed public class Perceptron : Layer
    {
        double[] synapsys; //Peso di ciascun collegamento neurone-neurone fra strati consecutivi
        double action = 0; //Valore passato durante Train/Prediction
        int NumSynapses;

        //Metodi protezione dati
        public double getSynapsys(int i)
        {
            return synapsys[i];
        }

        public void setSynapsys(double weight)
        {

        }

        public void setAction(double data)
        {
            action = data;
        }

        public double getAction()
        {
            return action;
        }
        //end protection methods

            //Costruttore di percettroni, inizializza i pesi sinaptici con valori casuali
        public Perceptron()
        {
            NumSynapses = NumPercept[N + 1];
            synapsys = new double[NumSynapses];
            for (int i = 0; i < NumSynapses; i++)
            {
                synapsys[i] = rand.Next();
            }
        }

        public void weightrecalc()
        {

        }

        public void ActionCalc(Perceptron Per)
        {
            
        }

    }

}


   