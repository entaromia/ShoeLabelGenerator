using System.Globalization;
using System.Windows.Data;

namespace LabelGenGUI
{
    /// <summary>
    /// Converts empty string inputs to zero in order to avoid null integers
    /// </summary>
    public class EmptyToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value;

        // replace empty string input with '0'
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is string input && string.IsNullOrEmpty(input) ? 0 : value;
    }
}
