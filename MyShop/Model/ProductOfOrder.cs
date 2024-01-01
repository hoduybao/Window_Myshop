namespace MyShop.Model
{
    public class ProductOfOrder
    {
        public int STT { get; set; }
        public int Id { get; set; } = 0;
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        // public int Amount { get; set; }

        public double TongTien => (double)Price * Quantity;
        //public double TongTien => (double)Price * Amount;
        //public double TongTien { get; set; }


    }
}
