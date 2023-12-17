using MyShop.Model;

namespace MyShop.Helpers
{
    public class Response1Order
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Order Data { get; set; }
    }
}
