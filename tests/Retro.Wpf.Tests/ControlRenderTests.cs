using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Retro.Wpf.Controls;
using Xunit;

namespace Retro.Wpf.Tests
{
    /// <summary>Renders every themed control, checks that the Retro template is in use, and writes PNG snapshots.</summary>
    public class ControlRenderTests
    {
        public static IEnumerable<object[]> Controls()
        {
            yield return new object[] { "Button" };
            yield return new object[] { "ButtonDisabled" };
            yield return new object[] { "ButtonIcon" };
            yield return new object[] { "ToggleButton" };
            yield return new object[] { "TextBox" };
            yield return new object[] { "PasswordBox" };
            yield return new object[] { "ComboBox" };
            yield return new object[] { "ComboBoxEditable" };
            yield return new object[] { "CheckBox" };
            yield return new object[] { "RadioButton" };
            yield return new object[] { "TabControl" };
            yield return new object[] { "GroupBox" };
            yield return new object[] { "Expander" };
            yield return new object[] { "ListBox" };
            yield return new object[] { "ListView" };
            yield return new object[] { "TreeView" };
            yield return new object[] { "DataGrid" };
            yield return new object[] { "ProgressBar" };
            yield return new object[] { "ProgressBarIndeterminate" };
            yield return new object[] { "Slider" };
            yield return new object[] { "ScrollViewer" };
            yield return new object[] { "Menu" };
            yield return new object[] { "ToolBar" };
            yield return new object[] { "StatusBar" };
            yield return new object[] { "Calendar" };
            yield return new object[] { "DatePicker" };
            yield return new object[] { "ToggleSwitch" };
            yield return new object[] { "Tag" };
            yield return new object[] { "MessageStrip" };
            yield return new object[] { "Badge" };
            yield return new object[] { "Avatar" };
            yield return new object[] { "Breadcrumb" };
            yield return new object[] { "Pagination" };
            yield return new object[] { "FormField" };
            yield return new object[] { "Timeline" };
            yield return new object[] { "EmptyState" };
        }

        private static FrameworkElement Create(string name)
        {
            switch (name)
            {
                case "Button": return new Button { Content = "Execute" };
                case "ButtonDisabled": return new Button { Content = "Disabled", IsEnabled = false };
                case "ButtonIcon": return WithIcon(new Button { Content = "Track Shipment" }, IconKind.Export);
                case "ToggleButton": return new ToggleButton { Content = "Active", IsChecked = true };
                case "TextBox": return new TextBox { Text = "DHL Freight", Width = 160 };
                case "PasswordBox": return new PasswordBox { Password = "secret", Width = 160 };
                case "ComboBox": return new ComboBox { ItemsSource = new[] { "DHL Freight", "UPS Ground" }, SelectedIndex = 0, Width = 160 };
                case "ComboBoxEditable": return new ComboBox { IsEditable = true, Text = "Free text", Width = 160 };
                case "CheckBox": return new CheckBox { Content = "Checked", IsChecked = true };
                case "RadioButton": return new RadioButton { Content = "Selected", IsChecked = true };
                case "TabControl":
                    var tabs = new TabControl { Width = 300, Height = 90 };
                    tabs.Items.Add(new TabItem { Header = "Header", Content = new TextBlock { Text = "Page" } });
                    tabs.Items.Add(new TabItem { Header = "Items (6)" });
                    tabs.Items.Add(new TabItem { Header = "Log", IsEnabled = false });
                    tabs.SelectedIndex = 0;
                    return tabs;
                case "GroupBox": return new GroupBox { Header = "General data", Content = new TextBlock { Text = "Content" }, Width = 200 };
                case "Expander": return new Expander { Header = "Sections", IsExpanded = true, Content = new TextBlock { Text = "Body" }, Width = 200 };
                case "ListBox": return new ListBox { ItemsSource = new[] { "One", "Two", "Three" }, SelectedIndex = 1, Width = 160 };
                case "ListView":
                    var gv = new GridView();
                    gv.Columns.Add(new GridViewColumn { Header = "Name", Width = 100 });
                    return new ListView { View = gv, ItemsSource = new[] { "A", "B" }, Width = 160, Height = 80 };
                case "TreeView":
                    var root = new TreeViewItem { Header = "Logistics", IsExpanded = true };
                    root.Items.Add(new TreeViewItem { Header = "Shipment", IsSelected = true });
                    root.Items.Add(new TreeViewItem { Header = "Tracking" });
                    var tree = new TreeView { Width = 160, Height = 80 };
                    tree.Items.Add(root);
                    return tree;
                case "DataGrid":
                    var grid = new DataGrid { Width = 260, Height = 100, AutoGenerateColumns = true, IsReadOnly = true };
                    grid.ItemsSource = new[] { new Row("6000068250", "DHL Freight"), new Row("6000068251", "UPS Ground") };
                    grid.SelectedIndex = 1;
                    return grid;
                case "ProgressBar": return new ProgressBar { Value = 60, Width = 200 };
                case "ProgressBarIndeterminate": return new ProgressBar { IsIndeterminate = true, Width = 200 };
                case "Slider": return new Slider { Maximum = 100, Value = 60, Width = 160 };
                case "ScrollViewer":
                    return new ScrollViewer
                    {
                        Width = 120, Height = 80,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                        Content = new Border { Width = 400, Height = 400 },
                    };
                case "Menu":
                    var menu = new Menu { Width = 260 };
                    menu.Items.Add(new MenuItem { Header = "_Shipment" });
                    menu.Items.Add(new MenuItem { Header = "_Edit" });
                    return menu;
                case "ToolBar":
                    var tb = new ToolBar { Width = 200 };
                    tb.Items.Add(WithIcon(new Button(), IconKind.Save));
                    tb.Items.Add(new Separator());
                    tb.Items.Add(WithIcon(new Button { Content = "Execute" }, IconKind.Execute));
                    return tb;
                case "StatusBar":
                    var sb = new StatusBar { Width = 260 };
                    sb.Items.Add(new StatusBarItem { Content = "Ready." });
                    return sb;
                case "Calendar": return new Calendar { DisplayDate = new DateTime(2026, 3, 1), SelectedDate = new DateTime(2026, 3, 17), IsTodayHighlighted = false };
                case "DatePicker": return new DatePicker { SelectedDate = new DateTime(2026, 3, 17), Width = 160 };
                case "ToggleSwitch": return new ToggleSwitch { IsChecked = true };
                case "Tag": return new Tag { Tone = TagTone.Error, Content = "Error", IsClosable = true };
                case "MessageStrip": return new MessageStrip { Severity = MessageSeverity.Error, Content = "Field \"Carrier\" is required.", Width = 300 };
                case "Badge": return new Badge { Count = 3, Content = new RetroIcon { Kind = IconKind.Mail, Size = 22 }, Margin = new Thickness(6) };
                case "Avatar": return new Avatar { Initials = "JM" };
                case "Breadcrumb":
                    var bc = new Breadcrumb();
                    bc.Items.Add("Home");
                    bc.Items.Add("Shipment");
                    bc.Items.Add("Create");
                    return bc;
                case "Pagination": return new Pagination { PageCount = 20, CurrentPage = 2 };
                case "FormField": return new FormField { Label = "Carrier", IsRequired = true, ErrorText = "Field is required.", Content = new TextBox { Width = 160 } };
                case "Timeline":
                    var tl = new Timeline { Width = 220 };
                    tl.Items.Add(new TimelineItem { Header = "Document created", Content = "2026-03-17 14:55" });
                    tl.Items.Add(new TimelineItem { Header = "Sent for approval", Content = "2026-03-17 15:02" });
                    return tl;
                case "EmptyState": return new EmptyState { Description = "Nothing here.", Width = 260 };
                default: throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        private static T WithIcon<T>(T element, IconKind icon) where T : DependencyObject
        {
            Assist.SetIcon(element, icon);
            return element;
        }

        public sealed class Row
        {
            public Row(string doc, string carrier) { Doc = doc; Carrier = carrier; }
            public string Doc { get; }
            public string Carrier { get; }
        }

        [Theory]
        [MemberData(nameof(Controls))]
        public void Control_renders_with_the_retro_template(string name)
        {
            Ui.Run(() =>
            {
                Ui.BindingErrors.Clear();
                FrameworkElement element = Create(name);
                var host = new Border { Padding = new Thickness(6), Child = element };
                Ui.Render(host, snapshot: "control-" + name);

                bool usesRetroVisuals = Ui.Descendants<Bevel>(host).Any() || Ui.Descendants<TabShape>(host).Any()
                                        || Ui.Descendants<RetroIcon>(host).Any();
                if (name != "Menu" && name != "StatusBar" && name != "Breadcrumb" && name != "Timeline" && name != "Avatar"
                    && name != "Tag" && name != "MessageStrip" && name != "RadioButton")
                {
                    Assert.True(usesRetroVisuals, name + " does not use a Retro template");
                }
                AssertNoBindingErrors();
            });
        }

        internal static void AssertNoBindingErrors()
        {
            var errors = Ui.BindingErrors.Messages;
            Assert.True(errors.Count == 0, "Binding errors:\n" + string.Join("\n", errors));
        }

        [Fact]
        public void Button_has_strong_raised_bevel()
        {
            Ui.Run(() =>
            {
                var bmp = Ui.Render(new Button { Content = "OK", Width = 80 });
                Assert.Equal(24, bmp.PixelHeight);
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(bmp, 0, 0));
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(bmp, 1, 1));
                Assert.Equal(Ui.Hex("#6E7A89"), Ui.Pixel(bmp, 78, 22));
            });
        }

        [Fact]
        public void Disabled_button_drops_the_black_outline()
        {
            Ui.Run(() =>
            {
                var bmp = Ui.Render(new Button { Content = "Off", Width = 80, IsEnabled = false });
                Assert.Equal(Ui.Hex("#D6E4F1"), Ui.Pixel(bmp, 0, 0));   // host surface shows through
                Assert.Equal(Ui.Hex("#EAF1F8"), Ui.Pixel(bmp, 40, 3));   // surface-2 fill
            });
        }

        [Fact]
        public void TextBox_is_sunken_white_and_turns_pink_on_error()
        {
            Ui.Run(() =>
            {
                var ok = Ui.Render(new TextBox { Width = 100 });
                Assert.Equal(22, ok.PixelHeight);
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(ok, 0, 0));
                Assert.Equal(Ui.Hex("#6E7A89"), Ui.Pixel(ok, 1, 1));
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(ok, 50, 11));

                var error = new TextBox { Width = 100 };
                Assist.SetIsError(error, true);
                var bad = Ui.Render(error, snapshot: "textbox-error");
                Assert.Equal(Ui.Hex("#FFEAE7"), Ui.Pixel(bad, 50, 11));

                var disabled = Ui.Render(new TextBox { Width = 100, IsEnabled = false });
                Assert.Equal(Ui.Hex("#EAF1F8"), Ui.Pixel(disabled, 50, 11));
            });
        }

        [Fact]
        public void CheckBox_draws_the_pixel_check_mark()
        {
            Ui.Run(() =>
            {
                var on = Ui.Render(new CheckBox { IsChecked = true });
                Assert.Equal(15, on.PixelWidth);
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(on, 0, 0));
                Assert.Equal(Ui.Hex("#000000"), Ui.Pixel(on, 4, 8));   // bottom-left stroke of the check
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(on, 11, 11));

                var off = Ui.Render(new CheckBox { IsChecked = false });
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(off, 4, 8));
            });
        }

        [Fact]
        public void ToggleSwitch_moves_the_knob_and_turns_amber()
        {
            Ui.Run(() =>
            {
                var on = Ui.Render(new ToggleSwitch { IsChecked = true }, snapshot: "switch-on");
                Assert.Equal(36, on.PixelWidth);
                Assert.Equal(18, on.PixelHeight);
                Assert.Equal(Ui.Hex("#F0AB00"), Ui.Pixel(on, 8, 9));

                var off = Ui.Render(new ToggleSwitch { IsChecked = false }, snapshot: "switch-off");
                Assert.Equal(Ui.Hex("#EAF1F8"), Ui.Pixel(off, 28, 9));
            });
        }

        [Fact]
        public void ProgressBar_fills_with_stripes_up_to_its_value()
        {
            Ui.Run(() =>
            {
                var bmp = Ui.Render(new ProgressBar { Value = 50, Width = 202 });
                Assert.Equal(14, bmp.PixelHeight);
                Color filled = Ui.Pixel(bmp, 20, 7);
                Assert.True(filled == Ui.Hex("#2A6FB8") || filled == Ui.Hex("#4A7FB8"), "unexpected stripe color " + filled);
                Assert.Equal(Ui.Hex("#FFFFFF"), Ui.Pixel(bmp, 180, 7));
            });
        }

        [Fact]
        public void Selected_tab_is_blue_and_selected_row_is_amber()
        {
            Ui.Run(() =>
            {
                var tabs = (TabControl)Create("TabControl");
                var host = new Border { Child = tabs };
                var bmp = Ui.Render(host);
                var tab = (TabItem)tabs.Items[0];
                Point p = Ui.Origin(tab, host);
                Assert.Equal(Ui.Hex("#2A6FB8"), Ui.Pixel(bmp, (int)p.X + 6, (int)(p.Y + tab.ActualHeight - 3)));

                var grid = (DataGrid)Create("DataGrid");
                var gridHost = new Border { Child = grid };
                var gbmp = Ui.Render(gridHost);
                var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(1);
                Point rp = Ui.Origin(row, gridHost);
                Assert.Equal(Ui.Hex("#F0AB00"), Ui.Pixel(gbmp, (int)rp.X + 2, (int)rp.Y + 3));
            });
        }

        [Fact]
        public void Tag_and_message_tones_use_status_colors()
        {
            Ui.Run(() =>
            {
                var err = Ui.Render(new Tag { Tone = TagTone.Error, Content = "Error" });
                Assert.Equal(Ui.Hex("#8C1A13"), Ui.Pixel(err, 0, 0));
                Assert.Equal(Ui.Hex("#C8281E"), Ui.Pixel(err, 2, 2));

                var info = Ui.Render(new Tag { Content = "Info" });
                Assert.Equal(Ui.Hex("#1F6BB8"), Ui.Pixel(info, 0, 0));
                Assert.Equal(Ui.Hex("#E0EDF8"), Ui.Pixel(info, 2, 2));

                var warn = Ui.Render(new MessageStrip { Severity = MessageSeverity.Warning, Content = "Check", Width = 200 });
                Assert.Equal(Ui.Hex("#E8A100"), Ui.Pixel(warn, 0, 0));
                Assert.Equal(Ui.Hex("#FFF4D6"), Ui.Pixel(warn, 150, 3));
            });
        }
    }
}
