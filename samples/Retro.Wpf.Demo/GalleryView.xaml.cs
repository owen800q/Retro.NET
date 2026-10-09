using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Retro.Wpf.Controls;

namespace Retro.Wpf.Demo
{
    public sealed class SampleRow
    {
        public SampleRow(string doc, string carrier, string status, TagTone tone, string amount)
        {
            Doc = doc;
            Carrier = carrier;
            Status = status;
            Tone = tone;
            Amount = amount;
        }

        public string Doc { get; }
        public string Carrier { get; }
        public string Status { get; }
        public TagTone Tone { get; }
        public string Amount { get; }
    }

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
                new SampleRow("6000068250", "DHL Freight", "Open", TagTone.Info, "14,553.21"),
                new SampleRow("6000068251", "UPS Ground", "Hold", TagTone.Warning, "2,841.00"),
                new SampleRow("6000068252", "FedEx Express", "Error", TagTone.Error, "9,120.45"),
                new SampleRow("6000068253", "Schenker", "Open", TagTone.Info, "655.10"),
                new SampleRow("6000068254", "Kuehne+Nagel", "Open", TagTone.Info, "23,990.00"),
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
