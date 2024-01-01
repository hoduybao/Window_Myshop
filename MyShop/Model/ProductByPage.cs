using System.Collections.Generic;

namespace MyShop.Model
{
    public class ProductByPage
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<Product> Products { get; set; }
    }
    public class PaginationItem
    {
        public int Page { get; set; }
        public int Total { get; set; }

    }
}
