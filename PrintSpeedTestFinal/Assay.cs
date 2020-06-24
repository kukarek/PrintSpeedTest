using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PrintSpeedTestFinal
{
    static class  Assay 
     {
      
        public static int mistakes;
        public static string PrintSpeed;
        public static string Accuracy;
        public static int Characters;

        public static void Compute(double minutes)
        {
         
            if (minutes != 0)
                PrintSpeed = Convert.ToString(Math.Round(Characters / minutes, 0)) + " зн/мин";

            if (Characters != 0)
            {
                Accuracy = Convert.ToString(Math.Round(((double)Characters - (double)mistakes) / (double)Characters * 100, 1)) + " %";
                if (double.Parse(Accuracy.Substring(0, Accuracy.Length - 2)) <= 0)
                {
                    Accuracy = "0 %";
                }
            }
            else
                Accuracy = "0 %";
        }

        public static string Rating()
        {
            double coefficient = double.Parse(PrintSpeed.Substring(0, PrintSpeed.Length - 7)) * double.Parse(Accuracy.Substring(0,Accuracy.Length - 2)) / 100;

            if (coefficient < 80)
                return "Попробуйте еще раз.";
            else if (coefficient >= 80 & coefficient < 200)
                return "Неплохо!";
            else if (coefficient >= 200)
                return "Потрясающе!";
            else
                return "";
        }
        


    }
}
