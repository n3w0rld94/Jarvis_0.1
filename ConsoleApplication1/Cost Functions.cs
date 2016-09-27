using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
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
}
