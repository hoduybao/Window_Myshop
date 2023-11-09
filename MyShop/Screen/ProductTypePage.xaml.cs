using MyShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProductType.xaml
    /// </summary>
    public partial class ProductTypePage : Page
    {
        ObservableCollection<ProductType> productTypes=new ObservableCollection<ProductType>();

        public ProductTypePage()
        {
            InitializeComponent();
            productTypes.Add(new ProductType { Name="Iphone",Id="MB001",NumOfProduct=10,Description="Điện thoại đẹp, thông minh"});
            productTypes.Add(new ProductType { Name = "Samsung", Id = "MB002", NumOfProduct = 4, Description = "Điện thoại bền, camera xịn" });
            productTypes.Add(new ProductType { Name = "Oppo", Id = "MB003", NumOfProduct = 8, Description = "Chụp ảnh xịn, giá rẻ" });
           
            listProductType.ItemsSource = productTypes;
        }



        private void BtnAddProductType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdateProductType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveProductType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnImportProductType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listProductType.SelectedItem == null) return;
            ProductType productType = listProductType.SelectedItem as ProductType;

   

            // Bật 2 button Sửa và Xóa
            btnUpdateProductType.IsEnabled = true;
            btnRemoveProductType.IsEnabled = true;
        }

       
    }
}
