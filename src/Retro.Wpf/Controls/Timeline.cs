using System.Windows;
using System.Windows.Controls;

namespace Retro.Wpf.Controls
{
    /// <summary>A vertical list of events with square markers; the first (latest) marker is amber.</summary>
    public class Timeline : ItemsControl
    {
        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            FocusableProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty HighlightFirstProperty = DependencyProperty.Register(
            nameof(HighlightFirst), typeof(bool), typeof(Timeline), new FrameworkPropertyMetadata(true));

        /// <summary>Marks the first item with the accent color (default true).</summary>
        public bool HighlightFirst { get => (bool)GetValue(HighlightFirstProperty); set => SetValue(HighlightFirstProperty, value); }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is TimelineItem;

        protected override DependencyObject GetContainerForItemOverride() => new TimelineItem();

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (HighlightFirst && element is TimelineItem t && ItemContainerGenerator.IndexFromContainer(t) == 0
                && t.ReadLocalValue(TimelineItem.IsHighlightedProperty) == DependencyProperty.UnsetValue)
            {
                t.SetCurrentValue(TimelineItem.IsHighlightedProperty, true);
            }
        }
    }

    /// <summary>A <see cref="Timeline"/> entry: bold <c>Header</c> title with muted <c>Content</c> below.</summary>
    public class TimelineItem : HeaderedContentControl
    {
        static TimelineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineItem), new FrameworkPropertyMetadata(typeof(TimelineItem)));
            FocusableProperty.OverrideMetadata(typeof(TimelineItem), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
            nameof(IsHighlighted), typeof(bool), typeof(TimelineItem), new FrameworkPropertyMetadata(false));

        public bool IsHighlighted { get => (bool)GetValue(IsHighlightedProperty); set => SetValue(IsHighlightedProperty, value); }
    }
}
