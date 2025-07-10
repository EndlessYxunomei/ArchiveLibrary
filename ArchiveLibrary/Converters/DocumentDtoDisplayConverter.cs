using ArchiveModels.DTO;
using System.Globalization;

namespace ArchiveLibrary.Converters;

public class DocumentDtoDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DocumentListDto documentListDto)
        {
            return $"{documentListDto.Name} от {documentListDto.Date:d}";
        }
        else return string.Empty ;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

}
