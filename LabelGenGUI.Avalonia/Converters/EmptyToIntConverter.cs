using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace LabelGenGUI.Avalonia.Converters;

/// <summary>
/// Converts empty string inputs to zero in order to avoid null integers
/// </summary>
public class EmptyToIntConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    => value?.ToString();

    // replace empty string input with '0'
    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    => value is string input && string.IsNullOrEmpty(input) ? 0 : System.Convert.ToInt32(value);
}
