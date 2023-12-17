using MyShop.Model;
using MyShop.Screen;
using MyShop.Services;
using System.Windows;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string user = editUsername.Text;
            string pass = editPassword.Password;

            Account account = new Account();
            account.Username = user;
            account.Password = pass;

            bool check = await AuthService.Login(account);

            if (check)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Left = Application.Current.MainWindow.Left;
                dashboard.Top = Application.Current.MainWindow.Top;
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}