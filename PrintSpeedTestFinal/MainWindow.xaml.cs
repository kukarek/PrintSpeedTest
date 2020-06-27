using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// </summary>\
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Таймер.
        /// </summary>
        DispatcherTimer timer { get; set; }

        /// <summary>
        /// Текущее время.
        /// </summary>
        DateTime timenow { get; set; }

        /// <summary>
        /// Время старта.
        /// </summary>
        DateTime timestart { get; set; }

        /// <summary>
        /// Исходный текст.
        /// </summary>
        TextOptions textoptions { get; set; }

        /// <summary>
        /// Начинает или завершает выполнение основной логики.
        /// </summary>
        Action action { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Grid1.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
         
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            SourceText.IsHitTestVisible = false;
            Typing.IsReadOnly = true;

            action = Start;
            textoptions = new TextOptions();
        }

        /// <summary>
        /// Выход из приложения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Перетаскивание формы за края.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //перетаскивание формы 
        {
            this.DragMove();
        }

        /// <summary>
        /// Старт выполнения вычислений.
        /// </summary>
        public void Start()
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

        /// <summary>
        /// Остановка вычислений.
        /// </summary>
        public void Stop()
        {

            Typing.IsReadOnly = true;
            timer.Stop();
            action = Start;
            

            if (Typing.Text != "") 
            {
                Tick();
                Results results = new Results(Assay.SpeedValues, Assay.AccuracyValues);
                results.ShowDialog();
            }

            Clean();

            action();
        }

        /// <summary>
        /// Работа таймера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timerTick(object sender,EventArgs e)
        {
            Tick();
        }

        /// <summary>
        /// Выполнение вычислений.
        /// </summary>
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
        
     
        /// <summary>
        /// Проверка правильности печати.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Определение горячей клавиши.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Typing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                action();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            action();
        }

        /// <summary>
        /// Очистка всех полей и объктов.
        /// </summary>
        public void Clean()
        {
            Typing.Text = null;
            SourceText.Text = null;
            Exactness.Content = "0 %";
            Speed.Content = "0 зн/мин";

            Assay.Accuracy = null;
            Assay.PrintSpeed = null;
            Assay.Characters = 0;
            Assay.mistakes = 0;
            Assay.AccuracyValues = new List<double>();
            Assay.SpeedValues = new List<double>();
        }
    }
}
