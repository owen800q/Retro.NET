using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Retro.Wpf.Controls
{
    /// <summary>Color tone of a <see cref="Tag"/>.</summary>
    public enum TagTone { Info, Warning, Error, Success }

    /// <summary>A small square status label (Info / Warning / Error / Success), optionally closable.</summary>
    [TemplatePart(Name = PartClose, Type = typeof(ButtonBase))]
    public class Tag : ContentControl
    {
        public const string PartClose = "PART_Close";
        private ButtonBase? _close;

        static Tag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
            FocusableProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty ToneProperty = DependencyProperty.Register(
            nameof(Tone), typeof(TagTone), typeof(Tag), new FrameworkPropertyMetadata(TagTone.Info));

        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
            nameof(IsClosable), typeof(bool), typeof(Tag), new FrameworkPropertyMetadata(false));

        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            nameof(Close), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Tag));

        public TagTone Tone { get => (TagTone)GetValue(ToneProperty); set => SetValue(ToneProperty, value); }

        /// <summary>Shows the × button.</summary>
        public bool IsClosable { get => (bool)GetValue(IsClosableProperty); set => SetValue(IsClosableProperty, value); }

        /// <summary>Raised when × is clicked. If no handler marks it handled, the tag collapses itself.</summary>
        public event RoutedEventHandler Close { add => AddHandler(CloseEvent, value); remove => RemoveHandler(CloseEvent, value); }

        public override void OnApplyTemplate()
        {
            if (_close != null)
                _close.Click -= OnCloseClick;
            base.OnApplyTemplate();
            _close = GetTemplateChild(PartClose) as ButtonBase;
            if (_close != null)
                _close.Click += OnCloseClick;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e) => RequestClose();

        /// <summary>Raises <see cref="Close"/> as if × had been clicked.</summary>
        public void RequestClose()
        {
            var args = new RoutedEventArgs(CloseEvent, this);
            RaiseEvent(args);
            if (!args.Handled)
                Visibility = Visibility.Collapsed;
        }
    }
}
