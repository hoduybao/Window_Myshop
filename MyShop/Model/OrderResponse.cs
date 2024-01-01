using System.Collections.Generic;

namespace MyShop.Model
{
    public class OrderResponse
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<Order> Orders { get; set; }
        public int CurrentPage { get; set; }
    }
}
