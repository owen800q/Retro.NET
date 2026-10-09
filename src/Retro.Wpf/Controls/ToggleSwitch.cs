using System.Windows;
using System.Windows.Controls.Primitives;

namespace Retro.Wpf.Controls
{
    /// <summary>The beveled on/off switch: sunken track, raised knob, amber when on. Switches instantly.</summary>
    public class ToggleSwitch : ToggleButton
    {
        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
        }
    }
}
