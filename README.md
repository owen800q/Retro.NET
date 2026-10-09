# Retro.NET

**A pixel-faithful retro enterprise theme for WPF**: the beveled, 3D, MS Sans Serif look of classic SAP GUI / Windows 9x workstations, built as a real WPF control library.

[![build](https://github.com/owen800q/Retro.NET/actions/workflows/build.yml/badge.svg)](https://github.com/owen800q/Retro.NET/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Retro.NET.svg)](https://www.nuget.org/packages/Retro.NET)

![Shipment Maintenance demo](https://raw.githubusercontent.com/owen800q/Retro.NET/main/docs/screenshots/main-window.png)

- **One line to restyle an app.** Every standard WPF control gets the Retro look: Button, TextBox, PasswordBox, ComboBox, CheckBox, RadioButton, TabControl, GroupBox, Expander, ListBox, ListView, TreeView, DataGrid, ProgressBar, Slider, ScrollBar, Menu, ContextMenu, ToolBar, StatusBar, ToolTip, Calendar, DatePicker, Hyperlink and Label.
- **Extra controls from the design system.** `RetroWindow`, `Bevel`, `ToggleSwitch`, `Tag`, `MessageStrip`, `Badge`, `Avatar`, `Breadcrumb`, `Pagination`, `FormField`, `Timeline`, `EmptyState`, `RetroIcon` and `RetroMessageBox`.
- **44 pixel-style 16×16 icons** (`RetroIcon`, `{retro:Icon Save}`).
- **Design tokens** as overridable `Retro.*` resources: colors, gradients, type scale and spacing.
- Targets **.NET Framework 4.6.2**, **.NET 8** and **.NET 10** (`-windows`).

## Install

```
dotnet add package Retro.NET
```

## Quick start

Merge the theme in `App.xaml`:

```xml
<Application ...
             xmlns:retro="https://github.com/owen800q/Retro.NET">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <retro:RetroTheme />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

Derive your windows from `RetroWindow` to get the glossy blue title bar and beveled frame:

```xml
<retro:RetroWindow x:Class="MyApp.MainWindow"
                   xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                   xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                   xmlns:retro="https://github.com/owen800q/Retro.NET"
                   Title="Shipment: Display 0000080014" TitleIcon="Document">
  <DockPanel>
    <Menu DockPanel.Dock="Top">
      <MenuItem Header="_Shipment">
        <MenuItem Header="_Save" InputGestureText="Ctrl+S" retro:Assist.Icon="Save" />
      </MenuItem>
    </Menu>
    <ToolBar DockPanel.Dock="Top">
      <Button retro:Assist.Icon="Save" ToolTip="Save" />
      <Separator />
      <Button retro:Assist.Icon="Execute" Content="Execute" />
    </ToolBar>
    <StatusBar DockPanel.Dock="Bottom">
      <StatusBarItem Content="Ready." />
    </StatusBar>

    <GroupBox Header="General data" Margin="6">
      <StackPanel>
        <retro:FormField Label="Carrier" IsRequired="True">
          <ComboBox Width="200" />
        </retro:FormField>
        <retro:FormField Label="Tax ID" HelpText="Enter the customer's tax ID (10 digits).">
          <TextBox Width="200" />
        </retro:FormField>
      </StackPanel>
    </GroupBox>
  </DockPanel>
</retro:RetroWindow>
```

```csharp
public partial class MainWindow : Retro.Wpf.Controls.RetroWindow { ... }
```

A plain `Window` can use `Style="{StaticResource Retro.Window}"` for the background and font.

## Controls

| Control | What it is |
| --- | --- |
| `RetroWindow` | Custom chrome: title bar gradient, 14px icon, beveled `_ ▢ ×` caption buttons, raised frame. `TitleIcon`, `TitleBarContent`. |
| `Bevel` | The core primitive: a decorator that draws crisp 1px bevels. `Kind` = `Raised`, `RaisedStrong`, `Sunken`, `SunkenStrong`, `Flat`, `Fieldset`. Presets: `Retro.Panel`, `Retro.InsetPanel`, `Retro.Banner`, `Retro.Popover`, `Retro.Well`. |
| `ToggleSwitch` | Sunken track and raised knob that turns amber when on. Switches instantly, with no animation. |
| `Tag` | Square status label: `Tone` = `Info`, `Warning`, `Error`, `Success`. Set `IsClosable` to show a × button; clicking it raises `Close`. |
| `MessageStrip` | Inline message with a severity icon and ×. |
| `Badge` | Red count bubble over any content (`Count`, `MaxCount` → `99+`, `Text`). |
| `Avatar` | Initials or an image in a ringed circle (the system's only round element). |
| `Breadcrumb` | `Home › Shipment › Create`. The last item is the current page, and clicking an item raises `ItemClick`. |
| `Pagination` | `‹ 1 … 4 5 6 … 20 ›`, current page in amber. `CurrentPage` (two-way), `PageCount`, `PageChanged`. |
| `FormField` | Label-on-left form row: `Label *:`, input, help text, or red `ErrorText` (also tints the input). |
| `Timeline` / `TimelineItem` | Square-marker event list. |
| `EmptyState` | Icon well, "No Data Available", description, optional action. |
| `RetroIcon` | One of the 44 icons (`Kind`, `Size`). |
| `RetroMessageBox` | Drop-in `MessageBox.Show` replacement using the beveled dialog. |

### Attached properties (`retro:Assist`)

- `Assist.Icon="Save"`: puts an icon before the content of a Button, ToggleButton or MenuItem.
- `Assist.IsError="True"`: turns an input pink (inherited, so it can be set on a container).
- `Assist.Placeholder="Enter carrier…"`: italic hint text in an empty TextBox or ComboBox.

### Styles you can opt into

`Retro.ToolbarButton`, `Retro.IconButton`, `Retro.LinkButton`, `Retro.DataGridCheckBox` (use as `DataGridCheckBoxColumn.ElementStyle`), `Retro.Hyperlink.Visited`, `Retro.VerticalSeparator`, and the type ramp `Retro.Text.H1`, `H2`, `H3`, `Body`, `Bold`, `Help`, `Mono`.

## Design tokens

All tokens are resources keyed `Retro.*`. Override any of them in your application resources:

| Token | Value | Use |
| --- | --- | --- |
| `Retro.Surface` | `#D6E4F1` | the signature desktop wash |
| `Retro.Surface2` / `Surface3` | `#EAF1F8` / `#F5F8FB` | panels / insets |
| `Retro.HeaderBlue1…3`, `HeaderGloss` | `#003D7A` `#2A6FB8` `#4A7FB8` `#6FA3D6` | title bar, active tab, menus |
| `Retro.Accent` | `#F0AB00` | selection, active row, current page |
| `Retro.AccentSoft` / `AccentPale` | `#FFE8A8` / `#FFF6D9` | hover |
| `Retro.BorderLight` / `BorderMid` / `BorderDark` | `#FFFFFF` / `#A6B4C5` / `#6E7A89` | bevels and lines |
| `Retro.StatusError` / `Warning` / `Info` / `Success` | `#C8281E` `#E8A100` `#1F6BB8` `#2E8B3E` | status |
| `Retro.Gradient.TitleBar`, `Toolbar`, `Button`, `ButtonHover`, `ButtonPressed` | | gradients |
| `Retro.FontFamily` | Microsoft Sans Serif, Tahoma | 11px body (`Retro.FontSize.XS…3XL` = 10–20) |

The same colors are available in code as `Retro.Wpf.RetroColors`.

> **Font:** the theme uses the *Microsoft Sans Serif* font that ships with Windows. The package does not redistribute any font files.

## Design rules (from the design system)

- Corners are always square (radius 0). The avatar is the only round element.
- Depth comes from 1px two-tone bevels only: no blur and no soft drop shadows.
- Amber `#F0AB00` marks selection, focus and the primary highlight.
- Layout is dense, on a 4px grid, with labels on the left.
- There is almost no motion. Buttons swap their bevel when pressed, and the only animation is the indeterminate progress bar.

## Demo

`samples/Retro.Wpf.Demo` recreates the design's **Shipment Maintenance** transaction (menu bar, application toolbar, display-mode banner, breadcrumb, Header / Items / Documents tabs, status bar and confirm dialogs). It also includes a **Component Gallery**, under *Goto › Component Gallery*.

```
dotnet run --project samples/Retro.Wpf.Demo
```

![Items tab](https://raw.githubusercontent.com/owen800q/Retro.NET/main/docs/screenshots/shipment-items.png)

![RetroMessageBox](https://raw.githubusercontent.com/owen800q/Retro.NET/main/docs/screenshots/dialog.png)

![Component gallery](https://raw.githubusercontent.com/owen800q/Retro.NET/main/docs/screenshots/gallery.png)

## Showcase

### [RetroCap](https://github.com/owen800q/RetroCap)

A QQ-style screenshot and annotation tool for Windows 10 and 11. Press **Ctrl+Alt+A** anywhere, pick a region, then mark it up with rectangles, arrows, text, a highlighter or mosaic, and copy it, save it or pin it to the screen. Its UI follows the same Retro SAP GUI design system.

![RetroCap annotating a capture](https://raw.githubusercontent.com/owen800q/RetroCap/master/docs/screenshot-annotate.png)

*Built something with Retro.NET? Open a pull request to add it here.*

## Building

```
dotnet build Retro.NET.slnx
dotnet test tests/Retro.Wpf.Tests      # Windows only
```

The tests render every control and both demo screens off-screen. They check pixels (bevel colors, selection colors, sizes) and write PNG snapshots. CI uploads those snapshots as the `ui-snapshots` artifact. Each green build of `main` is published to nuget.org as `1.0.<run>` through NuGet trusted publishing.

The original design handoff (tokens, component CSS, preview cards and the reference board) is kept in [`design/`](design/).

## License

MIT. This is an original design inspired by classic enterprise GUIs. It is not affiliated with or endorsed by SAP SE or Microsoft.
