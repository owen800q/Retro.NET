using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Retro.Wpf.Controls;
using Xunit;

namespace Retro.Wpf.Tests
{
    public class BehaviorTests
    {
        private static string Strip(int current, int count, int max = 7) =>
            string.Join(" ", Pagination.BuildPages(current, count, max).Select(p => p.IsCurrent ? "[" + p.Label + "]" : p.Label));

        [Fact]
        public void Pagination_builds_the_expected_strips()
        {
            Assert.Equal("‹ [1] 2 3 4 5 ›", Strip(1, 5));
            Assert.Equal("‹ 1 2 3 4 5 6 [7] ›", Strip(7, 7));
            Assert.Equal("‹ 1 [2] 3 4 5 … 20 ›", Strip(2, 20));
            Assert.Equal("‹ 1 2 3 [4] 5 … 20 ›", Strip(4, 20));
            Assert.Equal("‹ 1 … 9 [10] 11 … 20 ›", Strip(10, 20));
            Assert.Equal("‹ 1 … 16 [17] 18 19 20 ›", Strip(17, 20));
            Assert.Equal("‹ 1 … 16 17 18 19 [20] ›", Strip(20, 20));
            Assert.Equal("‹ [1] ›", Strip(0, 0));
        }

        [Fact]
        public void Pagination_prev_and_next_are_disabled_at_the_ends()
        {
            var first = Pagination.BuildPages(1, 10, 7);
            Assert.False(first.First().IsEnabled);
            Assert.True(first.Last().IsEnabled);
            var last = Pagination.BuildPages(10, 10, 7);
            Assert.True(last.First().IsEnabled);
            Assert.False(last.Last().IsEnabled);
            Assert.Equal(9, last.First().Page);
        }

        [Fact]
        public void Pagination_command_changes_page_and_raises_event()
        {
            Ui.Run(() =>
            {
                var p = new Pagination { PageCount = 10 };
                int? seen = null;
                p.PageChanged += (s, e) => seen = e.NewValue;
                Ui.Render(p);
                Assert.True(Pagination.GoToPageCommand.CanExecute(4, p));
                Pagination.GoToPageCommand.Execute(4, p);
                Assert.Equal(4, p.CurrentPage);
                Assert.Equal(4, seen);
                Assert.False(Pagination.GoToPageCommand.CanExecute(11, p));

                p.CurrentPage = 99;
                Assert.Equal(10, p.CurrentPage);
                p.PageCount = 3;
                Assert.Equal(3, p.CurrentPage);
                Assert.Equal(3, p.Pages.Count(e => e.Kind == PageEntryKind.Page));
            });
        }

        [Fact]
        public void Badge_formats_and_hides()
        {
            Ui.Run(() =>
            {
                var b = new Badge { Count = 3 };
                Assert.Equal("3", b.DisplayText);
                Assert.True(b.IsBadgeVisible);
                b.Count = 120;
                Assert.Equal("99+", b.DisplayText);
                b.Count = 0;
                Assert.False(b.IsBadgeVisible);
                b.ShowZero = true;
                Assert.True(b.IsBadgeVisible);
                b.Text = "New";
                Assert.Equal("New", b.DisplayText);
            });
        }

        [Fact]
        public void Tag_close_collapses_unless_handled()
        {
            Ui.Run(() =>
            {
                var tag = new Tag { Content = "x", IsClosable = true };
                Ui.Render(tag);
                var close = (Button)tag.Template.FindName(Tag.PartClose, tag);
                ((IInvokeProvider)new ButtonAutomationPeer(close).GetPattern(PatternInterface.Invoke)).Invoke();
                Ui.DoEvents();
                Assert.Equal(Visibility.Collapsed, tag.Visibility);

                var kept = new Tag { Content = "y", IsClosable = true };
                kept.Close += (s, e) => e.Handled = true;
                kept.RequestClose();
                Assert.Equal(Visibility.Visible, kept.Visibility);
            });
        }

        [Fact]
        public void MessageStrip_icon_follows_severity()
        {
            Ui.Run(() =>
            {
                var m = new MessageStrip();
                Assert.Equal(IconKind.Info, m.Icon);
                m.Severity = MessageSeverity.Error;
                Assert.Equal(IconKind.Error, m.Icon);
                m.Severity = MessageSeverity.Success;
                Assert.Equal(IconKind.Success, m.Icon);
                m.Severity = MessageSeverity.None;
                Assert.Null(m.Icon);
                m.RequestClose();
                Assert.Equal(Visibility.Collapsed, m.Visibility);
            });
        }

        [Fact]
        public void ToggleSwitch_toggles()
        {
            Ui.Run(() =>
            {
                var s = new ToggleSwitch();
                Ui.Render(s);
                ((IToggleProvider)new ToggleButtonAutomationPeer(s).GetPattern(PatternInterface.Toggle)).Toggle();
                Assert.True(s.IsChecked);
            });
        }

        [Fact]
        public void FormField_error_flags_inner_inputs()
        {
            Ui.Run(() =>
            {
                var box = new TextBox();
                var field = new FormField { Label = "Carrier", Content = box };
                Ui.Render(field);
                Assert.False(Assist.GetIsError(box));
                field.ErrorText = "Field is required.";
                Assert.True(field.HasError);
                Assert.True(Assist.GetIsError(box));
                field.ErrorText = null;
                Assert.False(field.HasError);
                Assert.False(Assist.GetIsError(box));
            });
        }

        [Fact]
        public void Breadcrumb_marks_first_and_current_and_reports_clicks()
        {
            Ui.Run(() =>
            {
                var bc = new Breadcrumb();
                bc.Items.Add("Home");
                bc.Items.Add("Shipment");
                bc.Items.Add("Create");
                object? clicked = null;
                bc.ItemClick += (s, e) => clicked = e.Item;
                Ui.Render(bc);

                var items = Enumerable.Range(0, 3).Select(i => (BreadcrumbItem)bc.ItemContainerGenerator.ContainerFromIndex(i)).ToList();
                Assert.True(items[0].IsFirst);
                Assert.False(items[1].IsFirst);
                Assert.True(items[2].IsCurrent);
                Assert.False(items[1].IsCurrent);

                items[1].PerformClick();
                Ui.DoEvents();
                Assert.Equal("Shipment", clicked);

                clicked = null;
                items[2].PerformClick();
                Ui.DoEvents();
                Assert.Null(clicked);

                bc.Items.Add("Items");
                Ui.Render(bc);
                Assert.False(items[2].IsCurrent);
            });
        }

        [Fact]
        public void Timeline_highlights_the_first_item()
        {
            Ui.Run(() =>
            {
                var tl = new Timeline();
                var a = new TimelineItem { Header = "a" };
                var b = new TimelineItem { Header = "b" };
                tl.Items.Add(a);
                tl.Items.Add(b);
                Ui.Render(tl);
                Assert.True(a.IsHighlighted);
                Assert.False(b.IsHighlighted);
            });
        }

        [Fact]
        public void RetroDialog_maps_buttons_icons_and_results()
        {
            Ui.Run(() =>
            {
                var d = new RetroDialog("Delete?", "Confirm Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                Assert.Equal(new[] { "OK", "Cancel" }, d.Buttons.Select(b => b.Label));
                Assert.True(d.Buttons[0].IsDefault);
                Assert.True(d.Buttons[1].IsCancel);
                Assert.Equal(IconKind.Warning, d.Image);
                Assert.Equal(MessageBoxResult.Cancel, d.Result);
                Assert.Equal(ResizeMode.NoResize, d.ResizeMode);
                d.Complete(MessageBoxResult.OK);
                Assert.Equal(MessageBoxResult.OK, d.Result);

                var yn = new RetroDialog("Save?", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);
                Assert.Equal(new[] { "Yes", "No", "Cancel" }, yn.Buttons.Select(b => b.Label));
                Assert.True(yn.Buttons[1].IsDefault);
                Assert.Equal(IconKind.Help, yn.Image);
                yn.Close();

                var ok = new RetroDialog("Done.", "Info");
                Assert.Single(ok.Buttons);
                Assert.True(ok.Buttons[0].IsCancel);
                Assert.Null(ok.Image);
                ok.Close();
            });
        }

        [Fact]
        public void RetroIcon_measures_to_its_size()
        {
            Ui.Run(() =>
            {
                var icon = new RetroIcon { Kind = IconKind.Save, Size = 32 };
                icon.Measure(new Size(100, 100));
                Assert.Equal(new Size(32, 32), icon.DesiredSize);
            });
        }
    }
}
