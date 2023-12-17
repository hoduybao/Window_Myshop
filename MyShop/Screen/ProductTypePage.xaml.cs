using MyShop.Model;
using MyShop.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ProductType.xaml
    /// </summary>
    public partial class ProductTypePage
    {
        ObservableCollection<ProductType> productTypes = new ObservableCollection<ProductType>();

        public ProductTypePage()
        {
            InitializeComponent();
            //productTypes.Add(new ProductType { Name = "Iphone", Id = 1, NumOfProduct = 10, Description = "Điện thoại đẹp, thông minh" });
            //productTypes.Add(new ProductType { Name = "Samsung", Id = 2, NumOfProduct = 4, Description = "Điện thoại bền, camera xịn" });
            //productTypes.Add(new ProductType { Name = "Oppo", Id = 3, NumOfProduct = 8, Description = "Chụp ảnh xịn, giá rẻ" });
            //listProductType.ItemsSource = productTypes;
            LoadProductTypeList();
        }

        private async void LoadProductTypeList()
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                token = reader.ReadToEnd();
            }

            List<ProductType> _listType = await ProductTypeService.GetProductTypeList(token);
            productTypes = new ObservableCollection<ProductType>(_listType);

            //
            listProductType.ItemsSource = productTypes;
        }
        public void UpdateProductTypeList(IEnumerable<ProductType> productTypeList)
        {
            productTypes.Clear();
            foreach (var type in productTypeList)
            {
                productTypes.Add(type);
            }
        }
        private void BtnAddProductType_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProductType(this);
            //NavigationService.Navigate(screen);
            screen.Show();

        }

        private void BtnUpdateProductType_Click(object sender, RoutedEventArgs e)
        {
            var type = (ProductType)listProductType.SelectedItem;
            var updateType = new ProductType()
            {
                Id = type.Id,
                Name = type.Name
            };
            var newScreen = new UpdateProductType(this, updateType);
            newScreen.Show();
        }

        private async void BtnRemoveProductType_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của sản phẩm được chọn
            var productType = (ProductType)listProductType.SelectedItem;
            int typeId = productType.Id;

            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm xóa sản phẩm
            bool success = await ProductTypeService.DeleteProductType(typeId, token);

            // Nếu xóa thành công
            if (success)
            {
                // Xóa sản phẩm khỏi danh sách             
                productTypes.Remove(productType);
                MessageBox.Show("Sản phẩm đã được xóa thành công");
            }
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
