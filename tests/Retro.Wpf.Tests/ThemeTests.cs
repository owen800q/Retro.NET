using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Retro.Wpf.Controls;
using Xunit;

namespace Retro.Wpf.Tests
{
    public class ThemeTests
    {
        [Theory]
        [InlineData("Retro.Surface", "#D6E4F1")]
        [InlineData("Retro.Surface2", "#EAF1F8")]
        [InlineData("Retro.Surface3", "#F5F8FB")]
        [InlineData("Retro.SurfaceInset", "#FFFFFF")]
        [InlineData("Retro.HeaderBlue1", "#003D7A")]
        [InlineData("Retro.HeaderBlue2", "#2A6FB8")]
        [InlineData("Retro.HeaderGloss", "#6FA3D6")]
        [InlineData("Retro.Accent", "#F0AB00")]
        [InlineData("Retro.AccentSoft", "#FFE8A8")]
        [InlineData("Retro.AccentPale", "#FFF6D9")]
        [InlineData("Retro.BorderLight", "#FFFFFF")]
        [InlineData("Retro.BorderMid", "#A6B4C5")]
        [InlineData("Retro.BorderDark", "#6E7A89")]
        [InlineData("Retro.ForegroundMuted", "#4A5566")]
        [InlineData("Retro.ForegroundDisabled", "#8A95A5")]
        [InlineData("Retro.Link", "#1F6BB8")]
        [InlineData("Retro.LinkVisited", "#6B3F8A")]
        [InlineData("Retro.LinkHover", "#C8281E")]
        [InlineData("Retro.StatusError", "#C8281E")]
        [InlineData("Retro.StatusWarning", "#E8A100")]
        [InlineData("Retro.StatusInfo", "#1F6BB8")]
        [InlineData("Retro.StatusSuccess", "#2E8B3E")]
        [InlineData("Retro.InputFocusBackground", "#FFFFE1")]
        public void Brush_tokens_match_the_design(string key, string hex)
        {
            Ui.Run(() =>
            {
                var brush = Assert.IsType<SolidColorBrush>(Application.Current.FindResource(key));
                Assert.Equal(Ui.Hex(hex), brush.Color);
            });
        }

        [Fact]
        public void RetroColors_constants_match_the_resource_dictionary()
        {
            Ui.Run(() =>
            {
                Assert.Equal(RetroColors.Surface, ((SolidColorBrush)Application.Current.FindResource("Retro.Surface")).Color);
                Assert.Equal(RetroColors.Accent, ((SolidColorBrush)Application.Current.FindResource("Retro.Accent")).Color);
                Assert.Equal(RetroColors.BorderDark, ((SolidColorBrush)Application.Current.FindResource("Retro.BorderDark")).Color);
            });
        }

        [Fact]
        public void Type_scale_and_font_stack_are_defined()
        {
            Ui.Run(() =>
            {
                Assert.Equal(10.0, Application.Current.FindResource("Retro.FontSize.XS"));
                Assert.Equal(11.0, Application.Current.FindResource("Retro.FontSize.SM"));
                Assert.Equal(20.0, Application.Current.FindResource("Retro.FontSize.3XL"));
                var family = Assert.IsType<FontFamily>(Application.Current.FindResource("Retro.FontFamily"));
                Assert.StartsWith("Microsoft Sans Serif", family.Source);
            });
        }

        [Fact]
        public void Every_resource_in_the_theme_can_be_instantiated()
        {
            Ui.Run(() =>
            {
                int count = 0;
                Visit(new RetroTheme(), ref count);
                Assert.True(count > 150, "expected a full theme, found " + count + " resources");
            });
        }

        [Fact]
        public void Generic_dictionary_loads()
        {
            Ui.Run(() =>
            {
                var generic = (ResourceDictionary)Application.LoadComponent(new Uri("/Retro.Wpf;component/Themes/Generic.xaml", UriKind.Relative));
                int count = 0;
                Visit(generic, ref count);
                Assert.True(generic.Contains(typeof(RetroWindow)));
                Assert.True(generic.Contains(typeof(Pagination)));
            });
        }

        [Fact]
        public void All_icons_resolve_to_16px_images()
        {
            Ui.Run(() =>
            {
                var kinds = Enum.GetValues(typeof(IconKind)).Cast<IconKind>().ToList();
                Assert.Equal(44, kinds.Count);
                foreach (IconKind kind in kinds)
                {
                    DrawingImage image = RetroIcons.Get(kind);
                    Assert.Equal(16, image.Width, 3);
                    Assert.Equal(16, image.Height, 3);
                    Assert.True(image.IsFrozen);
                    Assert.IsType<DrawingImage>(Application.Current.FindResource(RetroIcons.GetResourceKey(kind)));
                }
                Assert.Same(RetroIcons.Get(IconKind.Save), new IconExtension(IconKind.Save).ProvideValue(null!));
            });
        }

        private static void Visit(ResourceDictionary dictionary, ref int count)
        {
            foreach (ResourceDictionary merged in dictionary.MergedDictionaries)
                Visit(merged, ref count);
            foreach (DictionaryEntry entry in dictionary)
            {
                Assert.NotNull(entry.Value);
                count++;
            }
        }
    }
}
