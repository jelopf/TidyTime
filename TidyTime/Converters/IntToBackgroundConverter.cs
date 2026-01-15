using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TidyTime.Converters;

public class IntToBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int currentDifficulty && parameter is string paramString)
        {
            if (int.TryParse(paramString, out int buttonDifficulty))
            {
                return currentDifficulty == buttonDifficulty 
                    ? "#86C6FF" 
                    : "#EFEFEE";
            }
        }
        return "#EFEFEE";
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}