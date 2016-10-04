using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    //Questa classe astratta contiene tutti i metodi corrispondenti alle funzioni di attivazione principali,
    //Semplici formule matematiche

    class Activation
    {
        public double LogisticSigmoid(double x)
        {
            return (1 / (1 + Math.Pow(Math.E, -x)));
        }

        public double HiperTan(double x)
        {
            return ((Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x)));
        }

        public bool HeivisideStep(double x)
        {
            return (x >= 0);
        }

        //ritorna il vettore probabilità
        public void Softmax(Layer layer)
        {
            double TotalDivisor = 0;
            for (int i = 0; i < layer.perceptron.Length; i++)
                TotalDivisor += Math.Pow(Math.E, layer.perceptron[i].getAction());
            for (int i = 0; i < layer.perceptron.Length; i++)
                layer.perceptron[i].setAction(Math.Pow(Math.E, layer.perceptron[i].getAction()) / TotalDivisor);
        }
    }
}
