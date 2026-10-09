using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Retro.Wpf.Controls
{
    /// <summary>Overlays a small red count bubble on the top-right corner of its content.</summary>
    public class Badge : ContentControl
    {
        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));
            FocusableProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
            nameof(Count), typeof(int), typeof(Badge), new FrameworkPropertyMetadata(0, OnChanged));

        public static readonly DependencyProperty MaxCountProperty = DependencyProperty.Register(
            nameof(MaxCount), typeof(int), typeof(Badge), new FrameworkPropertyMetadata(99, OnChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(Badge), new FrameworkPropertyMetadata(null, OnChanged));

        public static readonly DependencyProperty ShowZeroProperty = DependencyProperty.Register(
            nameof(ShowZero), typeof(bool), typeof(Badge), new FrameworkPropertyMetadata(false, OnChanged));

        private static readonly DependencyPropertyKey DisplayTextPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(DisplayText), typeof(string), typeof(Badge), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DisplayTextProperty = DisplayTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsBadgeVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsBadgeVisible), typeof(bool), typeof(Badge), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsBadgeVisibleProperty = IsBadgeVisiblePropertyKey.DependencyProperty;

        /// <summary>The number to show.</summary>
        public int Count { get => (int)GetValue(CountProperty); set => SetValue(CountProperty, value); }

        /// <summary>Counts above this render as "<c>MaxCount</c>+" (99+ by default).</summary>
        public int MaxCount { get => (int)GetValue(MaxCountProperty); set => SetValue(MaxCountProperty, value); }

        /// <summary>Free text shown instead of <see cref="Count"/> (e.g. "!" or "New").</summary>
        public string? Text { get => (string?)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        /// <summary>Show the bubble when <see cref="Count"/> is 0.</summary>
        public bool ShowZero { get => (bool)GetValue(ShowZeroProperty); set => SetValue(ShowZeroProperty, value); }

        public string DisplayText => (string)GetValue(DisplayTextProperty);
        public bool IsBadgeVisible => (bool)GetValue(IsBadgeVisibleProperty);

        /// <summary>Formats a count the way the badge displays it.</summary>
        public static string Format(int count, int maxCount) =>
            count > maxCount ? maxCount.ToString(CultureInfo.InvariantCulture) + "+" : count.ToString(CultureInfo.InvariantCulture);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Badge)d).Update();

        private void Update()
        {
            string? text = Text;
            if (!string.IsNullOrEmpty(text))
            {
                SetValue(DisplayTextPropertyKey, text);
                SetValue(IsBadgeVisiblePropertyKey, true);
                return;
            }
            SetValue(DisplayTextPropertyKey, Format(Count, MaxCount));
            SetValue(IsBadgeVisiblePropertyKey, Count > 0 || (Count == 0 && ShowZero));
        }
    }
}
