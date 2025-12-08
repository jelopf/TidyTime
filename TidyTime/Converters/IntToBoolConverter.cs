using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TidyTime.Converters;

public class IntToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count && parameter is string param)
        {
            return param == "0" 
                ? count == 0 
                : count > 0;
        }
        return false;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}