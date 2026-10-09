using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Retro.Wpf.Controls
{
    /// <summary>Severity of a <see cref="MessageStrip"/>.</summary>
    public enum MessageSeverity { None, Info, Warning, Error, Success }

    /// <summary>A one-line inline message with a status icon and an optional × (the "Message" component).</summary>
    [TemplatePart(Name = PartClose, Type = typeof(ButtonBase))]
    public class MessageStrip : ContentControl
    {
        public const string PartClose = "PART_Close";
        private ButtonBase? _close;

        static MessageStrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageStrip), new FrameworkPropertyMetadata(typeof(MessageStrip)));
            FocusableProperty.OverrideMetadata(typeof(MessageStrip), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(
            nameof(Severity), typeof(MessageSeverity), typeof(MessageStrip),
            new FrameworkPropertyMetadata(MessageSeverity.Info, OnSeverityChanged));

        private static readonly DependencyPropertyKey IconPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Icon), typeof(IconKind?), typeof(MessageStrip), new FrameworkPropertyMetadata(IconKind.Info));

        public static readonly DependencyProperty IconProperty = IconPropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
            nameof(IsClosable), typeof(bool), typeof(MessageStrip), new FrameworkPropertyMetadata(true));

        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            nameof(Close), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageStrip));

        public MessageSeverity Severity { get => (MessageSeverity)GetValue(SeverityProperty); set => SetValue(SeverityProperty, value); }

        /// <summary>Icon derived from <see cref="Severity"/>.</summary>
        public IconKind? Icon => (IconKind?)GetValue(IconProperty);

        public bool IsClosable { get => (bool)GetValue(IsClosableProperty); set => SetValue(IsClosableProperty, value); }

        /// <summary>Raised when × is clicked. If no handler marks it handled, the strip collapses itself.</summary>
        public event RoutedEventHandler Close { add => AddHandler(CloseEvent, value); remove => RemoveHandler(CloseEvent, value); }

        internal static IconKind? IconFor(MessageSeverity severity)
        {
            switch (severity)
            {
                case MessageSeverity.Info: return IconKind.Info;
                case MessageSeverity.Warning: return IconKind.Warning;
                case MessageSeverity.Error: return IconKind.Error;
                case MessageSeverity.Success: return IconKind.Success;
                default: return null;
            }
        }

        private static void OnSeverityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            d.SetValue(IconPropertyKey, IconFor((MessageSeverity)e.NewValue));

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
