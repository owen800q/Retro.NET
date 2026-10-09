using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Retro.Wpf.Controls;

namespace Retro.Wpf.Demo
{
    public partial class GalleryView : UserControl
    {
        private static readonly string[] SwatchKeys =
        {
            "Surface", "Surface2", "Surface3", "HeaderBlue1", "HeaderBlue2", "HeaderGloss",
            "Accent", "AccentSoft", "AccentPale", "BorderMid", "BorderDark", "ForegroundMuted",
            "Link", "StatusError", "StatusWarning", "StatusInfo", "StatusSuccess", "ForegroundDisabled",
        };

        public GalleryView()
        {
            InitializeComponent();

            SampleGrid.ItemsSource = new[]
            {
                new { Doc = "6000068250", Carrier = "DHL Freight", Status = "Open", Tone = TagTone.Info, Amount = "14,553.21" },
                new { Doc = "6000068251", Carrier = "UPS Ground", Status = "Hold", Tone = TagTone.Warning, Amount = "2,841.00" },
                new { Doc = "6000068252", Carrier = "FedEx Express", Status = "Error", Tone = TagTone.Error, Amount = "9,120.45" },
                new { Doc = "6000068253", Carrier = "Schenker", Status = "Open", Tone = TagTone.Info, Amount = "655.10" },
                new { Doc = "6000068254", Carrier = "Kuehne+Nagel", Status = "Open", Tone = TagTone.Info, Amount = "23,990.00" },
            };
            SampleGrid.SelectedIndex = 2;

            SampleCalendar.DisplayDate = new DateTime(2026, 3, 1);
            SampleCalendar.SelectedDate = new DateTime(2026, 3, 17);

            Icons.ItemsSource = Enum.GetValues(typeof(IconKind)).Cast<IconKind>().ToList();

            Swatches.ItemsSource = SwatchKeys.Select(k =>
            {
                var brush = TryFindResource("Retro." + k) as SolidColorBrush;
                string hex = brush == null ? "" : "#" + brush.Color.ToString().Substring(3);
                return new { Name = k, Brush = (Brush?)brush, Hex = hex };
            }).ToList();
        }

        private void OnConfirm(object sender, RoutedEventArgs e) =>
            RetroMessageBox.Show(Window.GetWindow(this), "Delete document 6000068252? This action cannot be undone.",
                "Confirm Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

        private void OnError(object sender, RoutedEventArgs e) =>
            RetroMessageBox.Show(Window.GetWindow(this), "Field \"Carrier\" is required. Enter a value before saving.",
                "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

        private void OnQuestion(object sender, RoutedEventArgs e) =>
            RetroMessageBox.Show(Window.GetWindow(this), "Save changes to shipment 0000080014 before leaving?",
                "Exit Transaction", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
    }
}
