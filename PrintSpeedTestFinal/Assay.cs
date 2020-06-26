using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintSpeedTestFinal
{
    static class  Assay 
     {
        public static List<double> SpeedValues = new List<double>();
        public static List<double> AccuracyValues = new List<double>();

        public static int mistakes { get; set; }
        public static string PrintSpeed { get; set; }
        public static string Accuracy { get; set; }
        public static int Characters { get; set; }

        public static void Compute(double minutes)
        {
         
            if (minutes != 0)
                PrintSpeed = Convert.ToString(Math.Round(Characters / minutes, 0)) + " зн/мин"; //вычисление скорости

            if (Characters != 0)
            {
                Accuracy = Convert.ToString(Math.Round(((double)Characters - (double)mistakes) / (double)Characters * 100, 1)) + " %"; // вычисление точности
                if (double.Parse(Accuracy.Substring(0, Accuracy.Length - 2)) <= 0) // костыль)
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
        
        public static void WritingValuesForTheChart()
        {
            double SpeedValue = double.Parse(PrintSpeed.Replace(" зн/мин", ""));
            SpeedValues.Add(SpeedValue);
            double AccuracyValue = double.Parse(Accuracy.Replace(" %", ""));
            AccuracyValues.Add(AccuracyValue);
        }




    }
}
