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
    /// Interaction logic for DetailCakeScreen.xaml
    /// </summary>
    public partial class DetailCakeScreen : Window
    {
        private string idCake;
        private Cake cake;
        private bool isEdit = false;
        private List<Category> categories;


        public DetailCakeScreen(string id)
        {
            InitializeComponent();
            idCake = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayDetail();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void imageUploadImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void removeImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayDetail()
        {
            cake = CakesDAO.GetById(idCake);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var pathImageAbsolute = $"{folder}\\Assets\\Images\\Uploads\\{cake.Image_Main}";

            imageUploadImage.Source = new BitmapImage(new Uri(pathImageAbsolute, UriKind.Absolute));

            categories = CategoriesDAO.GetCategories();

            categoryComboBox.ItemsSource = categories;

            //var categoryEntered = categories[categoryComboBox.SelectedIndex];
            categoryComboBox.SelectedIndex = categories.FindIndex(item => item.Id == cake.Category.Id);

            this.DataContext = cake;



        }
    }
}
