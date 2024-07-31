using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace LabelGenGUI.Converters
{
    public class ShoeCountListToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IList list)
            {
                StringBuilder builder = new();
                int start = 38;
                foreach (int item in list)
                {
                    if (item != 0)
                    {
                        Trace.WriteLine("[ValueConverter] Not zero, appending to string");
                        builder.Append($"{start}: {item}, ");
                    }
                    start++;
                }
                builder.Length -= 2;
                return builder.ToString();
            }
            return "Not supported!";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
