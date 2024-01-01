using Microsoft.Win32;
using MyShop.Model;
using MyShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ListProduct.xaml
    /// </summary>
    public partial class ListProduct
    {
        ObservableCollection<Product> _products;
        private int _currentPage = 1;
        bool flag = false;
        public int _pageSize;
        private int _totalPages = 0;

        public ListProduct()
        {
            InitializeComponent();
            LoadProductTypeList();
            sortCost.Items.Add(new ComboBoxItem { Content = "Tăng dần" });
            sortCost.Items.Add(new ComboBoxItem { Content = "Giảm dần" });
            ComboBoxItem itemSortToSelect = (ComboBoxItem)sortCost.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            itemSortToSelect.IsSelected = true;
            _pageSize = 10;
            NumberProduct.Text = _pageSize.ToString();

            //LoadProductList();
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

        //private async void LoadProductList()
        //{
        //    // Fetch product list from API
        //    List<Product> totalProduct = await ProductService.GetProductList();
        //    string token = "";
        //    using (var reader = new StreamReader("data.json"))
        //    {
        //        // Đọc dữ liệu từ file
        //        token = reader.ReadToEnd();
        //    }
        //    // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang 
        //    //List<Product> productList = await ProductService.GetProductListByPage(_currentPage, _pageSize, token);
        //    ProductByPage productByPage = await ProductService.GetProductListByPage(_currentPage, _pageSize, token);
        //    List<Product> productList = productByPage.Products;
        //    _totalPages = (totalProduct.Count / _pageSize) + ((totalProduct.Count % _pageSize) == 0 ? 0 : 1);

        //    _products = new ObservableCollection<Product>(productList);

        //    // Get the current data source
        //    productsComboBox.ItemsSource = _products;
        //    //new

        //    if (flag == false)
        //    {
        //        flag = true;

        //        UpdatePagination();
        //    }
        //}

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var type = (ProductType)comboBoxTypeProduct.SelectedItem;
            //categoryIndex = type.Id;
            ProductType selectedProductType = comboBoxTypeProduct.SelectedItem as ProductType;
            if (selectedProductType != null)
            {
                categoryIndex = selectedProductType.Id;

                string token = "";
                using (var reader = new StreamReader("data.json"))
                {
                    // Đọc dữ liệu từ file
                    token = reader.ReadToEnd();
                }

                ProductByPage productByPage;
                List<Product> productList;
                // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
                if (categoryIndex == -1)
                {
                    productByPage = await ProductService.GetProductListByPage(_currentPage, _pageSize, token);
                    productList = productByPage.Products;
                    _totalPages = productByPage.TotalPages;
                }
                else
                {
                    // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
                    productByPage = await ProductService.GetProductListCategoryByPage(_currentPage, _pageSize, categoryIndex, token);
                    productList = productByPage.Products;
                    _totalPages = productByPage.TotalPages;
                }


                // Hiển thị danh sách sản phẩm
                //UpdateProductList(productList);
                _products = new ObservableCollection<Product>(productList);
                productsComboBox.ItemsSource = _products;

                // Cập nhật phân trang
                flag = false;
                UpdatePagination();
            }
        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                await LoadProductListByPage(_currentPage);
                UpdatePagination();
            }
        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                // UpdatePagination();
                //await LoadProductsAndPaginate();
                await LoadProductListByPage(_currentPage);
                UpdatePagination();
            }
        }

        private async void pagingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                flag = true;
                return;
            }
            if (pagingComboBox.SelectedItem != null)
            {
                PaginationItem selectedPage = (PaginationItem)pagingComboBox.SelectedItem;

                // Lấy trang hiện tại và cập nhật _currentPage
                _currentPage = selectedPage.Page;

                // Gọi hàm để lấy danh sách sản phẩm theo loại và trang
                await LoadProductListByPage(_currentPage);
            }
        }
        public async Task LoadProductListByPage(int page)
        {
            // Lấy loại sản phẩm đang được chọn          
            ProductType selectedProductType = comboBoxTypeProduct.SelectedItem as ProductType;
            if (selectedProductType != null)
            {
                categoryIndex = selectedProductType.Id;

                string token = "";
                using (var reader = new StreamReader("data.json"))
                {
                    // Đọc dữ liệu từ file
                    token = reader.ReadToEnd();
                }

                List<Product> productList;
                // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
                ProductByPage productByPage;
                if (categoryIndex == -1)
                {

                    productByPage = await ProductService.GetProductListByPage(_currentPage, _pageSize, token);
                    productList = productByPage.Products;
                    _totalPages = productByPage.TotalPages;
                }
                else
                {
                    // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
                    productByPage = await ProductService.GetProductListCategoryByPage(page, _pageSize, categoryIndex, token);
                    productList = productByPage.Products;
                    _totalPages = productByPage.TotalPages;
                }

                // Hiển thị danh sách sản phẩm
                //UpdateProductList(productList);
                _products = new ObservableCollection<Product>(productList);

                // Get the current data source
                productsComboBox.ItemsSource = _products;
                // Cập nhật phân trang
                // UpdatePagination();
            }
        }

        private void UpdatePagination()
        {
            // Xóa các mục hiện tại trong ComboBox
            pagingComboBox.Items.Clear();

            // Thêm các mục mới với số trang từ 1 đến totalPages
            for (int i = 1; i <= _totalPages; i++)
            {
                // Tạo một đối tượng PaginationItem và thêm vào ComboBox
                PaginationItem pageItem = new PaginationItem { Page = i, Total = _totalPages };
                pagingComboBox.Items.Add(pageItem);
            }

            // Chọn trang hiện tại
            pagingComboBox.SelectedIndex = _currentPage - 1;
        }
        public void UpdateProductList(IEnumerable<Product> productList)
        {
            if (_products != null)
            {
                _products.Clear();
            }

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
            // UpdatePagination();
            productsComboBox.ItemsSource = _products;
            // await LoadProductListByPage(_currentPage);

        }

        private void productsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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


        private void ComboBox_SelectionChangedSort(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new AddProduct(this, _currentPage);
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
                await LoadProductListByPage(_currentPage);

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
            var newScreen = new UpdateProduct(this, newproduct, _currentPage);
            newScreen.Show();

        }


        private string selectedPath;
        private async void btnImportProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*",
                    Title = "Chọn file Excel"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Lưu trữ đường dẫn của hình ảnh được chọn
                    selectedPath = openFileDialog.FileName;
                    bool result = await BackupService.ImportFile(selectedPath);
                    if (result == true)
                    {
                        //new
                        await LoadProductListByPage(_currentPage);
                        UpdatePagination();
                        MessageBox.Show("Nhập sản phẩm từ Excel thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi nhập sản phẩm từ Excel.");
                    }
                    // Gọi hàm ImportProductsFromExcel với đường dẫn file Excel và accessToken (bạn cần thay thế accessToken thực tế)
                    //string token = "";
                    //using (var reader = new StreamReader("data.json"))
                    //{
                    //    // Đọc dữ liệu từ file
                    //    token = reader.ReadToEnd();
                    //}
                    //var importResult = BackupService.ImportProductsFromExcel(selectedPath, token);

                    //if (importResult.Result != null)
                    //{
                    //    //_products.UpdateProductList(await ProductService.GetProductList());
                    //    // _products = new ObservableCollection<Product>(await ProductService.GetProductList());
                    //    //UpdateProductList(await ProductService.GetProductList());
                    //    MessageBox.Show("Nhập sản phẩm từ Excel thành công!");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Lỗi khi nhập sản phẩm từ Excel.");
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }

        }

        private void btnListTypeProduct_Click(object sender, RoutedEventArgs e)
        {
            var page = new ProductTypePage();
            NavigationService.Navigate(page);
        }

        private async void btnExportProduct_Click(object sender, RoutedEventArgs e)
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                token = reader.ReadToEnd();
            }
            bool result = await BackupService.ExportFile(token);
        }

        private async void NumberProduct_LostFocus(object sender, RoutedEventArgs e)
        {
            string size = NumberProduct.Text;
            int newPageSize;
            if (int.TryParse(size, out newPageSize))
            {
                // Input is a valid integer
                _pageSize = newPageSize;
                await LoadProductListByPage(_currentPage);
                UpdatePagination();
            }
            else
            {
                // Input is not a valid integer
                // Handle the error (e.g., display a message, reset the TextBox)
                MessageBox.Show("Invalid page size. Please enter a valid integer.");
                NumberProduct.Text = _pageSize.ToString();  // Reset to previous value
            }
        }


    }
}
