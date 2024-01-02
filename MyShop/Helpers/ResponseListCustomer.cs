using MyShop.Model;
using System.Collections.Generic;

namespace MyShop.Helpers
{
    public class ResponseListCustomer
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<Client> data { get; set; }

    }
}
