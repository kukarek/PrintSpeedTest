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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;




namespace PrintSpeedTestFinal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    //
    //
    // Нужно: 
    // 1) Подсчет скорости печати и ошибок(исправленных тоже) в реальном времени(точность в %) (сделано)
    // 2) Подсчет скорости печати и ошибок после остановки (сделано)
    // 3) Подсвечивание ошибок красным (сделано)
    // 4) По enter старт/завершения печати(сделано)
    // 5) Начало/конец при начале/конце печати в текстбокс (сделано)


    //  осталось добавить текст в папку


    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public static bool AppGeneral { get; set; }

        DateTime timenow, timestart;
        delegate void Delegate();
        Delegate action;
        TextOptions textoptions;

        public MainWindow()
        {
            InitializeComponent();
            Grid1.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
      
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            SourceText.IsHitTestVisible = false;
            Typing.IsReadOnly = true;

            action = Do;
            textoptions = new TextOptions();

            AppGeneral = false;
        }

      
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //перетаскивание формы 
        {
            this.DragMove();
        }

        public void Do()
        {
            Typing.IsReadOnly = false;
            Typing.Focus(); 
            DialogWindow notice = new DialogWindow();
            notice.ShowDialog();

            SourceText.Text = textoptions.GetText();
         
            timer.Start();
            timestart = DateTime.Now;

            action = Stop;
        }
        public void Stop()
        {

            Typing.IsReadOnly = true;
            timer.Stop();
            action = Do;
            

            if (Typing.Text != "") 
            {
                Tick();
                Results results = new Results(Assay.SpeedValues, Assay.AccuracyValues);
                results.ShowDialog();
            }

            Clean();

            action();
        }


        public void timerTick(object sender,EventArgs e)
        {
            Tick();
        }

        public void Tick()
        {
            timenow = DateTime.Now;
            TimeSpan ts = timenow - timestart;
            double minutes = (double)(ts.Minutes + ts.Hours * 60 + Convert.ToDouble(ts.Seconds) / 60 + Convert.ToDouble(ts.Milliseconds) / 60000);
            Assay.Characters = Typing.Text.Length;
            Assay.Compute(minutes);

            Assay.WritingValuesForTheChart();

            Speed.Content = Assay.PrintSpeed; // обновление в окне 
            Exactness.Content = Assay.Accuracy;
        }
        
     

        private void Typing_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (Typing.Text != "")
            {

                if (Typing.Text != SourceText.Text.Substring(0, Typing.Text.Length))
                {
                    Typing.Background = Brushes.IndianRed;

                      if (Typing.Text[Typing.Text.Length - 1] != SourceText.Text[Typing.Text.Length - 1])
                        Assay.mistakes++;
                }
                else
                    Typing.Background = Brushes.White;
             
               
                
                if (Typing.Text.Length == SourceText.Text.Length)
                {
                    Stop();
                    return;
                }
            }
            else
                Typing.Background = Brushes.White;
        }

        private void Typing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                action();
        }

       

   
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            action();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Results.chartwindow != null)
            {
                AppGeneral = true;
                Results.chartwindow.Close();
            }
        }

        public void Clean()
        {
            Typing.Text = null;
            SourceText.Text = null;
            Exactness.Content = "0 %";
            Speed.Content = "0 зн/мин";

            Assay.Accuracy = null;
            Assay.Characters = 0;
            Assay.mistakes = 0;
            Assay.PrintSpeed = null;
            Assay.AccuracyValues = new List<double>();
            Assay.SpeedValues = new List<double>();
        }
    }
}
