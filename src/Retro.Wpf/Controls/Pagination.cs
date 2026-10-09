using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Retro.Wpf.Controls
{
    /// <summary>Kind of a <see cref="PageEntry"/>.</summary>
    public enum PageEntryKind { Previous, Page, Ellipsis, Next }

    /// <summary>One button of a <see cref="Pagination"/> strip.</summary>
    public sealed class PageEntry
    {
        public PageEntry(PageEntryKind kind, int? page, string label, bool isCurrent, bool isEnabled)
        {
            Kind = kind;
            Page = page;
            Label = label;
            IsCurrent = isCurrent;
            IsEnabled = isEnabled;
        }

        public PageEntryKind Kind { get; }

        /// <summary>Target page (1-based); null for an ellipsis.</summary>
        public int? Page { get; }

        public string Label { get; }
        public bool IsCurrent { get; }
        public bool IsEnabled { get; }

        public override string ToString() => Label;
    }

    /// <summary>Page navigation: <c>‹ 1 … 4 5 6 … 20 ›</c>, current page in amber.</summary>
    public class Pagination : Control
    {
        /// <summary>Navigates to the page given as the command parameter.</summary>
        public static readonly RoutedCommand GoToPageCommand = new RoutedCommand(nameof(GoToPageCommand), typeof(Pagination));

        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));
            FocusableProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(false));
            CommandManager.RegisterClassCommandBinding(typeof(Pagination), new CommandBinding(GoToPageCommand, OnGoToPage, OnCanGoToPage));
        }

        public Pagination()
        {
            Rebuild();
        }

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
            nameof(PageCount), typeof(int), typeof(Pagination),
            new FrameworkPropertyMetadata(1, OnPageCountChanged, (d, v) => Math.Max(1, (int)v)));

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            nameof(CurrentPage), typeof(int), typeof(Pagination),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentPageChanged, CoerceCurrentPage));

        public static readonly DependencyProperty MaxVisiblePagesProperty = DependencyProperty.Register(
            nameof(MaxVisiblePages), typeof(int), typeof(Pagination),
            new FrameworkPropertyMetadata(7, (d, e) => ((Pagination)d).Rebuild(), (d, v) => Math.Max(5, (int)v)));

        private static readonly DependencyPropertyKey PagesPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Pages), typeof(IReadOnlyList<PageEntry>), typeof(Pagination), new FrameworkPropertyMetadata(Array.Empty<PageEntry>()));

        public static readonly DependencyProperty PagesProperty = PagesPropertyKey.DependencyProperty;

        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(PageChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(Pagination));

        public int PageCount { get => (int)GetValue(PageCountProperty); set => SetValue(PageCountProperty, value); }

        /// <summary>The selected page, 1-based; coerced into [1, PageCount].</summary>
        public int CurrentPage { get => (int)GetValue(CurrentPageProperty); set => SetValue(CurrentPageProperty, value); }

        /// <summary>Maximum number of page/ellipsis buttons between ‹ and › (minimum 5).</summary>
        public int MaxVisiblePages { get => (int)GetValue(MaxVisiblePagesProperty); set => SetValue(MaxVisiblePagesProperty, value); }

        /// <summary>The buttons currently displayed, including ‹ and ›.</summary>
        public IReadOnlyList<PageEntry> Pages => (IReadOnlyList<PageEntry>)GetValue(PagesProperty);

        public event RoutedPropertyChangedEventHandler<int> PageChanged { add => AddHandler(PageChangedEvent, value); remove => RemoveHandler(PageChangedEvent, value); }

        /// <summary>Computes the button strip for the given state.</summary>
        public static IReadOnlyList<PageEntry> BuildPages(int currentPage, int pageCount, int maxVisiblePages)
        {
            pageCount = Math.Max(1, pageCount);
            maxVisiblePages = Math.Max(5, maxVisiblePages);
            int current = Math.Min(Math.Max(1, currentPage), pageCount);

            var list = new List<PageEntry>
            {
                new PageEntry(PageEntryKind.Previous, current - 1, "‹", false, current > 1),
            };

            void Page(int p) => list.Add(new PageEntry(PageEntryKind.Page, p, p.ToString(CultureInfo.InvariantCulture), p == current, true));
            void Ellipsis() => list.Add(new PageEntry(PageEntryKind.Ellipsis, null, "…", false, false));

            if (pageCount <= maxVisiblePages)
            {
                for (int p = 1; p <= pageCount; p++) Page(p);
            }
            else if (current <= maxVisiblePages - 3)
            {
                for (int p = 1; p <= maxVisiblePages - 2; p++) Page(p);
                Ellipsis();
                Page(pageCount);
            }
            else if (current >= pageCount - (maxVisiblePages - 4))
            {
                Page(1);
                Ellipsis();
                for (int p = pageCount - (maxVisiblePages - 3); p <= pageCount; p++) Page(p);
            }
            else
            {
                int window = maxVisiblePages - 4;
                int start = current - (window - 1) / 2;
                Page(1);
                Ellipsis();
                for (int p = start; p < start + window; p++) Page(p);
                Ellipsis();
                Page(pageCount);
            }

            list.Add(new PageEntry(PageEntryKind.Next, current + 1, "›", false, current < pageCount));
            return list;
        }

        private static object CoerceCurrentPage(DependencyObject d, object value)
        {
            var p = (Pagination)d;
            return Math.Min(Math.Max(1, (int)value), p.PageCount);
        }

        private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = (Pagination)d;
            p.CoerceValue(CurrentPageProperty);
            p.Rebuild();
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = (Pagination)d;
            p.Rebuild();
            p.RaiseEvent(new RoutedPropertyChangedEventArgs<int>((int)e.OldValue, (int)e.NewValue, PageChangedEvent));
        }

        private void Rebuild() => SetValue(PagesPropertyKey, BuildPages(CurrentPage, PageCount, MaxVisiblePages));

        private static int? ToPage(object? parameter)
        {
            if (parameter is int i) return i;
            if (parameter is string s && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r)) return r;
            return null;
        }

        private static void OnCanGoToPage(object sender, CanExecuteRoutedEventArgs e)
        {
            var p = (Pagination)sender;
            int? page = ToPage(e.Parameter);
            e.CanExecute = page.HasValue && page.Value >= 1 && page.Value <= p.PageCount;
            e.Handled = true;
        }

        private static void OnGoToPage(object sender, ExecutedRoutedEventArgs e)
        {
            int? page = ToPage(e.Parameter);
            if (page.HasValue)
                ((Pagination)sender).SetCurrentValue(CurrentPageProperty, page.Value);
            e.Handled = true;
        }
    }
}
