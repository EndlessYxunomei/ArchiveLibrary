using System.ComponentModel;
using System.Globalization;

namespace ArchiveLibrary.Converters;

public class EnumToIntConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        //если значение null, то выкидываем ошибку
        ArgumentNullException.ThrowIfNull(value);
        //если значение не Enum, то выкидывем ошибку
        return value is Enum
            ? (int)value
            : throw new ArgumentException("Not Enum type value");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        //если значение null, то выкидываем ошибку
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(value);
        //если значение не Enum, то выкидывем ошибку
        return Enum.IsDefined(targetType, value)
            ? Enum.ToObject(targetType, value)
            : throw new InvalidEnumArgumentException($"{value} is not valid for {parameter.ToString}.");
    }
}
