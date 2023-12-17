using MyShop.Model;
using MyShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        ObservableCollection<Product> _products;

        List<ProductOfOrder> productList = new List<ProductOfOrder>();

        private Order currentOrder;
        public Home()
        {
            InitializeComponent();
            currentOrder = new Order();
            currentOrder.OrderItems = new List<ProductOfOrder>();
            totalOrder.DataContext = currentOrder;
            productList = currentOrder.OrderItems;

        }

        int Product_amount;
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as ProductOfOrder;
                if (product != null)
                {
                    if (product.Amount < Product_amount) // Kiểm tra số lượng sản phẩm đã đặt hàng có nhỏ hơn số lượng hiện có không
                    {
                        product.Amount++;

                        UpdateProductListAmount(product.Id, 1);

                        // Cập nhật DataGrid              
                        dgHoaDon.Items.Refresh();
                        // Cập nhật tổng tiền của đơn hàng
                        UpdateTotalOrder();
                    }
                    else
                    {
                        MessageBox.Show("Không đủ sản phẩm để thêm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as ProductOfOrder;
                if (product != null && product.Amount > 0)
                {
                    product.Amount--;

                    UpdateProductListAmount(product.Id, -1);

                    // Cập nhật DataGrid
                    dgHoaDon.Items.Refresh();
                    // Cập nhật tổng tiền của đơn hàng
                    UpdateTotalOrder();
                }
            }
        }

        private void UpdateProductListAmount(int productId, int change)
        {
            // Cập nhật số lượng sản phẩm trong danh sách sản phẩm
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Amount += change;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgHoaDon.ItemsSource = productList;
            // Danh sách loại sản phẩm
            LoadProductTypeList();
            //
            sortCost.Items.Add(new ComboBoxItem { Content = "Tăng dần" });
            sortCost.Items.Add(new ComboBoxItem { Content = "Giảm dần" });
            ComboBoxItem itemSortToSelect = (ComboBoxItem)sortCost.Items[0];

            // Đặt IsSelected cho ComboBoxItem "Lựa chọn 1"
            itemSortToSelect.IsSelected = true;

            //list product
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ComboBox_SelectionChangedSort(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTypeProduct.SelectedItem != null)
            {
                string selectedOption = ((ComboBoxItem)sortCost.SelectedItem).Content.ToString();
            }
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
                productList.Sort((p1, p2) => p1.Price.CompareTo(p2.Price));
            }
            else if (sortType == "Giảm dần")
            {
                productList.Sort((p1, p2) => p2.Price.CompareTo(p1.Price));
            }
        }

        private void addMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sản phẩm được chọn
            Product selectedProduct = (Product)productsComboBox.SelectedItem;
            Product_amount = selectedProduct.Amount;
            if (selectedProduct.Amount > 0)
            {
                // Kiểm tra xem sản phẩm đã tồn tại trong danh sách "Thông tin sản phẩm" chưa
                if (productList.Any(item => item.Id == selectedProduct.Id))
                {
                    MessageBox.Show("Sản phẩm đã tồn tại trong danh sách thông tin sản phẩm.");
                    return;
                }

                Product product = new Product()
                {
                    Name = selectedProduct.Name
                };

                // Thêm sản phẩm vào danh sách "Thông tin sản phẩm" với số lượng mặc định là 1
                ProductOfOrder newProduct = new ProductOfOrder
                {
                    //STT = stt,
                    STT = productList.Count + 1,
                    Id = selectedProduct.Id,
                    Product = product,
                    Price = (decimal)selectedProduct.Price,
                    Amount = 1, // Số lượng mặc định là 1
                                //TongTien = selectedProduct.Price, // Tổng tiền ban đầu bằng giá của sản phẩm
                };


                // Giảm số lượng sản phẩm của sản phẩm đó
                selectedProduct.Amount--;
                productList.Add(newProduct);

                // Cập nhật DataGrid
                dgHoaDon.Items.Refresh();

                // Cập nhật tổng tiền của đơn hàng
                UpdateTotalOrder();

                currentOrder.OrderItems = productList;
            }
            else
            {
                MessageBox.Show("Số lượng sản phẩm còn lại không đủ.");
            }
        }

        private void UpdateTotalOrder()
        {
            // Cập nhật tổng tiền của đơn hàng
            currentOrder.Total = productList.Sum(p => p.TongTien);

            // Cập nhật DataContext để giao diện người dùng cập nhật tức thì
            totalOrder.DataContext = null;
            totalOrder.DataContext = currentOrder;
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sản phẩm được chọn để xóa từ danh sách "Danh sách sản phẩm"
            Product selectedProduct = (Product)productsComboBox.SelectedItem;

            // Kiểm tra xem sản phẩm đã được chọn trong danh sách "Danh sách sản phẩm" hay không
            if (selectedProduct != null)
            {
                // Kiểm tra xem sản phẩm đã được thêm vào danh sách "Thông tin sản phẩm" hay không
                ProductOfOrder existingProduct = productList.FirstOrDefault(p => p.Id == selectedProduct.Id);

                if (existingProduct != null)
                {
                    // Xóa sản phẩm khỏi danh sách "Thông tin sản phẩm"
                    productList.Remove(existingProduct);

                    // Cập nhật lại STT cho các sản phẩm còn lại trong danh sách
                    UpdateSTTForProductList();

                    // Cập nhật DataGrid
                    dgHoaDon.Items.Refresh();

                    // Cập nhật tổng tiền của đơn hàng
                    UpdateTotalOrder();
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu sản phẩm không tồn tại trong danh sách "Thông tin sản phẩm"
                    MessageBox.Show("Sản phẩm chưa được thêm vào đơn hàng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateSTTForProductList()
        {
            // Cập nhật lại STT cho danh sách sản phẩm
            for (int i = 0; i < productList.Count; i++)
            {
                productList[i].STT = i + 1;
            }
        }

        private void productsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnBillPayment_Click(object sender, RoutedEventArgs e)
        {
            string name = nameCustomer.Text;
            string phone = phoneCustomer.Text;
            Client client = new Client()
            {
                CustomerName = name,
                Email = "example@gmail.com",
                PhoneNumber = phone,
                Address = "TP HCM"
            };

            // Kiểm tra xem người dùng đã nhập tên khách hàng và số điện thoại chưa
            if (string.IsNullOrWhiteSpace(nameCustomer.Text) || string.IsNullOrWhiteSpace(phoneCustomer.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng.");
                return;
            }

            // Kiểm tra xem danh sách sản phẩm có sản phẩm nào không
            if (productList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng.");
                return;
            }
            try
            {
                string token = "";
                using (var reader = new StreamReader("data.json"))
                {
                    // Đọc dữ liệu từ file
                    token = reader.ReadToEnd();
                }
                // Gọi hàm AddOrder ở phần API để thêm đơn hàng
                Order addedOrder = await OrderService.AddOrder(client, GetOrderRequest(), token);

                if (addedOrder != null)
                {

                    // Cập nhật số lượng sản phẩm sau khi thanh toán thành công

                    foreach (var product in productList)
                    {
                        await OrderService.UpdateProductAmount(product.Id, Product_amount - product.Amount, token);
                    }
                    // Xử lý khi thêm đơn hàng thành công, có thể hiển thị thông báo, làm sạch danh sách hóa đơn, ...
                    MessageBox.Show("Đã thanh toán thành công!");
                    UpdateProductListAfterPayment();
                    // Đặt lại danh sách sản phẩm và thông tin khách hàng
                    productList.Clear();
                    dgHoaDon.Items.Refresh();
                    nameCustomer.Clear();
                    phoneCustomer.Clear();
                    moneyOfCustomer.Clear();
                    // Reset giá trị Total và leftMoney
                    currentOrder.Total = 0;
                    totalOrder.DataContext = null;
                    totalOrder.DataContext = currentOrder;
                    leftMoney.Text = "0";
                }
                else
                {
                    // Xử lý khi thêm đơn hàng không thành công
                    MessageBox.Show("Thanh toán thất bại!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi khi gọi API
                MessageBox.Show($"Error adding order: {ex.Message}");
            }

        }

        private void UpdateProductListAfterPayment()
        {
            // Duyệt qua danh sách sản phẩm trong đơn hàng
            foreach (var productInOrder in productList)
            {
                // Tìm sản phẩm tương ứng trong danh sách sản phẩm (_products)
                var productToUpdate = _products.FirstOrDefault(p => p.Id == productInOrder.Id);

                if (productToUpdate != null)
                {
                    // Cập nhật số lượng sản phẩm bằng cách trừ đi số lượng đã bán
                    productToUpdate.Amount -= productInOrder.Amount;

                    // Đảm bảo số lượng không bao giờ dưới 0
                    if (productToUpdate.Amount < 0)
                    {
                        productToUpdate.Amount = 0;
                    }
                }
            }

            // Cập nhật giao diện ListView
            productsComboBox.Items.Refresh();

        }
        private OrderRequest GetOrderRequest()
        {
            OrderRequest orderRequest = new OrderRequest
            {
                Phone = phoneCustomer.Text,
                ProductIds = productList.Select(item => item.Id).ToList(),
                Quantities = productList.Select(item => item.Amount).ToList(),
                Prices = productList.Select(item => (double)item.Price).ToList()
            };

            return orderRequest;
        }

        private void dgHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MoneyOfCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem TextBox có rỗng hay không
            if (string.IsNullOrWhiteSpace(moneyOfCustomer.Text))
            {
                MessageBox.Show("Vui lòng nhập số tiền khách hàng đưa.");
                return;
            }

            // Kiểm tra xem có thể tính toán tiền thừa hay không
            if (double.TryParse(moneyOfCustomer.Text, out double moneyFromCustomer))
            {
                // Tính tổng hóa đơn
                double totalBill = (double)productList.Sum(p => p.TongTien);

                // Kiểm tra nếu số tiền khách hàng đưa nhỏ hơn tổng hóa đơn
                if (moneyFromCustomer < totalBill)
                {
                    MessageBox.Show("Số tiền khách hàng đưa phải ít nhất bằng tổng hóa đơn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                    // Nếu có lỗi, bạn có thể làm gì đó ở đây, ví dụ: đặt giá trị mặc định hoặc xóa nội dung của TextBox
                    moneyOfCustomer.Text = string.Empty;
                    return;
                }

                // Tính tiền thừa
                double change = moneyFromCustomer - totalBill;

                // Hiển thị thông tin
                //totalOrder.Text = totalBill.ToString("C");
                leftMoney.Text = change.ToString();
            }
        }
    }
}
