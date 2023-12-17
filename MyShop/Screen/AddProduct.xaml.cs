using Microsoft.Win32;
using MyShop.Model;
using MyShop.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>

    public partial class AddProduct : Window
    {
        ObservableCollection<Product> _products;

        ListProduct _listProduct;// Thêm biến để lưu tham chiếu đến ListProduct                                
        public AddProduct(ListProduct listProduct)
        {
            InitializeComponent();
            _listProduct = listProduct;
        }

        public int categoryIndex { get; set; } = -1;
        private void categoryCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedProduct = (ProductType)categoryCombobox.SelectedItem;
            categoryIndex = selectedProduct.Id;
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        ObservableCollection<ProductType> _productsType;
        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            string ProductName = NameBox.Text;
            double productPrice = double.Parse(PriceBox.Text);
            int productAmount = int.Parse(AmountBox.Text);
            double productDiscount = double.Parse(DiscountBox.Text);

            ProductType category = new ProductType
            {
                Id = categoryIndex
            };

            Product newProduct = new Product
            {
                Name = ProductName,
                Price = productPrice,
                Category = category,
                Image = selectedImagePath,
                Amount = productAmount,
                Discount = productDiscount,

            };
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            Product result = await ProductService.AddProduct(newProduct, token);
            // _products.Add(result);
            _listProduct.UpdateProductList(await ProductService.GetProductList());

            //bool check = await ProductService.AddProduct(newProduct, token);
            //if (check == true)
            //{
            //    MessageBox.Show("Thêm sản phẩm thành công!");
            //}
            //else
            //{
            //    MessageBox.Show("Thêm sản phẩm thất bại!");
            //}

            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                token = reader.ReadToEnd();
            }

            List<ProductType> _listType = await ProductTypeService.GetProductTypeList(token);
            _productsType = new ObservableCollection<ProductType>(_listType);
            categoryCombobox.ItemsSource = _productsType;
        }

    }
}
