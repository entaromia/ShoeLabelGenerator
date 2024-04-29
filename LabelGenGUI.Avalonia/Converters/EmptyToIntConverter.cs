using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace LabelGenGUI.Avalonia.Converters;

/// <summary>
/// Converts empty or invalid string inputs to zero
/// </summary>
public class EmptyToIntConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    => value?.ToString();

    // replace empty or invalid input with '0'
    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
        => value is string input && int.TryParse(input, out int result) ? result : 0;
}
