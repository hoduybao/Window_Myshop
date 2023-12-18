using System.ComponentModel;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

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

        public double NewPrice => (double)Price * (1 - Discount / 100);
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
