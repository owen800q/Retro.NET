using System;
using System.Windows;

namespace Retro.Wpf
{
    /// <summary>
    /// Merges the complete Retro theme (tokens, icons and styles for every standard WPF control).
    /// <code>
    /// &lt;Application.Resources&gt;
    ///   &lt;ResourceDictionary&gt;
    ///     &lt;ResourceDictionary.MergedDictionaries&gt;
    ///       &lt;retro:RetroTheme /&gt;
    ///     &lt;/ResourceDictionary.MergedDictionaries&gt;
    ///   &lt;/ResourceDictionary&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// </summary>
    public class RetroTheme : ResourceDictionary
    {
        /// <summary>Pack URI of the full theme dictionary.</summary>
        public static readonly Uri ThemeUri = new Uri("pack://application:,,,/Retro.Wpf;component/Themes/Retro.xaml", UriKind.Absolute);

        /// <summary>Pack URI of the tokens-only dictionary (colors, brushes, fonts, spacing).</summary>
        public static readonly Uri TokensUri = new Uri("pack://application:,,,/Retro.Wpf;component/Themes/Tokens.xaml", UriKind.Absolute);

        public RetroTheme()
        {
            Source = ThemeUri;
        }
    }
}
