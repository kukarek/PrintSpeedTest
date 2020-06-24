using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Window
    {
        public SeriesCollection series { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Chart()
        {
            InitializeComponent();

            series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                 
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 3, 8, 6, 3 ,7 }
                }
            };

            Labels = new[] { "a","b","c","d","e"};
            YFormatter = value => value.ToString("C");

            DataContext = this;
        }
    }
}
