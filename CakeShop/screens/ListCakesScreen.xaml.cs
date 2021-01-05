using CakeShop.DAO;
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
        private string categoryFilter;
        class PageInfo
        {
            public int Page { get; set; }
            public int TotalPages { get; set; }
        }

        class Paging
        {
            private int _totalPages;
            public int CurrentPage { get; set; }

            public int RowsPerPage { get; set; }

            public int TotalPages
            {
                get => _totalPages; set
                {
                    _totalPages = value;
                    Pages = new List<PageInfo>();
                    for (int i = 1; i <= _totalPages; i++)
                    {
                        Pages.Add(new PageInfo()
                        {
                            Page = i,
                            TotalPages = _totalPages
                        });
                    }
                }
            }

            public List<PageInfo> Pages { get; set; }

        }

        private Paging cakesPaging;
        private const int ROW_PER_PAGE = 12;

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

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
        private void addCakeButton_Click(object sender, RoutedEventArgs e)
        {
            var AddCakeScreen = new AddCakeScreen();
            if (AddCakeScreen.ShowDialog() == true)
            {
                var newCake = AddCakeScreen.NewCake;
                // handle with new cake
                CakesDAO.InsertCake(newCake);
                DisplayProducts();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var categories = CategoriesDAO.GetCategories();

            categoryComboBox.ItemsSource = categories;
            categoryComboBox.SelectedIndex = 0;
            categoryFilter = categories[0].Id;

            HandlePagingInfoForCakes();
            DisplayProducts();
        }

        void DisplayProducts()
        {
            var fetchedCakes = CakesDAO.GetCake(cakesPaging.RowsPerPage, cakesPaging.CurrentPage, categoryFilter);
            cakesListView.ItemsSource = fetchedCakes;
        }

        void HandlePagingInfoForCakes()
        {
            var count = CakesDAO.CountCakes();

            cakesPaging = new Paging
            {
                CurrentPage = 1,
                RowsPerPage = ROW_PER_PAGE,
                TotalPages = count / ROW_PER_PAGE +
                    (((count % ROW_PER_PAGE) == 0) ? 0 : 1)
            };

            pagingListView.Items.Clear();

            var prevListViewItem = new ListViewItem();
            prevListViewItem.Content = "⏪  prev";
            Thickness paddingForNext = prevListViewItem.Padding;
            paddingForNext.Left = 20;
            paddingForNext.Right = 13;
            paddingForNext.Top = 8;
            paddingForNext.Bottom = 8;
            prevListViewItem.Padding = paddingForNext;
            prevListViewItem.Focusable = false;
            prevListViewItem.Foreground = Brushes.White;
            prevListViewItem.PreviewMouseLeftButtonUp += prevLVItem_PreviewMouseLeftButtonUp;
            pagingListView.Items.Add(prevListViewItem);

            foreach (var page in cakesPaging.Pages)
            {

                var pageListViewItem = new ListViewItem();
                pageListViewItem.Foreground = Brushes.White;
                Thickness padding = pageListViewItem.Padding;
                padding.Left = 13;
                padding.Right = 13;
                padding.Top = 8;
                padding.Bottom = 8;
                pageListViewItem.Padding = padding;

                pageListViewItem.Content = page.Page;
                pageListViewItem.PreviewMouseLeftButtonUp += HandlePageClicked;

                if (page.Page == 1)
                {
                    pageListViewItem.IsSelected = true;
                }

                pagingListView.Items.Add(pageListViewItem);
            }

            var nextListViewItem = new ListViewItem();
            nextListViewItem.Content = "next  ⏩";
            Thickness paddingForPrev = nextListViewItem.Padding;
            paddingForPrev.Left = 13;
            paddingForPrev.Right = 20;
            paddingForPrev.Top = 8;
            paddingForPrev.Bottom = 8;
            nextListViewItem.Padding = paddingForPrev;
            nextListViewItem.Focusable = false;
            nextListViewItem.Foreground = Brushes.White;
            nextListViewItem.PreviewMouseLeftButtonUp += nextLVItem_PreviewMouseLeftButtonUp;
            pagingListView.Items.Add(nextListViewItem);


        }

        private void nextLVItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cakesPaging.CurrentPage < cakesPaging.TotalPages)
            {
                cakesPaging.CurrentPage++;
                foreach (dynamic item in pagingListView.Items)
                {
                    if (item.Content.ToString() == (cakesPaging.CurrentPage - 1).ToString())
                    {
                        item.IsSelected = false;
                    }
                    if (item.Content.ToString() == cakesPaging.CurrentPage.ToString())
                    {
                        item.IsSelected = true;
                    }
                }
            }
            DisplayProducts();
        }

        private void HandlePageClicked(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            cakesPaging.CurrentPage = int.Parse(item.Content.ToString());

            DisplayProducts();
        }

        private void prevLVItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cakesPaging.CurrentPage > 1)
            {
                cakesPaging.CurrentPage--;
                foreach (dynamic item in pagingListView.Items)
                {
                    if (item.Content.ToString() == (cakesPaging.CurrentPage + 1).ToString())
                    {
                        item.IsSelected = false;
                    }
                    if (item.Content.ToString() == cakesPaging.CurrentPage.ToString())
                    {
                        item.IsSelected = true;
                    }
                }
            }
            DisplayProducts();
        }

        private void cakeListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var that = sender as StackPanel;
            var id = that.Tag;
            var detailTripScreen = new DetailCakeScreen(id.ToString());
            detailTripScreen.ShowDialog();
            DisplayProducts();
        }

        private void orderListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ordersScreen = new OrdersScreen();
            ordersScreen.Show();

            this.Close();
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var that = sender as ComboBox;
            categoryFilter = (that.SelectedIndex).ToString();
            HandlePagingInfoForCakes();
            DisplayProducts();
        }
    }
}
