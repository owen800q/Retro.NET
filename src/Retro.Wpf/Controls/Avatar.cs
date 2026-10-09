using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>The design system's only round element: initials or an image in a ringed circle.</summary>
    public class Avatar : Control
    {
        static Avatar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Avatar), new FrameworkPropertyMetadata(typeof(Avatar)));
            FocusableProperty.OverrideMetadata(typeof(Avatar), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty InitialsProperty = DependencyProperty.Register(
            nameof(Initials), typeof(string), typeof(Avatar), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            nameof(ImageSource), typeof(ImageSource), typeof(Avatar), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(double), typeof(Avatar), new FrameworkPropertyMetadata(32.0));

        public string? Initials { get => (string?)GetValue(InitialsProperty); set => SetValue(InitialsProperty, value); }
        public ImageSource? ImageSource { get => (ImageSource?)GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }

        /// <summary>Diameter of the circle inside the 1px outer ring (32 by default).</summary>
        public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
    }
}
