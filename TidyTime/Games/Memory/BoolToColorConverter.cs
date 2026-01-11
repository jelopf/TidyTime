using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TidyTime.Games.Memory;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isPlayersTurn)
        {
            return isPlayersTurn 
                ? new SolidColorBrush(Color.Parse("#00AA00")) // Зеленый - ход игрока
                : new SolidColorBrush(Color.Parse("#FFAA00")); // Оранжевый - показывает игра
        }
        
        return new SolidColorBrush(Colors.Gray);
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}