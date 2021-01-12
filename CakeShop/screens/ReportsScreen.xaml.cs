using CakeShop.DAO;
using CakeShop.DTO;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for ReportsScreen.xaml
    /// </summary>
    public partial class ReportsScreen : Window
    {

        private List<Order> orders = new List<Order>();
        public ReportsScreen()
        {
            InitializeComponent();
            PointLabel = chartPoint =>
                string.Format("{0}", chartPoint.Y);

            
            
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            orders = OrdersDAO.GetOrders();
            DisplayRevenueByCategory();
            DisplayRevenueByMonth();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        private void listCakeListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listCakesScreen = new ListCakesScreen();
            listCakesScreen.Show();

            this.Close();
        }

        private void DisplayRevenueByCategory()
        {
            SeriesCollection series = new SeriesCollection();
            List<Category> categories = CategoriesDAO.GetCategories();
            categories = categories.Skip(1).ToList();

            List<CartItem> cartItems = new List<CartItem>();
            foreach (var order in orders)
            {
                cartItems.AddRange(order.CartItems);
            }

            
            foreach (var category in categories)
            {
                long value = 0;
                
                foreach(var cartItem in cartItems)
                {
                    if(cartItem.Cake.Category.Id == category.Id)
                    {
                        value += cartItem.TotalPrice;
                    }
                }
                
                PieSeries pieSeries = new PieSeries
                {
                    Title = category.Name,
                    Values = new ChartValues<long> { value },
                    DataLabels = true,
                    LabelPoint = PointLabel
                };
                series.Add(pieSeries);
            }
            
            revenueByCategoryPie.Series = series;
        }

        private void DisplayRevenueByMonth()
        {
            SeriesCollection series = new SeriesCollection();
            List<string> months = new List<string>()
            {
                "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
            };

            


            foreach (var month in months)
            {
                long value = 0;

                foreach (var order in orders)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var date = DateTime.ParseExact(order.CreatedDate, "dd/MM/yyyy", provider);
                    var monthName = date.ToString("MMMM");
                    if(monthName == month)
                    {
                        value += order.TotalPrice;
                    }
                }

                ColumnSeries columnSeries = new ColumnSeries
                {
                    Title = month,
                    Values = new ChartValues<long> { value }
                };
                series.Add(columnSeries);
            }
            Labels = months.ToArray();
            revueneByMonthChart.Series = series;

        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (menuToggleButton.IsChecked == true)
            {
                tt_cakes.Visibility = Visibility.Collapsed;
                tt_orders.Visibility = Visibility.Collapsed;
                tt_report.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_cakes.Visibility = Visibility.Visible;
                tt_orders.Visibility = Visibility.Visible;
                tt_report.Visibility = Visibility.Visible;
            }
        }

        private void orderMenu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var orderScreen = new OrdersScreen();
            orderScreen.Show();

            this.Close();
        }
    }
}
