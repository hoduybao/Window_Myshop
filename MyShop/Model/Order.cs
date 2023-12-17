using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyShop.Model
{
    public class Order : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Client Client { get; set; }
        public double Total { get; set; }
        // public List<ProductOfOrder> OrderItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<ProductOfOrder> orderItems;
        public List<ProductOfOrder> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                // Cập nhật tổng tiền mỗi khi danh sách sản phẩm thay đổi
                Total = orderItems.Sum(item => item.TongTien);
            }
        }
    }
}
