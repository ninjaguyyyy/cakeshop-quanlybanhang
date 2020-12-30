using CakeShop.DAO;
using CakeShop.DTO;
using Microsoft.Win32;
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
    /// Interaction logic for AddCakeScreen.xaml
    /// </summary>
    public partial class AddCakeScreen : Window
    {
        private Cake newCake = new Cake();
        private string fileUploadPath = "mac-dinh.png";
        private List<Category> categories;

        internal Cake NewCake { get => newCake; set => newCake = value; }

        public AddCakeScreen()
        {
            InitializeComponent();
        }   

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameCakeTextbox.Text;
            string price = priceTextbox.Text;
            string description = descriptionTextbox.Text;

            if (name == "" || price == "")
            {
                MessageBox.Show("Chưa điền đầy đủ thông tin", "Lỗi");
                return;
            }

            var nameImage = "default.jpg";

            if (imageUploadImage.Tag.ToString() != "default.jpg")
            {
                nameImage = imageUploadImage.Tag.ToString();

                // Copy file
                var folder = AppDomain.CurrentDomain.BaseDirectory;

                string newNameFile = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(nameImage);
                var targetPath = $"{folder}\\Assets\\Images\\Uploads\\";
                var destFile = System.IO.Path.Combine(targetPath, newNameFile);


                System.IO.File.Copy(fileUploadPath, destFile, true);

                // --
                nameImage = newNameFile;
            }

            var categoryEntered = categories[categoryComboBox.SelectedIndex];

            if (NewCake.Id == null)
            {
                NewCake.Id = Guid.NewGuid().ToString();
            }
            NewCake.Name = name;
            NewCake.Price = int.Parse(price);
            NewCake.Description = description;
            NewCake.Image_Main = nameImage;
            NewCake.Category = categoryEntered;

            DialogResult = true;
        }

        private void imageUploadImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh bìa cho bánh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            op.Multiselect = false;
            var o = op.ShowDialog();
            if (o == true)
            {
                fileUploadPath = op.FileName;
                imageUploadImage.Source = new BitmapImage(new Uri(op.FileName));
                imageUploadImage.Tag = op.SafeFileName;

                nameFileTextblock.Text = op.SafeFileName;
            }
        }

        private void removeImage_Click(object sender, RoutedEventArgs e)
        {
            imageUploadImage.Source = new BitmapImage(new Uri("../Assets/Images/bg_upload.png", UriKind.Relative));
            nameFileTextblock.Text = "mac_dinh.png";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            categories = CategoriesDAO.GetCategories();

            categoryComboBox.ItemsSource = categories;
        }
    }
}
