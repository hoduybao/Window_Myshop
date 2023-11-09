using MyShop.Model;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        BindingList<Product> _products;

        public Home()
        {
            InitializeComponent();
          
        }

     
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as ProductOfOrder;
                if (product != null)
                {
                    product.SoLuong++;
                    // Cập nhật DataGrid
                    dgHoaDon.Items.Refresh();
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as ProductOfOrder;
                if (product != null && product.SoLuong > 0)
                {
                    product.SoLuong--;
                    // Cập nhật DataGrid
                    dgHoaDon.Items.Refresh();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<ProductOfOrder> productList = new List<ProductOfOrder>
{
    new ProductOfOrder { STT = 1, TenSanPham = "Sản phẩm 1", DonGia = 10.0M, SoLuong = 1 },
    new ProductOfOrder { STT = 2, TenSanPham = "Sản phẩm 2", DonGia = 15.0M, SoLuong = 2 },
    new ProductOfOrder { STT = 3, TenSanPham = "Sản phẩm 3", DonGia = 20.0M, SoLuong = 1 },
};
            dgHoaDon.ItemsSource = productList;
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





            //list product
            _products = new BindingList<Product>()
            {
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                               new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                                               new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},
                new Product() { Avatar = "../Images/Login/logo.png", TenSanPham="Iphone 15 Promax", DonGia=35000000,SoLuong=100},

            };

            productsComboBox.ItemsSource = _products;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Handle the selection change here
            if (comboBoxTypeProduct.SelectedItem != null)
            {
                string selectedOption = ((ComboBoxItem)comboBoxTypeProduct.SelectedItem).Content.ToString();
            }
        }

        private void ComboBox_SelectionChangedSort(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTypeProduct.SelectedItem != null)
            {
                string selectedOption = ((ComboBoxItem)sortCost.SelectedItem).Content.ToString();
            }
        }

        private void studentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
