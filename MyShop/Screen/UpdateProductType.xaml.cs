using MyShop.Model;
using MyShop.Services;
using System.IO;
using System.Windows;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for UpdateProductType.xaml
    /// </summary>
    public partial class UpdateProductType : Window
    {
        private ProductType _productType;
        ProductTypePage _listProductType;
        public UpdateProductType(ProductTypePage listProductType, ProductType type)
        {
            InitializeComponent();
            _productType = type;
            _listProductType = listProductType;
            DataContext = type;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string typeName = newProductNameTextBox.Text;

            ProductType newType = new ProductType
            {
                Id = _productType.Id,
                Name = typeName
            };
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            ProductType check = await ProductTypeService.UpdateProductType(newType, token);
            _listProductType.UpdateProductTypeList(await ProductTypeService.GetProductTypeList(token));
            //if (check == true)
            //{
            //    MessageBox.Show("Cập nhật thành công!");
            //}
            //else
            //{
            //    MessageBox.Show("Cập nhật thất bại!");
            //}
            this.Close();
        }
    }
}
