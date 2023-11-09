using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyShop.Helpers
{
    public class indexConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ListViewItem listViewItem)
            {
                // Lấy ra ListView của ListViewItem
                ListView listView = ItemsControl.ItemsControlFromItemContainer(listViewItem) as ListView;

                if (listView != null)
                {
                    // Lấy ra chỉ mục của ListViewItem trong ListView
                    int index = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);

                    // Chuyển đổi index từ 0-based thành 1-based
                    int stt = index + 1;

                    return stt.ToString();
                }
            }

            return string.Empty; // Trả về chuỗi rỗng nếu không thể chuyển đổi
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("This operation is not supported.");
        }
    }
}
