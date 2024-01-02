using MyShop.Model;
using MyShop.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer
    {
        List<Client> _clientList = new List<Client>();
        public Customer()
        {
            InitializeComponent();
            LoadClient();
        }

        private async void LoadClient()
        {
            await LoadClientList();
        }

        public async Task LoadClientList()
        {
            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm mới để lấy danh sách khách hàng
            List<Client> customerList = await ClientService.GetCustomerList(token);
            // Hiển thị danh sách sản phẩm
            _clientList = new List<Client>(customerList);
            // Get the current data source
            listClient.ItemsSource = _clientList;
            //listCustomer.Items.Refresh();
        }
        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var client = (Client)listClient.SelectedItem;
            if (client == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng muốn sửa");
                return;
            }
            var newClient = new Client()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                moneyString = client.moneyString

            };
            var newScreen = new UpdateClient(this, newClient);
            newScreen.Show();
        }


        private void listCustomer_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listClient.SelectedItem == null) return;
            Client client = listClient.SelectedItem as Client;

            // Bật button Sửa, Xóa
            btnUpdateCustomer.IsEnabled = true;
            btnDeleteClient.IsEnabled = true;
        }

        private async void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của đơn hàng được chọn           
            var selectedClient = (Client)listClient.SelectedItem;
            if (selectedClient == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng muốn xóa");
                return;
            }
            int clientID = selectedClient.Id;


            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            // Gọi hàm xóa khách hàng
            bool success = await ClientService.DeleteClient(clientID, token);

            // Nếu xóa thành công
            if (success)
            {
                // Xóa khách hàng khỏi danh sách                
                _clientList.Remove(selectedClient);
                MessageBox.Show("Khách hàng đã được xóa thành công");
                await LoadClientList();
            }
            else
            {
                MessageBox.Show("Không thể xóa hóa đơn");
            }
        }
    }
}
