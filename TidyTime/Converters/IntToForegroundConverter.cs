using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TidyTime.Converters;

public class IntToForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int currentDifficulty && parameter is string paramString)
        {
            if (int.TryParse(paramString, out int buttonDifficulty))
            {
                return currentDifficulty == buttonDifficulty 
                    ? "#FFFFFF" 
                    : "#656565";
            }
        }
        return "#656565";
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}