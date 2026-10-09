using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Retro.Wpf.Controls
{
    /// <summary>Path navigation (<c>Home › Shipment › Create</c>). The last item is the current location.</summary>
    public class Breadcrumb : ItemsControl
    {
        static Breadcrumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(typeof(Breadcrumb)));
            FocusableProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(false));
        }

        public Breadcrumb()
        {
            ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                    UpdatePositions();
            };
        }

        public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent(
            nameof(ItemClick), RoutingStrategy.Bubble, typeof(EventHandler<BreadcrumbItemClickEventArgs>), typeof(Breadcrumb));

        /// <summary>Raised when a (non-current) item is clicked.</summary>
        public event EventHandler<BreadcrumbItemClickEventArgs> ItemClick { add => AddHandler(ItemClickEvent, value); remove => RemoveHandler(ItemClickEvent, value); }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is BreadcrumbItem;

        protected override DependencyObject GetContainerForItemOverride() => new BreadcrumbItem();

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            UpdatePositions();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            UpdatePositions();
        }

        internal void UpdatePositions()
        {
            int count = Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is BreadcrumbItem c)
                {
                    c.SetValue(BreadcrumbItem.IsFirstPropertyKey, i == 0);
                    c.SetValue(BreadcrumbItem.IsCurrentPropertyKey, i == count - 1);
                }
            }
        }

        internal void OnItemClicked(BreadcrumbItem container)
        {
            object item = ItemContainerGenerator.ItemFromContainer(container);
            if (item == DependencyProperty.UnsetValue)
                item = container;
            RaiseEvent(new BreadcrumbItemClickEventArgs(ItemClickEvent, this, item));
        }
    }

    /// <summary>Event data for <see cref="Breadcrumb.ItemClick"/>.</summary>
    public class BreadcrumbItemClickEventArgs : RoutedEventArgs
    {
        public BreadcrumbItemClickEventArgs(RoutedEvent routedEvent, object source, object item) : base(routedEvent, source)
        {
            Item = item;
        }

        /// <summary>The clicked data item (or the <see cref="BreadcrumbItem"/> itself).</summary>
        public object Item { get; }
    }

    /// <summary>One segment of a <see cref="Breadcrumb"/>: a link, preceded by a › separator.</summary>
    public class BreadcrumbItem : ButtonBase
    {
        static BreadcrumbItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbItem), new FrameworkPropertyMetadata(typeof(BreadcrumbItem)));
        }

        internal static readonly DependencyPropertyKey IsFirstPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsFirst), typeof(bool), typeof(BreadcrumbItem), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsFirstProperty = IsFirstPropertyKey.DependencyProperty;

        internal static readonly DependencyPropertyKey IsCurrentPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsCurrent), typeof(bool), typeof(BreadcrumbItem), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsCurrentProperty = IsCurrentPropertyKey.DependencyProperty;

        /// <summary>True for the first segment (no separator).</summary>
        public bool IsFirst => (bool)GetValue(IsFirstProperty);

        /// <summary>True for the last segment (plain text, not a link).</summary>
        public bool IsCurrent => (bool)GetValue(IsCurrentProperty);

        internal void PerformClick() => OnClick();

        protected override void OnClick()
        {
            if (IsCurrent)
                return;
            base.OnClick();
            (ItemsControl.ItemsControlFromItemContainer(this) as Breadcrumb)?.OnItemClicked(this);
        }
    }
}
