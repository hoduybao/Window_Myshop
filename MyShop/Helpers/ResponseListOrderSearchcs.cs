using MyShop.Model;
using System.Collections.Generic;

namespace MyShop.Helpers
{
    public class ResponseListOrderSearchcs
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Order> data { get; set; }

    }
}
