using System;
using System.Windows;
using System.Windows.Input;

namespace Retro.Wpf.Controls
{
    /// <summary>
    /// A window with the Retro chrome: glossy blue gradient title bar, beveled caption buttons
    /// and a raised outer frame. Derive your windows from it (<c>&lt;retro:RetroWindow ...&gt;</c>).
    /// </summary>
    public class RetroWindow : Window
    {
        static RetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RetroWindow), new FrameworkPropertyMetadata(typeof(RetroWindow)));
        }

        public RetroWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => SystemCommands.CloseWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (s, e) => SystemCommands.MinimizeWindow(this),
                (s, e) => e.CanExecute = ResizeMode != ResizeMode.NoResize));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (s, e) => SystemCommands.MaximizeWindow(this),
                (s, e) => e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (s, e) => SystemCommands.RestoreWindow(this),
                (s, e) => e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip));
        }

        public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(
            nameof(TitleIcon), typeof(IconKind?), typeof(RetroWindow), new FrameworkPropertyMetadata(IconKind.Document));

        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
            nameof(TitleBarContent), typeof(object), typeof(RetroWindow), new FrameworkPropertyMetadata(null));

        /// <summary>14px icon at the left of the title bar; null hides it.</summary>
        public IconKind? TitleIcon { get => (IconKind?)GetValue(TitleIconProperty); set => SetValue(TitleIconProperty, value); }

        /// <summary>Extra content placed in the title bar, right-aligned before the caption buttons.</summary>
        public object? TitleBarContent { get => GetValue(TitleBarContentProperty); set => SetValue(TitleBarContentProperty, value); }

        /// <summary>Margin applied while maximized so the frame is not clipped by the screen edges.</summary>
        public static Thickness MaximizedMargin
        {
            get
            {
                Thickness r = SystemParameters.WindowResizeBorderThickness;
                double pad = SystemParameters.WindowNonClientFrameThickness.Left - r.Left;
                pad = Math.Max(0, pad);
                return new Thickness(r.Left + pad, r.Top + pad, r.Right + pad, r.Bottom + pad);
            }
        }
    }
}
