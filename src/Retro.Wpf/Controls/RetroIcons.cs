using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Retro.Wpf.Controls
{
    /// <summary>Access to the icon set as frozen <see cref="DrawingImage"/>s (16×16 DIP bounds).</summary>
    public static class RetroIcons
    {
        private static readonly Uri IconsUri = new Uri("/Retro.Wpf;component/Themes/Icons.xaml", UriKind.Relative);
        private static ResourceDictionary? _dictionary;
        private static readonly Dictionary<IconKind, DrawingImage> Cache = new Dictionary<IconKind, DrawingImage>();

        /// <summary>Resource key of an icon, e.g. <c>Retro.Icon.Save</c>.</summary>
        public static string GetResourceKey(IconKind kind) => "Retro.Icon." + kind;

        /// <summary>Returns the image for <paramref name="kind"/>.</summary>
        public static DrawingImage Get(IconKind kind)
        {
            lock (Cache)
            {
                if (Cache.TryGetValue(kind, out DrawingImage? image))
                    return image;

                _dictionary ??= (ResourceDictionary)Application.LoadComponent(IconsUri);
                image = (DrawingImage)_dictionary[GetResourceKey(kind)];
                Cache[kind] = image;
                return image;
            }
        }
    }
}
