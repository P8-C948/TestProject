using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace TestProject.GUI.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public static readonly BoolToColorConverter Instance = new();

        // Convert boolean sensor/connection states to IBrush for UI (true -> green, false -> red)
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool b) flag = b;
            else if (value is string s && bool.TryParse(s, out var parsed)) flag = parsed;

            return flag ? (IBrush)new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Not needed for one-way bindings; throw or attempt best-effort
            throw new NotSupportedException();
        }
    }
}