using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        /// <summary>
        /// Значения для графика.
        /// </summary>
        public ICollection<double> collection_1 { get; set; }

        /// <summary>
        /// Значения для графика.
        /// </summary>
        public ICollection<double> collection_2 { get; set; }

        /// <summary>
        /// Окно графика.
        /// </summary>
        public Chart chartwindow;
        public Results(ICollection<double> collection1, ICollection<double> collection2)
        {
            InitializeComponent();
            Grid1.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Speed.Content = Assay.PrintSpeed;
            Accuracy.Content = Assay.Accuracy;
            Rating.Content = Assay.Rating();

            collection_1 = collection1;
            collection_2 = collection2;
        }

        /// <summary>
        /// Закрытие окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Перетаскиване формы мышкой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //перетаскивание формы 
        {
            this.DragMove();
        }

        /// <summary>
        /// Вызов окна графика.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Click(object sender, RoutedEventArgs e)
        {
            chartwindow = new Chart(collection_1, collection_2);
            chartwindow.ShowDialog();
        }
    }
}
