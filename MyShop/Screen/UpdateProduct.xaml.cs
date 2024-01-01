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
        private int _currentPage;
        public UpdateProduct(ListProduct listProduct, Product product, int currentPage)
        {
            InitializeComponent();
            _listProduct = listProduct;
            _product = product;
            DataContext = product;
            _currentPage = currentPage;
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

            //if (_listProduct != null)
            //{
            //    // Assuming you have methods to load products by page and category
            //    await LoadProductListByPage(3);
            //     await _listProduct.LoadProductListByCategory(categoryIndex);
            //}

            // _listProduct?.UpdateProductList();
            await _listProduct.LoadProductListByPage(_currentPage);
            if (_productsType != null && _productsType.Count > 0)
            {
                // Lấy ra đối tượng ProductType tương ứng với categoryIndex
                ProductType selectedProductType = _productsType.FirstOrDefault(pt => pt.Id == categoryIndex);

                // Set giá trị cho ComboBox
                categoryCombobox.SelectedItem = selectedProductType;
            }
            this.Close();
        }


        //int _totalPages;
        //private int _currentPage;
        //private int _pageSize;
        //private async Task LoadProductListByPage(int page)
        //{
        //    // Lấy loại sản phẩm đang được chọn          
        //    ProductType selectedProductType = _productsType.FirstOrDefault(pt => pt.Id == categoryIndex);
        //    if (selectedProductType != null)
        //    {
        //        categoryIndex = selectedProductType.Id;

        //        string token = "";
        //        using (var reader = new StreamReader("data.json"))
        //        {
        //            // Đọc dữ liệu từ file
        //            token = reader.ReadToEnd();
        //        }

        //        List<Product> productList;
        //        // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
        //        ProductByPage productByPage;
        //        if (categoryIndex == -1)
        //        {

        //            productByPage = await ProductService.GetProductListByPage(_currentPage, _pageSize, token);
        //            productList = productByPage.Products;
        //            _totalPages = productByPage.TotalPages;
        //        }
        //        else
        //        {
        //            // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
        //            productByPage = await ProductService.GetProductListCategoryByPage(page, _pageSize, categoryIndex, token);
        //            productList = productByPage.Products;
        //            _totalPages = productByPage.TotalPages;
        //        }

        //        // Hiển thị danh sách sản phẩm
        //        //UpdateProductList(productList);
        //        _products = new ObservableCollection<Product>(productList);

        //        // Get the current data source
        //        productsComboBox.ItemsSource = _products;
        //        // Cập nhật phân trang
        //        // UpdatePagination();
        //    }
        //}

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
