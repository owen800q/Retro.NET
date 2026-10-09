using System.Windows;
using System.Windows.Controls;
using Retro.Wpf.Controls;

namespace Retro.Wpf.Demo
{
    public partial class ShipmentView : UserControl
    {
        public ShipmentView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public ShipmentViewModel ViewModel { get; } = new ShipmentViewModel();

        private void Status(string message) => ViewModel.StatusMessage = message;

        private void OnNew(object sender, RoutedEventArgs e) => Status("New shipment template loaded.");
        private void OnOpen(object sender, RoutedEventArgs e) => Status("Open document…");
        private void OnSave(object sender, RoutedEventArgs e) => Status("Document saved successfully — 0000080014");
        private void OnPrint(object sender, RoutedEventArgs e) => Status("Sent to printer LP-WAREHOUSE-01");
        private void OnFind(object sender, RoutedEventArgs e) => Status("Find: open dialog…");
        private void OnExecute(object sender, RoutedEventArgs e) => Status("F8 — execute transaction");
        private void OnRefresh(object sender, RoutedEventArgs e) => Status("Refreshed.");
        private void OnTable(object sender, RoutedEventArgs e) => Status("Table settings opened.");
        private void OnFilter(object sender, RoutedEventArgs e) => Status("Filter applied: status=Open");
        private void OnSort(object sender, RoutedEventArgs e) => Status("Sorted by Document");
        private void OnExport(object sender, RoutedEventArgs e) => Status("Exported to spreadsheet.");
        private void OnBack(object sender, RoutedEventArgs e) => Status("Returned to selection screen.");
        private void OnCancel(object sender, RoutedEventArgs e) => Status("Cancel — changes discarded.");
        private void OnAttach(object sender, RoutedEventArgs e) => Status("Attach: choose a file…");
        private void OnChangeMode(object sender, RoutedEventArgs e) => Status("Switched to Change mode.");
        private void OnExit(object sender, RoutedEventArgs e) => Window.GetWindow(this)?.Close();

        private void OnCrumb(object sender, BreadcrumbItemClickEventArgs e) =>
            Status("Navigate: " + ((e.Item as ContentControl)?.Content ?? e.Item));

        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReferenceEquals(e.OriginalSource, Tabs))
                Summary.Visibility = Tabs.SelectedIndex == 1 ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = RetroMessageBox.Show(Window.GetWindow(this),
                "Delete shipment 0000080014? This action cannot be undone.",
                "Confirm Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (r == MessageBoxResult.OK)
                Status("Document 0000080014 deleted.");
        }

        private void OnHelp(object sender, RoutedEventArgs e)
        {
            RetroMessageBox.Show(Window.GetWindow(this),
                "Transaction VL01N\n\nMaintain outbound shipment headers, items, and attached documents. " +
                "Press F8 to execute, Ctrl+S to save, Shift+F2 to delete.",
                "Help — Shipment Maintenance", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnGallery(object sender, RoutedEventArgs e)
        {
            new GalleryWindow { Owner = Window.GetWindow(this) }.Show();
        }
    }
}
