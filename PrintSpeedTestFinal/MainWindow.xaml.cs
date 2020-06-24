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
          
            Action.Content = "Завершить";
            timer.Start();
            timestart = DateTime.Now;

            action = Stop;
            Chart c = new Chart();
            c.Show();
        }
        public void Stop()
        {

            Typing.IsReadOnly = true;
            Action.Content = "Начать";
            timer.Stop();
            action = Do;
            

            if (Typing.Text != "") 
            {
                Tick();
                Results notice = new Results();
                notice.ShowDialog();
            }

            Clean();
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

            Speed.Content = Assay.PrintSpeed;
            Exactness.Content = Assay.Accuracy;
        }
        
     


        private void Action_Click(object sender, RoutedEventArgs e)
        {
             action();
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

        }

    }
}
