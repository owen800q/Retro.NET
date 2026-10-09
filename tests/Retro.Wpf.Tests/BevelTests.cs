using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Retro.Wpf.Controls;
using Xunit;

namespace Retro.Wpf.Tests
{
    public class BevelTests
    {
        private static readonly Color White = Ui.Hex("#FFFFFF");
        private static readonly Color Dark = Ui.Hex("#6E7A89");
        private static readonly Color Mid = Ui.Hex("#A6B4C5");
        private static readonly Color Black = Ui.Hex("#000000");
        private static readonly Color Fill = Ui.Hex("#EAF1F8");

        private static System.Windows.Media.Imaging.BitmapSource Draw(BevelKind kind, bool visible = true) =>
            Ui.Render(new Bevel { Kind = kind, Width = 20, Height = 10, IsBorderVisible = visible, Background = new SolidColorBrush(Fill) },
                20, 10, "bevel-" + kind + (visible ? "" : "-hidden"));

        [Fact]
        public void Raised_is_light_top_left_and_dark_bottom_right()
        {
            Ui.Run(() =>
            {
                var b = Draw(BevelKind.Raised);
                Assert.Equal(White, Ui.Pixel(b, 0, 0));
                Assert.Equal(White, Ui.Pixel(b, 0, 9));   // left column wins the corner (CSS shadow order)
                Assert.Equal(Dark, Ui.Pixel(b, 19, 9));
                Assert.Equal(Dark, Ui.Pixel(b, 19, 5));
                Assert.Equal(Fill, Ui.Pixel(b, 10, 5));
            });
        }

        [Fact]
        public void RaisedStrong_adds_a_black_outline()
        {
            Ui.Run(() =>
            {
                var b = Draw(BevelKind.RaisedStrong);
                Assert.Equal(Black, Ui.Pixel(b, 0, 0));
                Assert.Equal(Black, Ui.Pixel(b, 19, 9));
                Assert.Equal(White, Ui.Pixel(b, 1, 1));
                Assert.Equal(Dark, Ui.Pixel(b, 18, 8));
                Assert.Equal(Fill, Ui.Pixel(b, 10, 5));
            });
        }

        [Fact]
        public void Sunken_is_dark_top_left_and_light_bottom_right()
        {
            Ui.Run(() =>
            {
                var b = Draw(BevelKind.Sunken);
                Assert.Equal(Dark, Ui.Pixel(b, 0, 0));
                Assert.Equal(White, Ui.Pixel(b, 19, 9));
                Assert.Equal(White, Ui.Pixel(b, 19, 5));
            });
        }

        [Fact]
        public void SunkenStrong_has_outline_then_dark_then_light()
        {
            Ui.Run(() =>
            {
                var b = Draw(BevelKind.SunkenStrong);
                Assert.Equal(Black, Ui.Pixel(b, 0, 0));
                Assert.Equal(Dark, Ui.Pixel(b, 1, 1));
                Assert.Equal(White, Ui.Pixel(b, 18, 8));
            });
        }

        [Fact]
        public void Flat_and_fieldset()
        {
            Ui.Run(() =>
            {
                var flat = Draw(BevelKind.Flat);
                Assert.Equal(Mid, Ui.Pixel(flat, 0, 0));
                Assert.Equal(Mid, Ui.Pixel(flat, 19, 9));
                Assert.Equal(Fill, Ui.Pixel(flat, 1, 1));

                var fs = Draw(BevelKind.Fieldset);
                Assert.Equal(Dark, Ui.Pixel(fs, 0, 0));
                Assert.Equal(White, Ui.Pixel(fs, 1, 1));
                Assert.Equal(White, Ui.Pixel(fs, 18, 8));
                Assert.Equal(Dark, Ui.Pixel(fs, 19, 9));
            });
        }

        [Fact]
        public void Hidden_border_keeps_its_space_but_draws_nothing()
        {
            Ui.Run(() =>
            {
                var b = Draw(BevelKind.Raised, visible: false);
                Assert.Equal(Fill, Ui.Pixel(b, 0, 0));
                Assert.Equal(Fill, Ui.Pixel(b, 19, 9));

                var bevel = new Bevel { Kind = BevelKind.Raised, IsBorderVisible = false, Child = new Border { Width = 10, Height = 10 } };
                bevel.Measure(new Size(100, 100));
                Assert.Equal(new Size(12, 12), bevel.DesiredSize);
            });
        }

        [Theory]
        [InlineData(BevelKind.None, 0)]
        [InlineData(BevelKind.Raised, 1)]
        [InlineData(BevelKind.Sunken, 1)]
        [InlineData(BevelKind.Flat, 1)]
        [InlineData(BevelKind.RaisedStrong, 2)]
        [InlineData(BevelKind.SunkenStrong, 2)]
        [InlineData(BevelKind.Fieldset, 2)]
        public void Border_takes_part_in_layout(BevelKind kind, double width)
        {
            Ui.Run(() =>
            {
                Assert.Equal(width, Bevel.GetBorderWidth(kind));
                var bevel = new Bevel { Kind = kind, Padding = new Thickness(1, 2, 3, 4), Child = new Border { Width = 10, Height = 10 } };
                bevel.Measure(new Size(100, 100));
                Assert.Equal(new Size(10 + 4 + 2 * width, 10 + 6 + 2 * width), bevel.DesiredSize);
            });
        }
    }
}
