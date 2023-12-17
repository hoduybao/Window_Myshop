namespace MyShop.Helpers
{
    public class ResponseData<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
