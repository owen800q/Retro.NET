using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>A button of a <see cref="RetroDialog"/>.</summary>
    public sealed class DialogButton
    {
        public DialogButton(string label, MessageBoxResult result, bool isDefault, bool isCancel)
        {
            Label = label;
            Result = result;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }

        public string Label { get; }
        public MessageBoxResult Result { get; }
        public bool IsDefault { get; }
        public bool IsCancel { get; }
    }

    /// <summary>
    /// The beveled message box window: title bar, 32px status icon, flat question, and
    /// right-aligned OK / Cancel style buttons above an etched footer line.
    /// </summary>
    public class RetroDialog : RetroWindow
    {
        static RetroDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RetroDialog), new FrameworkPropertyMetadata(typeof(RetroDialog)));
        }

        public RetroDialog(object message, string caption, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            Title = caption;
            Message = message;
            Image = IconFor(image);
            Buttons = CreateButtons(button, defaultResult);
            Result = Buttons.FirstOrDefault(b => b.IsCancel)?.Result ?? Buttons.Last().Result;

            TitleIcon = null;
            ResizeMode = ResizeMode.NoResize;
            SizeToContent = SizeToContent.WidthAndHeight;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            MinWidth = 320;

            Content = BuildContent();
        }

        /// <summary>The message (a string or any content).</summary>
        public object Message { get; }

        /// <summary>The 32px status icon, or null.</summary>
        public IconKind? Image { get; }

        /// <summary>Buttons in display order.</summary>
        public IReadOnlyList<DialogButton> Buttons { get; }

        /// <summary>The button the user chose. Before a choice, the cancel result.</summary>
        public MessageBoxResult Result { get; private set; }

        /// <summary>Records <paramref name="result"/> and closes the dialog.</summary>
        public void Complete(MessageBoxResult result)
        {
            Result = result;
            Close();
        }

        internal static IconKind? IconFor(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Error: return IconKind.Error; // also Hand, Stop
                case MessageBoxImage.Question: return IconKind.Help;
                case MessageBoxImage.Warning: return IconKind.Warning; // also Exclamation
                case MessageBoxImage.Information: return IconKind.Info; // also Asterisk
                default: return null;
            }
        }

        internal static IReadOnlyList<DialogButton> CreateButtons(MessageBoxButton button, MessageBoxResult defaultResult)
        {
            var results = new List<MessageBoxResult>();
            switch (button)
            {
                case MessageBoxButton.OKCancel: results.Add(MessageBoxResult.OK); results.Add(MessageBoxResult.Cancel); break;
                case MessageBoxButton.YesNo: results.Add(MessageBoxResult.Yes); results.Add(MessageBoxResult.No); break;
                case MessageBoxButton.YesNoCancel: results.Add(MessageBoxResult.Yes); results.Add(MessageBoxResult.No); results.Add(MessageBoxResult.Cancel); break;
                default: results.Add(MessageBoxResult.OK); break;
            }

            if (!results.Contains(defaultResult))
                defaultResult = results[0];

            bool hasCancel = results.Contains(MessageBoxResult.Cancel);
            return results.Select(r => new DialogButton(
                r.ToString() == "OK" ? "OK" : r.ToString(),
                r,
                r == defaultResult,
                hasCancel ? r == MessageBoxResult.Cancel : results.Count == 1)).ToList();
        }

        private UIElement BuildContent()
        {
            var root = new DockPanel { LastChildFill = true };

            // Footer: etched top edge (white over dark), buttons right-aligned with a 4px gap.
            var footerLines = new Border { BorderThickness = new Thickness(0, 1, 0, 0) };
            footerLines.SetResourceReference(Border.BorderBrushProperty, "Retro.BorderLight");
            var footer = new Border { BorderThickness = new Thickness(0, 1, 0, 0), Padding = new Thickness(8, 6, 8, 6) };
            footer.SetResourceReference(Border.BorderBrushProperty, "Retro.BorderDark");
            footerLines.Child = footer;
            DockPanel.SetDock(footerLines, Dock.Bottom);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            foreach (DialogButton b in Buttons)
            {
                var btn = new Button
                {
                    Content = b.Label,
                    MinWidth = 64,
                    IsDefault = b.IsDefault,
                    IsCancel = b.IsCancel,
                    Margin = new Thickness(buttons.Children.Count == 0 ? 0 : 4, 0, 0, 0),
                    Tag = b.Result,
                };
                btn.Click += (s, e) => Complete((MessageBoxResult)((Button)s).Tag);
                buttons.Children.Add(btn);
            }
            footer.Child = buttons;
            root.Children.Add(footerLines);

            var body = new Grid { Margin = new Thickness(12) };
            body.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            body.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            if (Image.HasValue)
            {
                var icon = new RetroIcon { Kind = Image.Value, Size = 32, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 0, 8, 0) };
                body.Children.Add(icon);
            }

            UIElement text;
            if (Message is string s)
                text = new TextBlock { Text = s, TextWrapping = TextWrapping.Wrap, MaxWidth = 420, VerticalAlignment = VerticalAlignment.Top };
            else
                text = new ContentPresenter { Content = Message, VerticalAlignment = VerticalAlignment.Top };
            Grid.SetColumn(text, 1);
            body.Children.Add(text);
            root.Children.Add(body);

            return root;
        }
    }

    /// <summary>Drop-in replacement for <see cref="MessageBox"/> using <see cref="RetroDialog"/>.</summary>
    public static class RetroMessageBox
    {
        public static MessageBoxResult Show(string messageBoxText) =>
            Show(null, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None);

        public static MessageBoxResult Show(string messageBoxText, string caption) =>
            Show(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None);

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button) =>
            Show(null, messageBoxText, caption, button, MessageBoxImage.None);

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon) =>
            Show(null, messageBoxText, caption, button, icon);

        public static MessageBoxResult Show(Window? owner, object message, string caption, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            var dialog = new RetroDialog(message, caption, button, icon, defaultResult);
            owner ??= FindActiveWindow();
            if (owner != null && owner.IsVisible)
                dialog.Owner = owner;
            else
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();
            return dialog.Result;
        }

        private static Window? FindActiveWindow()
        {
            Application? app = Application.Current;
            if (app == null)
                return null;
            return app.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) ?? app.MainWindow;
        }
    }
}
