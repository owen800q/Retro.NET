using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Retro.Wpf
{
    /// <summary>Passes a nullable value through, but leaves the target untouched when it is null.</summary>
    internal sealed class NullSkipConverter : IValueConverter
    {
        public static readonly NullSkipConverter Instance = new NullSkipConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value ?? Binding.DoNothing;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value ?? Binding.DoNothing;
    }

    /// <summary>Visible when the value is a non-empty string / non-null object, otherwise Collapsed.</summary>
    internal sealed class NotEmptyToVisibilityConverter : IValueConverter
    {
        public static readonly NotEmptyToVisibilityConverter Instance = new NotEmptyToVisibilityConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool empty = value == null || (value is string s && s.Length == 0);
            return empty ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
