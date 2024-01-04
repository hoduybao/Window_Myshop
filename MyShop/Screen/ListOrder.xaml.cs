using MyShop.Model;
using MyShop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ListOrder.xaml
    /// </summary>
    /// 

    public partial class ListOrder : Page
    {
        List<Order> _orderList = new List<Order>();

        private int _currentPage = 1;
        bool flag = false;
        public int _pageSize;
        private int _totalPages = 0;

        public ListOrder()
        {
            InitializeComponent();
            _pageSize = 10;
            NumberProduct.Text = _pageSize.ToString();
            LoadOrderList();
        }

        private async void LoadOrderList()
        {
            await LoadOrderListByPage(_currentPage);
        }

        public async Task LoadOrderListByPage(int page)
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            List<Order> orderList;
            // Gọi hàm mới để lấy danh sách sản phẩm theo loại và trang
            OrderResponse orderResponse;
            orderResponse = await OrderService.GetOrderListByPage(_currentPage, _pageSize, token);
            orderList = orderResponse.Orders;
            _totalPages = orderResponse.TotalPages;

            // Hiển thị danh sách sản phẩm
            _orderList = new List<Order>(orderList);
            // Get the current data source
            dsHoaDon.ItemsSource = _orderList;
            dsHoaDon.Items.Refresh();
            //moi them
            UpdatePagination();
        }

        private void btnViewDetailOrder_Click(object sender, RoutedEventArgs e)
        {
            var page = new DetailOrder();
            NavigationService.Navigate(page);
        }

        private async void btnApplyFindOrder_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = startDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = endDatePicker.SelectedDate ?? DateTime.MaxValue;

            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm SearchOrderByDate từ API
            List<Order> searchedOrders = await OrderService.SearchOrderByDate(startDate, endDate, token);

            // Kiểm tra và xử lý dữ liệu trả về
            if (searchedOrders != null)
            {
                // Hiển thị dữ liệu trả về trong DataGrid (dsHoaDon)
                dsHoaDon.ItemsSource = searchedOrders;
            }
            else
            {
                // Xử lý khi có lỗi hoặc không có dữ liệu trả về
            }
        }

        private void dgHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dsHoaDon.SelectedItem == null) return;
            Order product = dsHoaDon.SelectedItem as Order;

            // Bật button Xóa
            btnDeleteOrder.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Order newProduct = new Order
            //{
            //    Id = 1,
            //    OrderDate = DateTime.Now,
            //    Client = new Client { Id = 1, CustomerName = "Bao", PhoneNumber = "01231", Address = "", Email = "" },
            //    Total = 10000,

            //};
            //Order newProduct2 = new Order
            //{
            //    Id = 1,
            //    OrderDate = DateTime.Now,
            //    Client = new Client { Id = 1, CustomerName = "Bao", PhoneNumber = "01231", Address = "", Email = "" },
            //    Total = 10000,

            //};


            // Giảm số lượng sản phẩm của sản phẩm đó
            //_orderList.Add(newProduct);
            //_orderList.Add(newProduct2);

            dsHoaDon.ItemsSource = _orderList;
            // Cập nhật DataGrid
            dsHoaDon.Items.Refresh();
        }

        private async void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của đơn hàng được chọn           
            var selectedOrder = (Order)dsHoaDon.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn muốn xóa");
                return;
            }
            int orderId = selectedOrder.Id;


            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm xóa sản phẩm
            bool success = await OrderService.DeleteOrder(orderId, token);

            // Nếu xóa thành công
            if (success)
            {
                // Xóa sản phẩm khỏi danh sách                
                _orderList.Remove(selectedOrder);
                MessageBox.Show("Hóa đơn đã được xóa thành công");
                await LoadOrderListByPage(_currentPage);
            }
            else
            {
                MessageBox.Show("Không thể xóa hóa đơn");
            }
        }

        private async void NumberProduct_LostFocus(object sender, RoutedEventArgs e)
        {
            string size = NumberProduct.Text;
            int newPageSize;
            if (int.TryParse(size, out newPageSize))
            {
                // Input is a valid integer
                _pageSize = newPageSize;
                await LoadOrderListByPage(_currentPage);
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


        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                await LoadOrderListByPage(_currentPage);
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
                await LoadOrderListByPage(_currentPage);
            }
        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                await LoadOrderListByPage(_currentPage);
                UpdatePagination();
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
    }
}
