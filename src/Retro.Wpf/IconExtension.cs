using System;
using System.Windows.Markup;
using System.Windows.Media;
using Retro.Wpf.Controls;

namespace Retro.Wpf
{
    /// <summary>
    /// Markup extension returning an icon as an <see cref="ImageSource"/>:
    /// <c>&lt;Image Source="{retro:Icon Save}" /&gt;</c>.
    /// </summary>
    [MarkupExtensionReturnType(typeof(ImageSource))]
    public class IconExtension : MarkupExtension
    {
        public IconExtension() { }

        public IconExtension(IconKind kind) { Kind = kind; }

        [ConstructorArgument("kind")]
        public IconKind Kind { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider) => RetroIcons.Get(Kind);
    }
}
