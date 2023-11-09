using MyShop.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ListProduct.xaml
    /// </summary>
    public partial class ListProduct : Page
    {
        BindingList<Product> _products;

        public ListProduct()
        {
            InitializeComponent();
            comboBoxTypeProduct.Items.Add(new ComboBoxItem { Content = "Tất cả" });

            comboBoxTypeProduct.Items.Add(new ComboBoxItem { Content = "Iphone" });
            comboBoxTypeProduct.Items.Add(new ComboBoxItem { Content = "Samsung" });
            comboBoxTypeProduct.Items.Add(new ComboBoxItem { Content = "Oppo" });
            ComboBoxItem itemToSelect = (ComboBoxItem)comboBoxTypeProduct.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            itemToSelect.IsSelected = true;

            sortCost.Items.Add(new ComboBoxItem { Content = "Tăng dần" });

            sortCost.Items.Add(new ComboBoxItem { Content = "Giảm dần" });

            ComboBoxItem itemSortToSelect = (ComboBoxItem)sortCost.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            itemSortToSelect.IsSelected = true;
            _products = new BindingList<Product>()
            {
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                               new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                                               new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},

            };

            productsComboBox.ItemsSource = _products;
        }

        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void studentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChangedSort(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnImportProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnListTypeProduct_Click(object sender, RoutedEventArgs e)
        {
            var page = new ProductTypePage();
            NavigationService.Navigate(page);
        }
    }
}
