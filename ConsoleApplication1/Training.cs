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
        const double c2 = 1.2, c1 = 0.5;
        
        //Calcola la variazione dei pesi sinaptici da applicare a ciascun collegamento sinaptico.
        //Per ogni neurone interno sommo gli errori delle SUE SINAPSI e basta.
        private double Delta(FFANN ffann, double[] desired_output, int j, int k, int q, double[][] dterr)
        {
            double delta = 0;
            if (j == ffann.NumLayers - 2)
            {
                delta = (desired_output[q] - ffann.layer[j + 1].perceptron[q].getAction()) * (ffann.layer[j+1].perceptron[k].getAction() - Math.Pow(ffann.layer[j+1].perceptron[k].getAction(), 2));
                dterr[j][k] += delta * ffann.layer[j].perceptron[k].getSynapsys(q);
            }
            else
            {
                delta = (ffann.layer[j+1].perceptron[k].getAction() - Math.Pow(ffann.layer[j+1].perceptron[k].getAction(), 2))*dterr[j+1][q];
                dterr[j][k] += delta * ffann.layer[j].perceptron[k].getSynapsys(q);
            }
            return (c1*delta*ffann.layer[j].perceptron[k].getAction());
        }

        private double ERR(double[] desired_output, FFANN ffann, int j)
        {
            double err = 0;
            for (int i = 0; i < desired_output.Length; i++) {
                err += desired_output[i] - ffann.layer[ffann.NumLayers - 1].perceptron[j].getAction();
            }
            return err;
        }

        
        public RPropPlus(FFANN ffann, double[][] stdDataset)
        {
            long epochs = 0;
            int z;
            double error = 0;
            double[] desired_output = new double[ffann.NumPercept[ffann.NumLayers-1]]; 
            double[][] dterr = new double[ffann.NumLayers-1][];
            //Azzero la matrice dterr.
            for (int i = 0; i < ffann.NumLayers-1; i++)
            {
                dterr[i] = new double[ffann.NumPercept[i]];
                for(z = 0; z < ffann.NumPercept[i]; z++)
                {
                    dterr[i][z] = 0;
                }
            }
            //Fine azzeramento
            do
            {
                for (int i = 0; i < stdDataset.Length; i++)
                {
                    for (int j = ffann.NumPercept[ffann.NumLayers-1]-1; j >= 0 ; j--)
                    {
                        desired_output[j] = stdDataset[i][stdDataset[0].Length - 1 - j];
                    }
                    ffann.Predict(stdDataset[i]); //Calcolo uscite della rete dato il sample i.
                    for (int j = ffann.NumLayers-2; j >= 0 ; j--) //Scorro la rete dal penultimo layer al primo.
                    {
                        for (int k = 0; k < ffann.NumPercept[j]; k++) // Scorro i percettroni dello strato j.
                        {
                            for (int q = 0; q < ffann.NumPercept[j + 1]; q++) // Scorro le sinapsi.
                            {
                                error += ERR(desired_output, ffann, j);

                                ffann.layer[j].perceptron[k].setSynapsys(ffann.layer[j].perceptron[k].getSynapsys(q) + Delta(ffann, desired_output, j, k, q, dterr), q);
                            }                
                            
                        }
                    }
                }
            } while (++epochs != 6000);
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

