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

namespace CakeShop.screens
{
    /// <summary>
    /// Interaction logic for ListCakesScreen.xaml
    /// </summary>
    public partial class ListCakesScreen : Window
    {
        public ListCakesScreen()
        {
            InitializeComponent();
        }

        private void mainPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuToggleButton.IsChecked = false;
        }

        private void menuToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            mainPanel.Opacity = 1;
        }

        private void menuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            mainPanel.Opacity = 0.3;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (menuToggleButton.IsChecked == true)
            {
                tt_cakes.Visibility = Visibility.Collapsed;
                tt_add.Visibility = Visibility.Collapsed;
                tt_orders.Visibility = Visibility.Collapsed;
                tt_report.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_cakes.Visibility = Visibility.Visible;
                tt_add.Visibility = Visibility.Visible;
                tt_orders.Visibility = Visibility.Visible;
                tt_report.Visibility = Visibility.Visible;
            }
        }

        private void addTripListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void modeSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
