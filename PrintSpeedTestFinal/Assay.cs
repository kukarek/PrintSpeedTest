using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Основная логика приложения.
    /// </summary>
    static class  Assay 
     {
        /// <summary>
        /// Список значений скорости для построения графика.
        /// </summary>
        public static List<double> SpeedValues = new List<double>();

        /// <summary>
        /// Список значений точности для построения графика.
        /// </summary>
        public static List<double> AccuracyValues = new List<double>();

        /// <summary>
        /// Количество ошибок.
        /// </summary>
        public static int mistakes { get; set; }

        /// <summary>
        /// Скорость печати.
        /// </summary>
        public static string PrintSpeed { get; set; }

        /// <summary>
        /// Точность печати.
        /// </summary>
        public static string Accuracy { get; set; }

        /// <summary>
        /// Количество напечатанных символов.
        /// </summary>
        public static int Characters { get; set; }

        /// <summary>
        /// Вычисление параметров.
        /// </summary>
        /// <param name="minutes"></param>
        public static void Compute(double minutes)
        {
            PrintSpeed = SpeedCount(minutes);
            Accuracy = AccuracyCount();
            if (double.Parse(Accuracy.Substring(0, Accuracy.Length - 2)) <= 0) // костыль)
            {
                Accuracy = "0 %";
            }
        }

        /// <summary>
        /// Вычисление скорости печати.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private static string SpeedCount(double minutes)
        {
            if (minutes != 0)
            {
                return Convert.ToString(Math.Round(Characters / minutes, 0)) + " зн/мин";
            }
            else
                return "0 зн/мин";
        }

        /// <summary>
        /// Вычисление точности печати.
        /// </summary>
        /// <returns></returns>
        private static string AccuracyCount()
        {
            if (Characters != 0)
            {
                return Convert.ToString(Math.Round(((double)Characters - (double)mistakes) / (double)Characters * 100, 1)) + " %"; // вычисление точности
               
            }
            else
                return "0 %";
        }
        

        /// <summary>
        /// Вычисление оценки результата.
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Запись значений для графика.
        /// </summary>
        public static void WritingValuesForTheChart()
        {
            double SpeedValue = double.Parse(PrintSpeed.Replace(" зн/мин", ""));
            SpeedValues.Add(SpeedValue);
            double AccuracyValue = double.Parse(Accuracy.Replace(" %", ""));
            AccuracyValues.Add(AccuracyValue);
        }
    }
}
