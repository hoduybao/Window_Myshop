using System.Collections.Generic;

namespace MyShop.Model
{
    public class OrderRequest
    {
        public string Phone { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> Quantities { get; set; }
        public List<double> Prices { get; set; }
    }
}
