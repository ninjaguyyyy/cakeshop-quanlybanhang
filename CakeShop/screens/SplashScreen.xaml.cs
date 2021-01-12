using CakeShop.DAO;
using CakeShop.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace CakeShop.screens
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private List<SplashData> splashData = new List<SplashData>();
        private System.Timers.Timer timer;
        private int count = 0;
        private int target = 5;

        public SplashScreen()
        {
            InitializeComponent();
        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            splashData = SplashDataDAO.GetAll();

            Random _rng = new Random();
            int indexRandom = _rng.Next(splashData.Count);

            nameCakeTextBlock.Text = splashData[indexRandom].Name;
            IntroAccessText.Text = splashData[indexRandom].Description;


            var isShowSplash = bool.Parse(ConfigurationManager.AppSettings["ShowSplashScreen"]);

            if (isShowSplash == false)
            {
                var listCakeScreen = new ListCakesScreen();
                listCakeScreen.Show();

                this.Close();
            }
            else
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += Timer_Elapsed;
                timer.Interval = 1000;
                timer.Start();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            count++;
            if (count == target)
            {
                timer.Stop();
                Dispatcher.Invoke(() =>
                {
                    var listCakeScreen = new ListCakesScreen();
                    listCakeScreen.Show();

                    this.Close();
                });
            }

            Dispatcher.Invoke(() =>
            {
                splashProgress.Value = count;
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
            config.Save(ConfigurationSaveMode.Minimal);
        }

        private void saveShowCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["ShowSplashScreen"].Value = "true";
            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
