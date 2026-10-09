using System.Windows;
using System.Windows.Controls;

namespace Retro.Wpf.Controls
{
    /// <summary>
    /// A label-on-left form row: right-aligned "<c>Label *:</c>", the input, and gray help text
    /// (or red error text) below it. A non-empty <see cref="ErrorText"/> flags inputs inside
    /// via the inherited <see cref="Assist.IsErrorProperty"/>.
    /// </summary>
    public class FormField : ContentControl
    {
        static FormField()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormField), new FrameworkPropertyMetadata(typeof(FormField)));
            FocusableProperty.OverrideMetadata(typeof(FormField), new FrameworkPropertyMetadata(false));
            IsTabStopProperty.OverrideMetadata(typeof(FormField), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            nameof(Label), typeof(string), typeof(FormField), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register(
            nameof(IsRequired), typeof(bool), typeof(FormField), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HelpTextProperty = DependencyProperty.Register(
            nameof(HelpText), typeof(string), typeof(FormField), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register(
            nameof(ErrorText), typeof(string), typeof(FormField), new FrameworkPropertyMetadata(null, OnErrorTextChanged));

        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(
            nameof(LabelWidth), typeof(double), typeof(FormField), new FrameworkPropertyMetadata(80.0));

        public static readonly DependencyProperty ShowColonProperty = DependencyProperty.Register(
            nameof(ShowColon), typeof(bool), typeof(FormField), new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(HasError), typeof(bool), typeof(FormField), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        public string? Label { get => (string?)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        /// <summary>Appends a red asterisk to the label.</summary>
        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }

        public string? HelpText { get => (string?)GetValue(HelpTextProperty); set => SetValue(HelpTextProperty, value); }

        /// <summary>Error message; when set the label turns red and inputs get the error fill.</summary>
        public string? ErrorText { get => (string?)GetValue(ErrorTextProperty); set => SetValue(ErrorTextProperty, value); }

        /// <summary>Width of the label column (80 by default, matching the design's <c>80px 1fr</c> grid).</summary>
        public double LabelWidth { get => (double)GetValue(LabelWidthProperty); set => SetValue(LabelWidthProperty, value); }

        public bool ShowColon { get => (bool)GetValue(ShowColonProperty); set => SetValue(ShowColonProperty, value); }

        public bool HasError => (bool)GetValue(HasErrorProperty);

        private static void OnErrorTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool hasError = !string.IsNullOrEmpty((string?)e.NewValue);
            d.SetValue(HasErrorPropertyKey, hasError);
            if (hasError)
                d.SetValue(Assist.IsErrorProperty, true);
            else
                d.ClearValue(Assist.IsErrorProperty);
        }
    }
}
