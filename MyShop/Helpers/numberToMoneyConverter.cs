using System;
using System.Globalization;
using System.Windows.Data;

namespace MyShop.Helpers
{
    public class numberToMoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            if (value.ToString() == "0")
            {
                return "0 đ";
            }
            string a = double.Parse(value.ToString()).ToString("#,###", cul.NumberFormat) + " đ";

            return a;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }

    }
}
