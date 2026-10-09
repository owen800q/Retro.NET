using System.Windows;
using System.Windows.Controls;

namespace Retro.Wpf.Controls
{
    /// <summary>Placeholder for empty lists: icon well, bold title, muted description and an optional action.</summary>
    public class EmptyState : Control
    {
        static EmptyState()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyState), new FrameworkPropertyMetadata(typeof(EmptyState)));
            FocusableProperty.OverrideMetadata(typeof(EmptyState), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(IconKind), typeof(EmptyState), new FrameworkPropertyMetadata(IconKind.Document));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(EmptyState), new FrameworkPropertyMetadata("No Data Available"));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(EmptyState), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            nameof(Action), typeof(object), typeof(EmptyState), new FrameworkPropertyMetadata(null));

        public IconKind Icon { get => (IconKind)GetValue(IconProperty); set => SetValue(IconProperty, value); }
        public string? Title { get => (string?)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public string? Description { get => (string?)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }

        /// <summary>Optional content under the description, typically a Button.</summary>
        public object? Action { get => GetValue(ActionProperty); set => SetValue(ActionProperty, value); }
    }
}
