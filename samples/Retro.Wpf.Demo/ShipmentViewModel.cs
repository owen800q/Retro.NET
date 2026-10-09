using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Retro.Wpf.Controls;

namespace Retro.Wpf.Demo
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public sealed class ShipmentItem : Observable
    {
        private bool _isSelected;

        public ShipmentItem(string doc, string material, int qty, string uom, decimal price, string status, TagTone tone)
        {
            Doc = doc;
            Material = material;
            Qty = qty;
            Uom = uom;
            Price = price;
            Status = status;
            StatusTone = tone;
        }

        public string Doc { get; }
        public string Material { get; }
        public int Qty { get; }
        public string Uom { get; }
        public decimal Price { get; }
        public decimal Amount => Qty * Price;
        public string Status { get; }
        public TagTone StatusTone { get; }

        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }
    }

    public sealed class ShipmentViewModel : Observable
    {
        private static readonly CultureInfo Us = CultureInfo.GetCultureInfo("en-US");

        private string _carrier = "DHL Freight";
        private string _service = "Standard";
        private string _route = "DE-HAM → FR-LYO";
        private string _pickup = "2026-03-17";
        private string _delivery = "2026-03-21";
        private string _priority = "Normal";
        private bool _express;
        private string _statusMessage = "Ready.";

        public ShipmentViewModel()
        {
            Items = new ObservableCollection<ShipmentItem>
            {
                new ShipmentItem("10000234", "Steel beam I-200", 12, "EA", 348.50m, "Open", TagTone.Info),
                new ShipmentItem("10000235", "Pallet hardwood", 24, "EA", 85.00m, "Open", TagTone.Info),
                new ShipmentItem("10000236", "Hydraulic fluid 20L", 8, "EA", 142.75m, "Hold", TagTone.Warning),
                new ShipmentItem("10000237", "Bolt M12×50 (box 100)", 50, "BX", 18.40m, "Open", TagTone.Info),
                new ShipmentItem("10000238", "Insulation foam roll", 6, "EA", 92.00m, "Error", TagTone.Error),
                new ShipmentItem("10000239", "Cable harness 12m", 4, "EA", 230.10m, "Open", TagTone.Info),
            };
            Items[2].IsSelected = true;
            foreach (ShipmentItem item in Items)
                item.PropertyChanged += (s, e) => OnPropertyChanged(nameof(SelectionSummary));
        }

        public string ShipmentId => "0000080014";

        public IReadOnlyList<string> Carriers { get; } = new[] { "DHL Freight", "UPS Ground", "FedEx Express", "Schenker", "Kuehne+Nagel" };
        public IReadOnlyList<string> Services { get; } = new[] { "Standard", "Express", "Economy" };
        public IReadOnlyList<string> Priorities { get; } = new[] { "Low", "Normal", "High", "Critical" };
        public IReadOnlyList<string> PaymentTerms { get; } = new[] { "Net 14", "Net 30", "Net 60", "Prepaid" };

        public ObservableCollection<ShipmentItem> Items { get; }

        public string Carrier { get => _carrier; set => Set(ref _carrier, value); }
        public string Service { get => _service; set => Set(ref _service, value); }
        public string Route { get => _route; set => Set(ref _route, value); }
        public string Pickup { get => _pickup; set => Set(ref _pickup, value); }
        public string Delivery { get => _delivery; set => Set(ref _delivery, value); }
        public string Priority { get => _priority; set => Set(ref _priority, value); }
        public string Payment { get; set; } = "Net 30";

        public bool Express
        {
            get => _express;
            set
            {
                if (Set(ref _express, value))
                    OnPropertyChanged(nameof(ExpressNote));
            }
        }

        public string ExpressNote => Express ? "On — surcharge applied" : "Off";

        public string StatusMessage { get => _statusMessage; set => Set(ref _statusMessage, value); }

        public string ItemsHeader => "Items (" + Items.Count.ToString(CultureInfo.InvariantCulture) + ")";

        public decimal TotalAmount => Items.Sum(i => i.Amount);

        public string SelectionSummary =>
            string.Format(Us, "Selected: {0} of {1} rows · Total amount: {2:N2} EUR",
                Items.Count(i => i.IsSelected), Items.Count, TotalAmount);
    }
}
