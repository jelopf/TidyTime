using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TidyTime.Models;

namespace TidyTime.Converters;

public class TaskStatusToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskStatus status && parameter is string param)
        {
            return param == "Completed" 
                ? status == TaskStatus.Completed 
                : status != TaskStatus.Completed;
        }
        return false;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}