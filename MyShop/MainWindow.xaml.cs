using MyShop.Model;
using MyShop.Screen;
using MyShop.Services;
using System.Configuration;
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
            string username = editUsername.Text;
            string password = editPassword.Password;

            if (rememberMe.IsChecked == true)
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Username"].Value = username;
                config.AppSettings.Settings["Password"].Value = password;
                config.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
            }
            Account account = new Account();
            account.Username = username;
            account.Password = password;

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

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            editUsername.Text = ConfigurationManager.AppSettings["Username"];
            editPassword.Password = ConfigurationManager.AppSettings["Password"];
        }
    }
}