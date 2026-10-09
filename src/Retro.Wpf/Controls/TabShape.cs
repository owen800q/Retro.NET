using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>
    /// The angled-cut tab shape (<c>clip-path: polygon(4px 0, 100%-4px 0, 100% 100%, 0 100%)</c>)
    /// with a highlight on the top and left slant and a shadow on the right slant and bottom.
    /// </summary>
    public class TabShape : Decorator
    {
        static TabShape()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(TabShape), new FrameworkPropertyMetadata(true));
            RenderOptions.EdgeModeProperty.OverrideMetadata(typeof(TabShape), new FrameworkPropertyMetadata(EdgeMode.Aliased));
        }

        public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(
            typeof(TabShape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LightBrushProperty = DependencyProperty.Register(
            nameof(LightBrush), typeof(Brush), typeof(TabShape),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderLight), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DarkBrushProperty = DependencyProperty.Register(
            nameof(DarkBrush), typeof(Brush), typeof(TabShape),
            new FrameworkPropertyMetadata(RetroColors.Frozen(RetroColors.BorderDark), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SlantProperty = DependencyProperty.Register(
            nameof(Slant), typeof(double), typeof(TabShape),
            new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush? Background { get => (Brush?)GetValue(BackgroundProperty); set => SetValue(BackgroundProperty, value); }
        public Brush? LightBrush { get => (Brush?)GetValue(LightBrushProperty); set => SetValue(LightBrushProperty, value); }

        /// <summary>Right slant and bottom line; null for the selected tab, which merges into the strip.</summary>
        public Brush? DarkBrush { get => (Brush?)GetValue(DarkBrushProperty); set => SetValue(DarkBrushProperty, value); }

        /// <summary>Horizontal inset of the top corners.</summary>
        public double Slant { get => (double)GetValue(SlantProperty); set => SetValue(SlantProperty, value); }

        protected override void OnRender(DrawingContext dc)
        {
            double w = RenderSize.Width, h = RenderSize.Height, s = Math.Min(Slant, w / 2);
            if (w <= 0 || h <= 0)
                return;

            var g = new StreamGeometry();
            using (StreamGeometryContext c = g.Open())
            {
                c.BeginFigure(new Point(s, 0), true, true);
                c.LineTo(new Point(w - s, 0), false, false);
                c.LineTo(new Point(w, h), false, false);
                c.LineTo(new Point(0, h), false, false);
            }
            g.Freeze();
            dc.DrawGeometry(Background, null, g);

            // Pixel lines: offset by .5 so 1px pens land on whole device pixels.
            if (DarkBrush != null)
            {
                var dark = new Pen(DarkBrush, 1);
                dc.DrawLine(dark, new Point(w - s - 0.5, 0.5), new Point(w - 0.5, h));
                dc.DrawRectangle(DarkBrush, null, new Rect(0, h - 1, w, 1));
            }
            if (LightBrush != null)
            {
                var light = new Pen(LightBrush, 1);
                dc.DrawRectangle(LightBrush, null, new Rect(s, 0, Math.Max(0, w - 2 * s), 1));
                dc.DrawLine(light, new Point(s + 0.5, 0.5), new Point(0.5, h));
            }
        }
    }
}
