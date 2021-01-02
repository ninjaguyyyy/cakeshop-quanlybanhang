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
    /// Interaction logic for DetailCakeScreen.xaml
    /// </summary>
    public partial class DetailCakeScreen : Window
    {
        private string idCake;
        private Cake cake;
        private bool isEditing = false;
        private List<Category> categories;
        private string fileUploadPath = "mac-dinh.png";
        private string nameImage;

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
            if(isEditing)
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
        }

        private void removeImage_Click(object sender, RoutedEventArgs e)
        {
            nameImage = "default.jpg";
            imageUploadImage.Source = new BitmapImage(new Uri("../Assets/Images/bg_upload.png", UriKind.Relative));
            nameFileTextblock.Text = "mac-dinh.png";
        }


        private void DisplayDetail()
        {
            cake = CakesDAO.GetById(idCake);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var pathImageAbsolute = $"{folder}\\Assets\\Images\\Uploads\\{cake.Image_Main}";

            imageUploadImage.Source = new BitmapImage(new Uri(pathImageAbsolute, UriKind.Absolute));

            categories = CategoriesDAO.GetCategories();

            categoryComboBox.ItemsSource = categories;

            categoryComboBox.SelectedIndex = categories.FindIndex(item => item.Id == cake.Category.Id);

            imageUploadImage.Tag = cake.Image_Main;

            nameImage = cake.Image_Main;

            this.DataContext = cake;



        }

        private void editOrSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            isEditing = !isEditing;
            if (isEditing)
            {
                nameCakeTextbox.IsEnabled = true;
                removeImage.IsEnabled = true;
                priceTextbox.IsEnabled = true;
                categoryComboBox.IsEnabled = true;
                descriptionTextbox.IsEnabled = true;
                editOrSubmitButton.Content = "Xác nhận";
            }
            else
            {
                
                if (imageUploadImage.Tag.ToString() != cake.Image_Main)
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

                var newCake = new Cake();
                newCake.Id = cake.Id;
                newCake.Name = nameCakeTextbox.Text;
                newCake.Price = int.Parse(priceTextbox.Text);
                newCake.Description = descriptionTextbox.Text;
                newCake.Image_Main = nameImage;
                newCake.Category = categoryEntered;
                CakesDAO.RemoveCake(cake.Id);
                CakesDAO.InsertCake(newCake);

                nameCakeTextbox.IsEnabled = false;
                removeImage.IsEnabled = false;
                priceTextbox.IsEnabled = false;
                categoryComboBox.IsEnabled = false;
                descriptionTextbox.IsEnabled = false;
                editOrSubmitButton.Content = "Chỉnh sửa";
            }
            
        }
    }
}
