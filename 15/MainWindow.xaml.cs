using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Configuration;
//Sql staff
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace _15
{

    ///<summary>
    ///базу даних
    ///інсталяція
    ///розділ «Допомога»

    

    public partial class MainWindow : Window
    {
        //DB
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //DataDisplayded dataDisplayded;
        DataTable dataTable { get; set; }
        //DataGrid DG = new DataGrid();




        Game myGame;
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        bool currentTime1 = true;
        bool defaultImageFlag = true;
        string defaultImagePath = "DefaultImg3.jpg";

        private Image[] imageArray = new Image[17];
        
        DateTime StartTime;
        int moves = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            myGame = new Game(4);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                clocktxtblock.Text = currentTime;
            }
        }
        private void button0_Click_1(object sender, RoutedEventArgs e)
        {
           

            int temp = Convert.ToInt16(((Button)sender).Tag);

           // MessageBox.Show(temp.ToString(), "Была нажата кнопка с Tag");
            myGame.swapTails(temp);
            //myButton(temp).Focus();
            RefreshTiles();
           // myButton(temp).Focus();

            //MessageBox.Show(myGame.checkToEndGame().ToString());
            moves++;

            ScoreLable.Text = moves.ToString();

            if (myGame.checkToEndGame())
            {
                sw.Stop();
                WinWindow koko = new WinWindow();
                koko.Show();
                MessageBox.Show("You Win!", $"15Puzzle and make {moves} your time: {clocktxtblock.Text} ");
               //TimeSpan endOfGame = DateTime.Now - StartTime;
                

            }
            //MessageBox.Show(nmb.ToString());
        }
        private Button myButton(int pos)
        {
            switch (pos)
            {
                case 0: return button0;
                case 1: return button1;
                case 2: return button2;
                case 3: return button3;
                case 4: return button4;
                case 5: return button5;
                case 6: return button6;
                case 7: return button7;
                case 8: return button8;
                case 9: return button9;
                case 10: return button10;
                case 11: return button11;
                case 12: return button12;
                case 13: return button13;
                case 14: return button14;
                case 15: return button15;
                default: return null;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
            moves = 0;
            sw.Reset();
            clocktxtblock.Text = "00:00:00";
            PauseButton.Header = "PAUSE";
            sw.Start();
            dt.Start();
            ScoreLable.Text = moves.ToString();
        }

        private void StartGame()
        {
            if (defaultImageFlag)
            {
                CutImage(defaultImagePath);
            }
            //myButton(0).Content = imageArray[0];
            myGame.start();
            myGame.mixTails();
            RefreshTiles();

            ///<summary>Start timer</summary>
            StartTime = DateTime.Now;
        }

        private void RefreshTiles()
        {
            for (int p = 0; p < 16; p++)
            {
                int nmb = myGame.get_number(p);
                
                // MessageBox.Show(nmb.ToString());
                // myButton(p).Content = nmb.ToString();
                //myButton(p).DataContext= nmb.ToString();
                //if (myButton(p).Tag==1) { myButton(p).Content = imageArray[nmb]; }
  
                myButton(p).Content = imageArray[nmb];
                if (nmb > 0)
                {
                    myButton(p).Visibility = Visibility.Visible;
                }

                else

                    myButton(p).Visibility = Visibility.Hidden;
            }
            if (!sw.IsRunning)
            {
                sw.Start();
                currentTime1 = true;
                PauseButton.Header = "PAUSE";
            }
        }

        private void Game15_Load(object sender, EventArgs e)
        {
            StartGame();
        }

       

        //private void CutImage(string img)
        //{
        //    int count = 0;

        //    BitmapImage src = new BitmapImage();
        //    src.BeginInit();
        //    src.UriSource = new Uri(img, UriKind.Relative);
        //    src.CacheOption = BitmapCacheOption.OnLoad;
        //    src.EndInit();
        //    int size=480;
        //    if (src.PixelWidth < src.PixelHeight)
        //    {
        //        size = src.PixelWidth;
        //    }
        //    else if (src.PixelWidth >= src.PixelHeight)
        //    {
        //        size = src.PixelHeight;
        //    }
        //    int sizecrop = size / 4;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < 4; j++)
        //        {
        //            imageArray[count] = new Image();
        //            imageArray[count].Source = new CroppedBitmap(src, new Int32Rect(j * sizecrop, i * sizecrop, sizecrop, sizecrop));
        //            myButton(count).Content = imageArray[count++];

        //        }
        //    }
        //    defaultImageFlag = false;
        //}
        private void CutImage(string img)
        {
            int count = 1;
            imageArray[0] = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(img, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            int size = 480;
            if (src.PixelWidth < src.PixelHeight)
            {
                size = src.PixelWidth;
            }
            else if (src.PixelWidth >= src.PixelHeight)
            {
                size = src.PixelHeight;
            }
            int sizecrop = size / 4;
            //Image[] imageArray = new Image[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (count == 16) continue;
                    imageArray[count] = new Image();
                    imageArray[count].Source = new CroppedBitmap(src, new Int32Rect(j * sizecrop, i * sizecrop, sizecrop, sizecrop));
                    myButton(count).Content = imageArray[count++];

                }
            }
            defaultImageFlag = false;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
        }

        private void HelpBTN_Click(object sender, RoutedEventArgs e)
        {
            
        }

       
        private void open_Click_1(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\Users\\Lenovo\\Pictures\\AppImages\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|PNG Photos (*.png)|*.jpg";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != null)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            CutImage(openFileDialog1.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            StartGame();
            moves = 0;
            sw.Reset();
            clocktxtblock.Text = "00:00:00";
            PauseButton.Header = "PAUSE";
            sw.Start();
            dt.Start();
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

            if (sw.IsRunning)
            {
                sw.Stop();
                currentTime1 = false;
                PauseButton.Header = "RESUME";
            }
            else
            {
                sw.Start();
                currentTime1 = true;
                PauseButton.Header = "PAUSE";
            }

            

            //elapsedtimeitem.Items.Add(currentTime);
        }
    }
}
