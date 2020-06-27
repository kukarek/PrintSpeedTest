using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Window
    {
        public SeriesCollection SeriesCollection { get; set; }

        /// <summary>
        /// Значения на оси Y
        /// </summary>
        public Func<double, string> YFormatter { get; set; }

        /// <summary>
        /// Значения на оси X
        /// </summary>
        public Func<double, string> XFormatter { get; set; }

        public Chart(ICollection<double> collect1, ICollection<double> collect2)
        {
            InitializeComponent();

            collect1 = Normalization(collect1);
            collect2 = Normalization(collect2);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Скорость печати",
                    Values = new ChartValues<double>(collect1)
                },
                new LineSeries
                {
                    Title = "Точность печати",
                    Values = new ChartValues<double>(collect2)
                }
            };

            YFormatter = value => value.ToString();
            XFormatter = value => value.ToString();

            DataContext = this;
        }

        /// <summary>
        /// Нормализация количества значений.
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        ICollection<double> Normalization(ICollection<double> collect)
        {
            if (collect.Count < 100)
            {
                return collect;
            }
            else
            {
                double k = Math.Round((double)(collect.Count / 50),0); // определяем во сколько раз надо сжать чтобы 
                                                                       // было не меньше 50 значений
               
                if(collect.Count % k == 0)  // проверяем возможность сжатия (проверяем остаток)
                {
                    return Compression(collect, k);
                }
                else  // если есть остаток и нет возможности ровно разделить удаляет элементы
                {
                    Random random = new Random();

                    while(collect.Count % k != 0)
                    {
                        collect.Remove(collect.ElementAt(random.Next(0, collect.Count - 1)));
                    }

                    return Compression(collect, k);
                }
            }
        }

        /// <summary>
        /// Сжатие коллекции значений.
        /// </summary>
        /// <param name="collect">Сжимаемая коллекция</param>
        /// <param name="k">Коэффицент сжатия</param>
        /// <returns></returns>
        ICollection<double> Compression(ICollection<double> collect, double k)
        {
            List<double> ConciseCollection = new List<double>();

            double element = 0;
            for (int a = 1; a != collect.Count; a++)
            {
                element = element + collect.ElementAt<double>(a - 1);
               
                if(a % k == 0)
                {
                    ConciseCollection.Add(element / k);
                    element = 0;
                }
            }

            return ConciseCollection;
        }
    }
}
