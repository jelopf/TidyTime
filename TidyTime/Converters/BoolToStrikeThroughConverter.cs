using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TidyTime.Converters;

public class BoolToStrikethroughConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool isStrikethrough && isStrikethrough 
            ? TextDecorations.Strikethrough 
            : null;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}