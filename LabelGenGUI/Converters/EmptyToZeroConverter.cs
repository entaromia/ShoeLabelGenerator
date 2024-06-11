using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace LabelGenGUI.Converters
{
    /// <summary>
    /// Converts empty inputs to zero
    /// </summary>
    public class EmptyToZeroConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? 0M : System.Convert.ToDecimal(value);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? 0 : System.Convert.ToInt32(value);
    }
}
