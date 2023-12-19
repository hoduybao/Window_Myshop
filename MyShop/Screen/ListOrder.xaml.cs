using MyShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for ListOrder.xaml
    /// </summary>
    /// 

    public partial class ListOrder : Page
    {
        List<Order> orderList = new List<Order>();

        public ListOrder()
        {
            InitializeComponent();
        }

        private void btnViewDetailOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnApplyFindOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Order newProduct = new Order
            {
               Id = 1,
               OrderDate= DateTime.Now,
               Client= new Client { Id = 1 ,CustomerName="Bao",PhoneNumber="01231",Address="",Email=""},
               Total=10000,
              
            };
            Order newProduct2 = new Order
            {
                Id = 1,
                OrderDate = DateTime.Now,
                Client = new Client { Id = 1, CustomerName = "Bao", PhoneNumber = "01231", Address = "", Email = "" },
                Total = 10000,

            };


            // Giảm số lượng sản phẩm của sản phẩm đó
            orderList.Add(newProduct);
            orderList.Add(newProduct2);

            dsHoaDon.ItemsSource = orderList;
            // Cập nhật DataGrid
            dsHoaDon.Items.Refresh();
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
