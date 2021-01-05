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
    /// Interaction logic for AddOrderScreen.xaml
    /// </summary>
    public partial class AddOrderScreen : Window
    {
        private List<CartItem> chosenCakes = new List<CartItem>();
        private Order newOrder = new Order();
        internal Order NewOrder { get => newOrder; set => newOrder = value; }
        public AddOrderScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayProducts();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameOrderTextbox.Text;
            string price = priceTextbox.Text;
            string description = descriptionTextbox.Text;

            if (name == "" || price == "")
            {
                MessageBox.Show("Chưa điền đầy đủ thông tin", "Lỗi");
                return;
            }
            NewOrder.Name = name;
            

            DialogResult = true;
        }

        private void cakeListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var that = sender as StackPanel;
            var id = that.Tag.ToString();
            var cake = CakesDAO.GetById(id);
            chosenCakes.Add(new CartItem()
            {
                Id = Guid.NewGuid().ToString(),
                Cake = cake,
                Quantity = 1,
                TotalPrice = cake.Price
            }) ;

            DisplayCartItems();
            DisplayProducts();
        }

        void DisplayProducts()
        {
            var fetchedCakes = CakesDAO.GetCake(100, 1, "0");
            cakesListView.ItemsSource = fetchedCakes;

            var nameGenerated = Guid.NewGuid().ToString();
            NewOrder.Id = nameGenerated;
            NewOrder.Name = nameGenerated;
            NewOrder.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            NewOrder.CartItems = chosenCakes;
            NewOrder.TotalPrice = chosenCakes.Sum(item => item.TotalPrice);
            priceTextbox.Text = NewOrder.TotalPrice.ToString() + " vnđ";

            this.DataContext = NewOrder;
        }
        void DisplayCartItems()
        {
            chosenCakeListView.ItemsSource = null;
            chosenCakeListView.ItemsSource = chosenCakes;
        }

        private void removeCakes_Click(object sender, RoutedEventArgs e)
        {
            chosenCakes.Clear();
            DisplayCartItems();
        }

        private void quantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var that = sender as TextBox;
            var obj = chosenCakes.FirstOrDefault(x => x.Id == that.Uid);
            if (obj != null && that.Text.ToString() != "")
            {
                obj.Quantity = int.Parse(that.Text.ToString());
                obj.TotalPrice = obj.Cake.Price * obj.Quantity;
            }

            DisplayProducts();
        }
    }
}
