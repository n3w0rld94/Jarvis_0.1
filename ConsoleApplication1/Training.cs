using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
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
        const double LearnRatePlus = 1.2, LearnRateMinus = 0.5, Delta = 0;

        public RPropPlus(FFANN ffann) : base(ffann)
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

        public Genetic(FFANN ffann) : base(ffann)
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

        public Particle_swarm_optimization(FFANN ffann) : base(ffann)
        {
            Random Rand = new Random();
            Cohesion = Rand.NextDouble();
            Separation = Rand.NextDouble();

        }
    }
}

