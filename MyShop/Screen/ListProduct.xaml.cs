using MyShop.Model;
using MyShop.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ListProduct.xaml
    /// </summary>
    public partial class ListProduct
    {
        ObservableCollection<Product> _products;

        public ListProduct()
        {
            InitializeComponent();
            //comboBoxTypeProduct.Items.Add(new ComboBoxItem { Content = "Tất cả" });
            //ComboBoxItem itemToSelect = (ComboBoxItem)comboBoxTypeProduct.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            //itemToSelect.IsSelected = true;
            LoadProductTypeList();

            sortCost.Items.Add(new ComboBoxItem { Content = "Tăng dần" });
            sortCost.Items.Add(new ComboBoxItem { Content = "Giảm dần" });

            ComboBoxItem itemSortToSelect = (ComboBoxItem)sortCost.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            itemSortToSelect.IsSelected = true;

            LoadProductList();
        }
        ObservableCollection<ProductType> _productsType;
        private async void LoadProductTypeList()
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                token = reader.ReadToEnd();
            }

            List<ProductType> _listType = await ProductTypeService.GetProductTypeList(token);
            // Thêm mục "Tất cả" vào đầu danh sách
            _listType.Insert(0, new ProductType { Id = -1, Name = "Tất cả" });
            _productsType = new ObservableCollection<ProductType>(_listType);
            comboBoxTypeProduct.ItemsSource = _productsType;
            // Mặc định chọn "Tất cả"
            comboBoxTypeProduct.SelectedIndex = 0;
        }

        private async void LoadProductList()
        {
            // Fetch product list from API
            List<Product> productList = await ProductService.GetProductList();

            // Update products list
            _products = new ObservableCollection<Product>(productList);

            // Get the current data source
            productsComboBox.ItemsSource = _products;
        }

        public void UpdateProductList(IEnumerable<Product> productList)
        {
            _products.Clear();
            foreach (var product in productList)
            {
                _products.Add(product);
            }
        }

        public async void UpdateProductList()
        {
            _products = new ObservableCollection<Product>(await ProductService.GetProductList());
            productsComboBox.ItemsSource = _products;
        }

        private async void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string name = searchNameProduct.Text;

            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Thực hiện tìm kiếm sản phẩm
            List<Product> productList = await ProductService.SearchProduct(name, token);

            // Lọc theo loại sản phẩm
            ProductType selectedProductType = comboBoxTypeProduct.SelectedItem as ProductType;
            if (selectedProductType != null && selectedProductType.Name != "Tất cả")
            {
                productList = productList.Where(p => p.Category?.Id == selectedProductType.Id).ToList();
            }

            //Lọc theo khoảng giá
            double minPrice = minValue.Value;
            double maxPrice = maxValue.Value;
            productList = FilterProductsByPrice(productList, minPrice, maxPrice);

            // Sắp xếp theo giá
            SortProductsByPrice(productList);
            _products = new ObservableCollection<Product>(productList);
            // Get the current data source
            productsComboBox.ItemsSource = _products;
        }

        private void productsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //new
            // Reset page when selection changes
            _currentPage = 0;
            UpdatePagination();
            //
            if (productsComboBox.SelectedItem == null) return;
            Product product = productsComboBox.SelectedItem as Product;

            // Bật 2 button Sửa và Xóa
            btnUpdateProduct.IsEnabled = true;
            btnDeleteProduct.IsEnabled = true;
        }

        private List<Product> FilterProductsByPrice(List<Product> productList, double minPrice, double maxPrice)
        {
            if (productList == null)
            {
                return new List<Product>();
            }

            return productList.Where(product => product.Price >= minPrice && product.Price <= maxPrice).ToList();
        }

        private void SortProductsByPrice(List<Product> productList)
        {
            if (productList == null)
            {
                return;
            }

            // Lấy loại sắp xếp được chọn từ ComboBox
            string sortType = ((ComboBoxItem)sortCost.SelectedItem)?.Content.ToString();

            // Sắp xếp theo giá tăng dần hoặc giảm dần
            if (sortType == "Tăng dần")
            {
                productList.Sort((p1, p2) => p1.NewPrice.CompareTo(p2.NewPrice));
            }
            else if (sortType == "Giảm dần")
            {
                productList.Sort((p1, p2) => p2.NewPrice.CompareTo(p1.NewPrice));
            }
        }

        public int categoryIndex { get; set; } = -1;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var type = (ProductType)comboBoxTypeProduct.SelectedItem;
            //categoryIndex = type.Id;
        }

        private void ComboBox_SelectionChangedSort(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new AddProduct(this);
            newScreen.Show();
        }

        private async void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của sản phẩm được chọn
            var product = (Product)productsComboBox.SelectedItem;
            if (product == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm muốn xóa");
                return;
            }
            int productId = product.Id;

            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm xóa sản phẩm
            bool success = await ProductService.DeleteProduct(productId, token);

            // Nếu xóa thành công
            if (success)
            {
                // Xóa sản phẩm khỏi danh sách             
                _products.Remove(product);
                MessageBox.Show("Sản phẩm đã được xóa thành công");
            }
        }

        Product _backup;
        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)productsComboBox.SelectedItem;
            if (product == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm muốn sửa");
                return;
            }
            var newproduct = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,

                Price = product.Price,
                Category = product.Category,
                Amount = product.Amount,
                Discount = product.Discount

            };
            //_backup = (Product)product.Clone();
            var newScreen = new UpdateProduct(this, newproduct);
            newScreen.Show();

        }

        private void btnImportProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnListTypeProduct_Click(object sender, RoutedEventArgs e)
        {
            var page = new ProductTypePage();
            NavigationService.Navigate(page);
        }

        // Phân trang
        private int _itemsPerPage = 10;
        private int _currentPage = 0;

        private void UpdatePagination()
        {
            CollectionViewSource.GetDefaultView(_products).Refresh();
        }


        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            UpdatePagination();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                UpdatePagination();
            }
        }


    }
}
