using MyShop.Model;

namespace MyShop.Helpers
{
    public class ResponseProductPage
    {
        public string status { get; set; }
        public string messeage { get; set; }
        public ProductByPage data { get; set; }
    }
}
