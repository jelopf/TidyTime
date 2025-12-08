using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TidyTime.Converters;

public class BoolToBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool isSelected && isSelected 
            ? "#FFFFFF" 
            : "Transparent";
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}