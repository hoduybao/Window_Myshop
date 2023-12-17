using Microsoft.Win32;
using MyShop.Model;
using MyShop.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        private Product _product;
        private ListProduct _listProduct;
        public UpdateProduct(ListProduct listProduct, Product product)
        {
            InitializeComponent();
            _listProduct = listProduct;
            _product = product;
            DataContext = product;
        }

        public int categoryIndex { get; set; } = -1;
        public string categoryName { get; set; } = string.Empty;

        BindingList<ProductType> _productsType;
        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string ProductName = NameBox.Text;
            double productPrice = double.Parse(PriceBox.Text);
            int productAmount = int.Parse(AmountBox.Text);
            double productDiscount = double.Parse(DiscountBox.Text);

            ProductType category = new ProductType
            {
                Id = categoryIndex,
                Name = categoryName,
            };

            Product newProduct = new Product
            {
                Id = this._product.Id,
                Name = ProductName,
                Price = productPrice,
                Category = category,
                //Image = selectedImagePath,
                Image = selectedImagePath != null ? selectedImagePath : _product.Image,
                Amount = productAmount,
                Discount = productDiscount,

            };


            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }
            Product result = await ProductService.UpdateProduct(newProduct, token);
            if (result != null)
            {
                MessageBox.Show("Cập nhật sản phẩm thành công!");
            }
            else
            {
                MessageBox.Show("Cập nhật sản phẩm thất bại!");
            }

            _listProduct?.UpdateProductList();

            if (_productsType != null && _productsType.Count > 0)
            {
                // Lấy ra đối tượng ProductType tương ứng với categoryIndex
                ProductType selectedProductType = _productsType.FirstOrDefault(pt => pt.Id == categoryIndex);

                // Set giá trị cho ComboBox
                categoryCombobox.SelectedItem = selectedProductType;
            }

            //categoryCombobox.SelectedItem = _productsType.FirstOrDefault(pt => pt.Id == categoryIndex);

            //bool check = await ProductService.UpdateProduct(newProduct, token);
            //if (check == true)
            //{
            //    MessageBox.Show("Cập nhật sản phẩm thành công!");
            //}
            //else
            //{
            //    MessageBox.Show("Cập nhật sản phẩm thất bại!");
            //}
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string selectedImagePath;
        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại chọn tệp tin
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
                Title = "Select an Image File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Lưu trữ đường dẫn của hình ảnh được chọn
                selectedImagePath = openFileDialog.FileName;
            }
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = (ProductType)categoryCombobox.SelectedItem;
            categoryIndex = selectedProduct.Id;
            categoryName = selectedProduct.Name;
            //categoryIndex = categoryCombobox.SelectedIndex;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                token = reader.ReadToEnd();
            }

            List<ProductType> _listType = await ProductTypeService.GetProductTypeList(token);
            _productsType = new BindingList<ProductType>(_listType);
            categoryCombobox.ItemsSource = _productsType;
            if (_product.Category != null)
            {
                categoryCombobox.SelectedItem = _productsType.FirstOrDefault(pt => pt.Id == _product.Category.Id);
            }
            else
            {
                // Không có loại sản phẩm cũ
                categoryCombobox.SelectedItem = null;
            }
        }
    }
}
