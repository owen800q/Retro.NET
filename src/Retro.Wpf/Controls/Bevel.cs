using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>
    /// A decorator that paints a background and one of the design system's crisp 1px bevel
    /// borders (see <see cref="BevelKind"/>). The border takes part in layout, so a
    /// <see cref="BevelKind.RaisedStrong"/> bevel reserves 2px on each side.
    /// </summary>
    public class Bevel : Decorator
    {
        static Bevel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Bevel), new FrameworkPropertyMetadata(typeof(Bevel)));
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(Bevel), new FrameworkPropertyMetadata(true));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(Bevel), new FrameworkPropertyMetadata(true));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
            nameof(Kind), typeof(BevelKind), typeof(Bevel),
            new FrameworkPropertyMetadata(BevelKind.Raised, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(
            typeof(Bevel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(Thickness), typeof(Bevel),
            new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty LightBrushProperty = DependencyProperty.Register(
            nameof(LightBrush), typeof(Brush), typeof(Bevel),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderLight), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DarkBrushProperty = DependencyProperty.Register(
            nameof(DarkBrush), typeof(Brush), typeof(Bevel),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderDark), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FlatBrushProperty = DependencyProperty.Register(
            nameof(FlatBrush), typeof(Brush), typeof(Bevel),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderMid), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty OutlineBrushProperty = DependencyProperty.Register(
            nameof(OutlineBrush), typeof(Brush), typeof(Bevel),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderBlack), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsBorderVisibleProperty = DependencyProperty.Register(
            nameof(IsBorderVisible), typeof(bool), typeof(Bevel),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DropShadowProperty = DependencyProperty.Register(
            nameof(DropShadow), typeof(double), typeof(Bevel),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DropShadowBrushProperty = DependencyProperty.Register(
            nameof(DropShadowBrush), typeof(Brush), typeof(Bevel),
            new FrameworkPropertyMetadata(RetroColors.Frozen(Color.FromArgb(0x40, 0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>The bevel style.</summary>
        public BevelKind Kind { get => (BevelKind)GetValue(KindProperty); set => SetValue(KindProperty, value); }

        /// <summary>Fill painted inside the outer outline.</summary>
        public Brush? Background { get => (Brush?)GetValue(BackgroundProperty); set => SetValue(BackgroundProperty, value); }

        /// <summary>Space between the bevel lines and the child.</summary>
        public Thickness Padding { get => (Thickness)GetValue(PaddingProperty); set => SetValue(PaddingProperty, value); }

        /// <summary>Highlight color (<c>--border-light</c>).</summary>
        public Brush? LightBrush { get => (Brush?)GetValue(LightBrushProperty); set => SetValue(LightBrushProperty, value); }

        /// <summary>Shadow color (<c>--border-dark</c>).</summary>
        public Brush? DarkBrush { get => (Brush?)GetValue(DarkBrushProperty); set => SetValue(DarkBrushProperty, value); }

        /// <summary>Line color of <see cref="BevelKind.Flat"/> (<c>--border-mid</c>).</summary>
        public Brush? FlatBrush { get => (Brush?)GetValue(FlatBrushProperty); set => SetValue(FlatBrushProperty, value); }

        /// <summary>Outer hairline color of the strong variants (<c>--border-black</c>).</summary>
        public Brush? OutlineBrush { get => (Brush?)GetValue(OutlineBrushProperty); set => SetValue(OutlineBrushProperty, value); }

        /// <summary>When false the border space is still reserved but nothing is drawn.</summary>
        public bool IsBorderVisible { get => (bool)GetValue(IsBorderVisibleProperty); set => SetValue(IsBorderVisibleProperty, value); }

        /// <summary>Offset of a hard (unblurred) drop shadow drawn outside the bounds, e.g. 2 for dialogs.</summary>
        public double DropShadow { get => (double)GetValue(DropShadowProperty); set => SetValue(DropShadowProperty, value); }

        public Brush? DropShadowBrush { get => (Brush?)GetValue(DropShadowBrushProperty); set => SetValue(DropShadowBrushProperty, value); }

        /// <summary>Thickness reserved by a bevel kind on every side.</summary>
        public static double GetBorderWidth(BevelKind kind)
        {
            switch (kind)
            {
                case BevelKind.None: return 0;
                case BevelKind.RaisedStrong:
                case BevelKind.SunkenStrong:
                case BevelKind.Fieldset: return 2;
                default: return 1;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double b = GetBorderWidth(Kind);
            Thickness p = Padding;
            double extraW = 2 * b + p.Left + p.Right;
            double extraH = 2 * b + p.Top + p.Bottom;
            UIElement? child = Child;
            if (child == null)
                return new Size(extraW, extraH);

            child.Measure(new Size(Math.Max(0, constraint.Width - extraW), Math.Max(0, constraint.Height - extraH)));
            return new Size(child.DesiredSize.Width + extraW, child.DesiredSize.Height + extraH);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElement? child = Child;
            if (child != null)
            {
                double b = GetBorderWidth(Kind);
                Thickness p = Padding;
                var rect = new Rect(
                    b + p.Left,
                    b + p.Top,
                    Math.Max(0, arrangeSize.Width - 2 * b - p.Left - p.Right),
                    Math.Max(0, arrangeSize.Height - 2 * b - p.Top - p.Bottom));
                child.Arrange(rect);
            }
            return arrangeSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            double w = RenderSize.Width;
            double h = RenderSize.Height;
            if (w <= 0 || h <= 0)
                return;

            if (DropShadow > 0 && DropShadowBrush != null)
                dc.DrawRectangle(DropShadowBrush, null, new Rect(DropShadow, DropShadow, w, h));

            BevelKind kind = Kind;
            bool strong = kind == BevelKind.RaisedStrong || kind == BevelKind.SunkenStrong;
            bool visible = IsBorderVisible;

            // Outer ring (strong outline, or the dark outer line of a fieldset).
            double o = 0;
            if (strong || kind == BevelKind.Fieldset)
            {
                if (visible)
                    Ring(dc, kind == BevelKind.Fieldset ? DarkBrush : OutlineBrush, 0, 0, w, h);
                o = 1;
            }

            if (Background != null)
                dc.DrawRectangle(Background, null, new Rect(o, o, Math.Max(0, w - 2 * o), Math.Max(0, h - 2 * o)));

            if (!visible)
                return;

            double iw = w - 2 * o;
            double ih = h - 2 * o;
            switch (kind)
            {
                case BevelKind.Raised:
                case BevelKind.RaisedStrong:
                    TwoTone(dc, LightBrush, DarkBrush, o, o, iw, ih);
                    break;
                case BevelKind.Sunken:
                case BevelKind.SunkenStrong:
                    TwoTone(dc, DarkBrush, LightBrush, o, o, iw, ih);
                    break;
                case BevelKind.Flat:
                    Ring(dc, FlatBrush, 0, 0, w, h);
                    break;
                case BevelKind.Fieldset:
                    Ring(dc, LightBrush, o, o, iw, ih);
                    break;
            }
        }

        /// <summary>Draws bottom/right with <paramref name="bottomRight"/>, then top/left on top of it (CSS shadow order).</summary>
        private static void TwoTone(DrawingContext dc, Brush? topLeft, Brush? bottomRight, double x, double y, double w, double h)
        {
            if (w < 1 || h < 1)
                return;
            if (bottomRight != null)
            {
                dc.DrawRectangle(bottomRight, null, new Rect(x, y + h - 1, w, 1));
                dc.DrawRectangle(bottomRight, null, new Rect(x + w - 1, y, 1, h));
            }
            if (topLeft != null)
            {
                dc.DrawRectangle(topLeft, null, new Rect(x, y, w, 1));
                dc.DrawRectangle(topLeft, null, new Rect(x, y, 1, h));
            }
        }

        private static void Ring(DrawingContext dc, Brush? brush, double x, double y, double w, double h)
        {
            if (brush == null || w < 1 || h < 1)
                return;
            dc.DrawRectangle(brush, null, new Rect(x, y, w, 1));
            dc.DrawRectangle(brush, null, new Rect(x, y + h - 1, w, 1));
            dc.DrawRectangle(brush, null, new Rect(x, y, 1, h));
            dc.DrawRectangle(brush, null, new Rect(x + w - 1, y, 1, h));
        }
    }
}
