using System.Windows;
using Retro.Wpf.Controls;

namespace Retro.Wpf
{
    /// <summary>Attached properties understood by the Retro control templates.</summary>
    public static class Assist
    {
        /// <summary>
        /// Icon shown before the content of a Button / ToggleButton / MenuItem, e.g.
        /// <c>&lt;Button retro:Assist.Icon="Save" Content="Save" /&gt;</c>.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            "Icon", typeof(IconKind?), typeof(Assist), new FrameworkPropertyMetadata(null));

        public static IconKind? GetIcon(DependencyObject d) => (IconKind?)d.GetValue(IconProperty);
        public static void SetIcon(DependencyObject d, IconKind? value) => d.SetValue(IconProperty, value);

        /// <summary>
        /// Puts an input into the error state (pink fill). Inherited, so setting it on a container
        /// (as <see cref="FormField"/> does) flags every input inside.
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.RegisterAttached(
            "IsError", typeof(bool), typeof(Assist),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetIsError(DependencyObject d) => (bool)d.GetValue(IsErrorProperty);
        public static void SetIsError(DependencyObject d, bool value) => d.SetValue(IsErrorProperty, value);

        /// <summary>Italic hint text shown in an empty TextBox / editable ComboBox / DatePicker.</summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached(
            "Placeholder", typeof(string), typeof(Assist), new FrameworkPropertyMetadata(null));

        public static string? GetPlaceholder(DependencyObject d) => (string?)d.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(DependencyObject d, string? value) => d.SetValue(PlaceholderProperty, value);
    }
}
