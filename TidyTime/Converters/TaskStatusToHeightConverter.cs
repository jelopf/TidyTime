using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TidyTime.Models;

namespace TidyTime.Converters;

public class TaskStatusToHeightConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskStatus status)
        {
            // высота 40 для обычных и 46 для выполненных
            return status == TaskStatus.Completed ? 46 : 40;
        }
        return 40;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}