using CakeShop.DAO;
using CakeShop.DTO;
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
    /// Interaction logic for OrdersScreen.xaml
    /// </summary>
    public partial class OrdersScreen : Window
    {
        private Order orderToShow = new Order();
        private List<Order> fetchedOrders = new List<Order>();
        public OrdersScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayOrders();
        }

        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var addOrderScreen = new AddOrderScreen();
            if (addOrderScreen.ShowDialog() == true)
            {
                var newOrder = addOrderScreen.NewOrder;
                OrdersDAO.InsertOrder(newOrder);
            }
        }

        private void mainPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuToggleButton.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        private void listCakeListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listCakeScreen = new ListCakesScreen();
            listCakeScreen.Show();

            this.Close();
        }

        void DisplayOrders()
        {
            fetchedOrders = OrdersDAO.GetOrders();
            ordersListView.ItemsSource = fetchedOrders;

            orderToShow = fetchedOrders[0];
            DisplayOrderToShow();
        }

        void DisplayOrderToShow()
        {
            cakesChosenListView.ItemsSource = orderToShow.CartItems;
            this.DataContext = orderToShow;
        }

        private void orderListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var that = sender as Border;
            string idOrderToShow = that.Uid;
            orderToShow = fetchedOrders.Find(item => item.Id == idOrderToShow);
            DisplayOrderToShow();
        }
    }
}
