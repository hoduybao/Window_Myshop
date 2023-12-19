using System.Collections.Generic;

namespace MyShop.Model
{
    public class ProductSold
    {
        public List<double> SoldQuantity { get; set; }
        public List<double> RemainingQuantity { get; set; }
        public List<string> ProductNames { get; set; }
    }
}
