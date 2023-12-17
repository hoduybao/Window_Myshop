using MyShop.Model;
using MyShop.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for AddProductType.xaml
    /// </summary>
    public partial class AddProductType : Window
    {
        ObservableCollection<ProductType> _types;
        ProductTypePage _listProductType;
        public AddProductType(ProductTypePage listProductType)
        {
            InitializeComponent();
            _listProductType = listProductType;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            string typeName = newProductNameTextBox.Text;

            ProductType newType = new ProductType
            {
                Name = typeName
            };
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            ProductType result = await ProductTypeService.AddProductType(newType, token);
            _listProductType.UpdateProductTypeList(await ProductTypeService.GetProductTypeList(token));
            //bool check = await ProductTypeService.AddProductType(newType, token);
            //if (check == true)
            //{
            //    MessageBox.Show("Thêm loại sản phẩm mới thành công!");
            //}
            //else
            //{
            //    MessageBox.Show("Thêm loại sản phẩm mới thất bại!");
            //}
            this.Close();
        }
    }
}
