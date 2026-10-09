using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Retro.Wpf.Tests
{
    /// <summary>
    /// Runs test bodies on a single long-lived STA thread that owns a WPF <see cref="Application"/>
    /// with the Retro theme merged into its resources.
    /// </summary>
    internal static class Ui
    {
        private static readonly Dispatcher Dispatcher;
        public static readonly BindingErrorListener BindingErrors = new BindingErrorListener();

        static Ui()
        {
            Dispatcher? dispatcher = null;
            Exception? startupError = null;
            using var ready = new ManualResetEventSlim();
            var thread = new Thread(() =>
            {
                try
                {
                    PresentationTraceSources.Refresh();
                    PresentationTraceSources.DataBindingSource.Listeners.Add(BindingErrors);
                    PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

                    var app = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
                    app.Resources.MergedDictionaries.Add(new RetroTheme());
                    dispatcher = Dispatcher.CurrentDispatcher;
                }
                catch (Exception ex)
                {
                    startupError = ex;
                }
                ready.Set();
                if (startupError == null)
                    Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            ready.Wait();
            if (startupError != null)
                throw new InvalidOperationException("WPF test host failed to start", startupError);
            Dispatcher = dispatcher!;
        }

        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

        public static void Run(Action action, [System.Runtime.CompilerServices.CallerMemberName] string? caller = null)
        {
            Exception? error = null;
            DispatcherOperation op = Dispatcher.BeginInvoke(new Action(() =>
            {
                try { action(); }
                catch (Exception ex) { error = ex; }
            }));
            if (op.Wait(Timeout) != DispatcherOperationStatus.Completed)
                throw new TimeoutException("UI work in " + caller + " did not finish within " + Timeout.TotalSeconds + "s (status " + op.Status + ")");
            if (error != null)
                ExceptionDispatchInfo.Capture(error).Throw();
        }

        /// <summary>Processes pending dispatcher work (bindings, template application, loaded events).</summary>
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => frame.Continue = false));
            Dispatcher.PushFrame(frame);
        }

        public static string SnapshotDirectory
        {
            get
            {
                string dir = Environment.GetEnvironmentVariable("RETRO_SNAPSHOTS")
                             ?? Path.Combine(AppContext.BaseDirectory, "snapshots");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        /// <summary>Lays out <paramref name="element"/> on a surface-colored host and renders it at 96 DPI.</summary>
        public static BitmapSource Render(FrameworkElement element, double width = double.NaN, double height = double.NaN, string? snapshot = null)
        {
            var host = new System.Windows.Controls.Border
            {
                Background = (Brush)Application.Current.FindResource("Retro.Surface"),
                Child = element,
                UseLayoutRounding = true,
                SnapsToDevicePixels = true,
            };
            TextOptionsHelper.Apply(host);

            var available = new Size(double.IsNaN(width) ? double.PositiveInfinity : width, double.IsNaN(height) ? double.PositiveInfinity : height);
            host.Measure(available);
            DoEvents();
            host.Measure(available);
            double w = double.IsNaN(width) ? Math.Ceiling(host.DesiredSize.Width) : width;
            double h = double.IsNaN(height) ? Math.Ceiling(host.DesiredSize.Height) : height;
            host.Arrange(new Rect(0, 0, w, h));
            host.UpdateLayout();
            DoEvents();
            host.UpdateLayout();

            var bmp = new RenderTargetBitmap(Math.Max(1, (int)w), Math.Max(1, (int)h), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(host);
            bmp.Freeze();
            if (snapshot != null)
                Save(bmp, snapshot);
            host.Child = null;
            return bmp;
        }

        public static BitmapSource RenderWindow(Window window, string? snapshot = null)
        {
            window.Left = -20000;
            window.Top = -20000;
            window.ShowActivated = false;
            window.ShowInTaskbar = false;
            window.Show();
            try
            {
                window.UpdateLayout();
                DoEvents();
                var root = (FrameworkElement)VisualTreeHelper.GetChild(window, 0);
                int w = (int)Math.Ceiling(root.ActualWidth), h = (int)Math.Ceiling(root.ActualHeight);
                var bmp = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(root);
                bmp.Freeze();
                if (snapshot != null)
                    Save(bmp, snapshot);
                return bmp;
            }
            finally
            {
                window.Close();
            }
        }

        public static void Save(BitmapSource bmp, string name)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using FileStream fs = File.Create(Path.Combine(SnapshotDirectory, name + ".png"));
            encoder.Save(fs);
        }

        public static Color Pixel(BitmapSource bmp, int x, int y)
        {
            var px = new byte[4];
            bmp.CopyPixels(new Int32Rect(x, y, 1, 1), px, 4, 0);
            return Color.FromArgb(px[3], px[2], px[1], px[0]);
        }

        public static Color Hex(string hex) => (Color)ColorConverter.ConvertFromString(hex);

        public static IEnumerable<T> Descendants<T>(DependencyObject root) where T : DependencyObject
        {
            int n = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < n; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child is T t)
                    yield return t;
                foreach (T d in Descendants<T>(child))
                    yield return d;
            }
        }

        public static Point Origin(Visual element, Visual ancestor) =>
            element.TransformToAncestor(ancestor).Transform(new Point(0, 0));
    }

    internal static class TextOptionsHelper
    {
        public static void Apply(FrameworkElement e)
        {
            e.SetResourceReference(System.Windows.Documents.TextElement.FontFamilyProperty, "Retro.FontFamily");
            e.SetResourceReference(System.Windows.Documents.TextElement.FontSizeProperty, "Retro.FontSize.SM");
            System.Windows.Media.TextOptions.SetTextFormattingMode(e, TextFormattingMode.Display);
        }
    }

    /// <summary>Collects WPF data-binding errors (System.Windows.Data Error: …).</summary>
    internal sealed class BindingErrorListener : TraceListener
    {
        private readonly List<string> _messages = new List<string>();
        private string _pending = string.Empty;

        public IReadOnlyList<string> Messages { get { lock (_messages) return _messages.ToList(); } }

        public void Clear() { lock (_messages) _messages.Clear(); }

        public override void Write(string? message) { lock (_messages) _pending += message; }

        public override void WriteLine(string? message)
        {
            lock (_messages)
            {
                _messages.Add(_pending + message);
                _pending = string.Empty;
            }
        }
    }
}
