using System;
using System.Globalization;
using System.Windows.Data;

namespace MyShop.Helpers
{
    public class percentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double discount)
            {
                // Chuyển đổi giá trị discount thành định dạng phần trăm
                return $"{discount:P0}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
