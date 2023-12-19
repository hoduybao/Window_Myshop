using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new Home());

        }

        public void btnCommands_Click(object sender, RoutedEventArgs e)
        {
            Button curButton = sender as Button;
            if (curButton.Tag.Equals("btnClose"))
            {
                this.Close();
            }
            else if (curButton.Tag.Equals("btnMinim"))
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (curButton.Tag.Equals("btnMaxim"))
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
        }

        /// <summary>
        /// Sự kiện di chuyển cửa sổ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void go_home(object sender, MouseButtonEventArgs e)
        {
        }

        private void go_products(object sender, MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new ListProduct());

        }

        private void go_sell(object sender, MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new Home());

        }

        private void go_Statistic(object sender, MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new StatisticPage());
        }
    }
}
