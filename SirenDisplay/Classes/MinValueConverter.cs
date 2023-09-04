using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SirenDisplay.Classes;

public sealed class MinValueConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        double minWidth = Math.Min((double)values[0], (double)values[1]);
        return minWidth;
    }
}