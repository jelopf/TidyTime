using System;
using System.Globalization;
using System.Collections.Generic;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace TidyTime.Converters;

public class MultiBoolToVisibilityConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count >= 2 && 
            values[0] is bool firstCondition && 
            values[1] is bool secondCondition)
        {
            return firstCondition && secondCondition;
        }
        return false;
    }
}