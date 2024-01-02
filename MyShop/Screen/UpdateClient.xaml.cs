using MyShop.Model;
using MyShop.Services;
using System.IO;
using System.Windows;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for UpdateClient.xaml
    /// </summary>
    public partial class UpdateClient : Window
    {
        private Client _client;
        private Customer _listClient;
        public UpdateClient(Customer listClient, Client client)
        {
            InitializeComponent();
            _listClient = listClient;
            _client = client;
            DataContext = client;
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string address = AddressBox.Text;
            string phoneNumber = PhoneNumberBox.Text;
            string email = EmailBox.Text;

            Client newClient = new Client
            {
                Id = this._client.Id,
                Name = name,
                Address = address,
                PhoneNumber = phoneNumber,
                Email = email,
                moneyString = this._client.moneyString
            };


            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }
            bool result = await ClientService.UpdateClient(newClient, token);
            if (result == true)
            {
                MessageBox.Show("Cập nhật khách hàng thành công!");
            }
            else
            {
                MessageBox.Show("Cập nhật khách hàng thất bại!");
            }

            await _listClient.LoadClientList();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
