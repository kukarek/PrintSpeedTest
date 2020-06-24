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

namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public Results()
        {
            InitializeComponent();
            Grid1.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Speed.Content = Assay.PrintSpeed;
            Accuracy.Content = Assay.Accuracy;
            Rating.Content = Assay.Rating();



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //перетаскивание формы 
        {
            this.DragMove();
        }
    }
}
