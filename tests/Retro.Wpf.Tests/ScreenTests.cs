using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Retro.Wpf.Controls;
using Retro.Wpf.Demo;
using Xunit;

namespace Retro.Wpf.Tests
{
    /// <summary>Renders the full demo screens (the design's UI kit) to PNG for visual review.</summary>
    public class ScreenTests
    {
        [Fact]
        public void Shipment_screen_renders_every_tab()
        {
            Ui.Run(() =>
            {
                Ui.BindingErrors.Clear();
                var view = new ShipmentView();
                var tabs = Ui.Descendants<TabControl>(view).FirstOrDefault();
                string[] names = { "header", "items", "documents" };
                for (int i = 0; i < names.Length; i++)
                {
                    var bmp = Ui.Render(view, 1090, 660, "screen-shipment-" + names[i]);
                    tabs ??= Ui.Descendants<TabControl>(view).First();
                    if (i + 1 < names.Length)
                        tabs.SelectedIndex = i + 1;
                }
                Assert.Equal("Items (6)", view.ViewModel.ItemsHeader);
                Assert.Empty(Ui.BindingErrors.Messages);
            });
        }

        [Fact]
        public void Gallery_renders()
        {
            Ui.Run(() =>
            {
                Ui.BindingErrors.Clear();
                var view = new GalleryView();
                var bmp = Ui.Render(view, 1160, double.NaN, "screen-gallery");
                Assert.True(bmp.PixelHeight > 1200);
                Assert.Empty(Ui.BindingErrors.Messages);
            });
        }

        [Fact]
        public void Main_window_renders_with_retro_chrome()
        {
            Ui.Run(() =>
            {
                var window = new MainWindow { Width = 1100, Height = 720 };
                var bmp = Ui.RenderWindow(window, "window-main");
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(bmp, 0, 0));
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(bmp, 1, 1));
                var title = Ui.Pixel(bmp, bmp.PixelWidth / 2, 3);
                Assert.True(title.B > title.R + 60, "title bar should be blue, got " + title);
            });
        }

        [Fact]
        public void Message_box_window_renders()
        {
            Ui.Run(() =>
            {
                var d = new RetroDialog("Delete shipment 0000080014? This action cannot be undone.", "Confirm Delete",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                d.WindowStartupLocation = WindowStartupLocation.Manual;
                var bmp = Ui.RenderWindow(d, "window-dialog");
                Assert.True(bmp.PixelWidth >= 320);
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(bmp, 0, 0));
            });
        }
    }
}
