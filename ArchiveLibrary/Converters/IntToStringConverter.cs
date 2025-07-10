using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveLibrary.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null || value is not int inventory_number || inventory_number < 1 ? string.Empty : value.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null || value is not string string_value ? 0 : int.TryParse(string_value, out int res) ? res : 0;
        }
    }
}
