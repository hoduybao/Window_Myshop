namespace MyShop.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public ProductType Category { get; set; }
        public int Amount { get; set; }
        public double Discount { get; set; }
        public double Profit { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
