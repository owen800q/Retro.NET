using System.Windows;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>Displays one of the design-system icons.</summary>
    public class RetroIcon : FrameworkElement
    {
        static RetroIcon()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(RetroIcon), new FrameworkPropertyMetadata(true));
            FocusableProperty.OverrideMetadata(typeof(RetroIcon), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
            nameof(Kind), typeof(IconKind), typeof(RetroIcon),
            new FrameworkPropertyMetadata(IconKind.Document, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(double), typeof(RetroIcon),
            new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(IconKind?), typeof(RetroIcon),
            new FrameworkPropertyMetadata(null, (d, e) =>
            {
                if (e.NewValue is IconKind kind)
                    d.SetCurrentValue(KindProperty, kind);
            }));

        public IconKind Kind { get => (IconKind)GetValue(KindProperty); set => SetValue(KindProperty, value); }

        /// <summary>
        /// Nullable alias of <see cref="Kind"/> for templates: <c>Icon="{TemplateBinding retro:Assist.Icon}"</c>.
        /// A null value leaves <see cref="Kind"/> unchanged (templates collapse the icon instead).
        /// </summary>
        public IconKind? Icon { get => (IconKind?)GetValue(IconProperty); set => SetValue(IconProperty, value); }

        /// <summary>Edge length in DIPs (16 by default; 32 for dialogs, 14 for menus and title bars).</summary>
        public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) => new System.Windows.Size(Size, Size);

        protected override void OnRender(DrawingContext dc)
        {
            double s = Size;
            double x = (RenderSize.Width - s) / 2;
            double y = (RenderSize.Height - s) / 2;
            dc.DrawImage(RetroIcons.Get(Kind), new Rect(System.Math.Round(x), System.Math.Round(y), s, s));
        }
    }
}
