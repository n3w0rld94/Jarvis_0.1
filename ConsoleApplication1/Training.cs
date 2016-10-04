using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANN
{
    class Train
    {
        
        public Train(FFANN ffann)
        {

        }

        //Effettua il training sul training set
        private void TrainOnSet(Train Alg, double[][] stdDataset)
        {

        }

        private void PreTrain(Train trainer)
        {

        }

    }

    class RPropPlus
    {
        const double c2 = 1.2, c1 = -0.5;
        double Error = 0;
        double[] desired_output;















        //Calcola la variazione dei pesi sinaptici da applicare a ciascun collegamento sinaptico.
        //Per ogni neurone interno sommo gli errori delle SUE SINAPSI e basta.
        private double Delta(FFANN ffann, int j, int k, int q, double[][] dterr)
        {
            double delta = 0;
            if (j == ffann.NumLayers - 2)
                delta = (-desired_output[q] + ffann.layer[j + 1].perceptron[q].getAction()) * (ffann.layer[j+1].perceptron[q].getAction()*(1 - ffann.layer[j+1].perceptron[q].getAction()));
            else
                delta = dterr[j+1][q]*(ffann.layer[j+1].perceptron[q].getAction()*(1 - ffann.layer[j+1].perceptron[q].getAction()));

            dterr[j][k] += delta * ffann.layer[j].perceptron[k].getSynapsys(q);
            return (c1*delta*ffann.layer[j].perceptron[k].getAction());
        }


















        //Costruttore, si occupa dell'allenamento vero e proprio.
        public RPropPlus(FFANN ffann, double[][] stdDataset)
        {
            int epochs = 0;
            double[][][] delta = new double[ffann.NumLayers-1][][]; //Matrice delta che conterrà le correzione dei pesi da applicare alla fine della propagazione.
            desired_output = new double[ffann.NumPercept[ffann.NumLayers - 1]];


            //inizializzazione di delta[][][]. CORRETTO.
            for (int i = 0; i < ffann.NumLayers-1; i++)
            {
                delta[i] = new double[ffann.NumPercept[i]][];
            }
            for (int j = 0; j < delta.Length; j++)
            {
                for (int i = 0; i < delta[j].Length; i++) //Calcolo il numero totale di sinapsi nella rete.
                {
                    delta[j][i] = new double[ffann.NumPercept[j + 1]];
                }
            }
            //Fine inizializzazione delta[][][].

            double[][] dterr = new double[ffann.NumLayers-1][]; 
            
            //Inizializzo ed azzero la matrice dterr. CORRETTO.
            for (int i = 0; i < ffann.NumLayers-1; i++)
            {
                dterr[i] = new double[ffann.NumPercept[i]];
                for(int z = 0; z < ffann.NumPercept[i]; z++)
                {
                    dterr[i][z] = 0;
                }
            }
            //Fine azzeramento.
            
            do //Scorro le epoche in cui la rete si allenerà. CORRETTO.
            {
                for (int i = 0; i < stdDataset.Length; i++) //Scorro tutti i samples nel dataset.
                {
                    Error = 0;
                    
                    //Copio i valori desiderati in un vettore comodo e calcolo l'Errore Quadratico Medio. CORRETTO.
                    for (int j = 0; j < ffann.NumPercept[ffann.NumLayers - 1]; j++) 
                    {
                        desired_output[j] = stdDataset[i][ffann.NumPercept[0] + j];
                        Error += 0.5 * Math.Pow(ffann.layer[ffann.NumLayers - 1].perceptron[j].getAction() - desired_output[j], 2);
                    }
                    //End

                    ffann.Predict(stdDataset[i]); //Calcolo le uscite della rete dato il sample i. CORRETTO RICORSIVAMENTE.

                    //Calcolo i delta dei pesi sinaptici. CORRETTO.
                    for (int j = ffann.NumLayers - 2; j >= 0; j--) //Scorro la rete dal penultimo layer al primo, per avere accesso diretto ai pesi sinaptici da correggere.
                        for (int k = 0; k < ffann.NumPercept[j]; k++) // Scorro i percettroni dello strato j.
                            for (int q = 0; q < ffann.NumPercept[j + 1]; q++) // Scorro le sinapsi.
                                delta[j][k][q] = Delta(ffann, j, k, q, dterr);

                    //Setto i nuovi pesi applicando i delta dei pesi sinaptici. CORRETTO.
                    for (int j = 0; j < ffann.NumLayers - 1; j++)
                        for (int k = 0; k < ffann.NumPercept[j]; k++)
                            for (int q = 0; q < ffann.NumPercept[j + 1]; q++)
                                ffann.layer[j].perceptron[k].setSynapsys(ffann.layer[j].perceptron[k].getSynapsys(q) - delta[j][k][q], q);
                    
                    //è necessario azzerare dterr per ogni sample analizzato. CORRETTO.
                    for (int j = 0; j < dterr.Length; j++)
                        for (int z = 0; z < dterr[j].Length; z++)
                            dterr[j][z] = 0;
                    //Fine azzeramento.
                }

                //Stampa l'errore quadratico medio se if è true. CORRETTO.
                if(epochs % 1000 == 0)
                    Console.WriteLine("\nErrore Quadratico Medio: " + Error + "\n");

            } while ((++epochs != 10000)&&(Error >= 0.01));
        }
    }







    class Genetic
    {
        double Fitness;

        public Genetic(FFANN ffann, double[][] stdDataset)
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







    class Particle_swarm_optimization
    {
        double Cohesion, Separation;

        public Particle_swarm_optimization(FFANN ffann, double[][] stdDataset)
        {
            Random Rand = new Random();
            Cohesion = Rand.NextDouble();
            Separation = Rand.NextDouble();
        }
    }
}