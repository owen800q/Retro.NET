using System.Windows.Media;

namespace Retro.Wpf
{
    /// <summary>
    /// The Retro SAP GUI palette as <see cref="Color"/> constants. These are the default
    /// values of the <c>Retro.*Color</c> resources in <c>Themes/Tokens.xaml</c>.
    /// </summary>
    public static class RetroColors
    {
        public static readonly Color Surface = Hex(0xD6E4F1);
        public static readonly Color Surface2 = Hex(0xEAF1F8);
        public static readonly Color Surface3 = Hex(0xF5F8FB);
        public static readonly Color SurfaceInset = Hex(0xFFFFFF);
        public static readonly Color SurfaceToolbar = Hex(0xDCE6F2);

        public static readonly Color HeaderBlue1 = Hex(0x003D7A);
        public static readonly Color HeaderBlue2 = Hex(0x2A6FB8);
        public static readonly Color HeaderBlue3 = Hex(0x4A7FB8);
        public static readonly Color HeaderGloss = Hex(0x6FA3D6);
        public static readonly Color TitleBarBorder = Hex(0x001E3F);

        public static readonly Color Accent = Hex(0xF0AB00);
        public static readonly Color Accent2 = Hex(0xE8A100);
        public static readonly Color AccentSoft = Hex(0xFFE8A8);
        public static readonly Color AccentPale = Hex(0xFFF6D9);

        public static readonly Color BorderLight = Hex(0xFFFFFF);
        public static readonly Color BorderMid = Hex(0xA6B4C5);
        public static readonly Color BorderDark = Hex(0x6E7A89);
        public static readonly Color BorderDarker = Hex(0x4A5566);
        public static readonly Color BorderBlack = Hex(0x000000);

        public static readonly Color Foreground = Hex(0x000000);
        public static readonly Color ForegroundMuted = Hex(0x4A5566);
        public static readonly Color ForegroundDisabled = Hex(0x8A95A5);

        public static readonly Color Link = Hex(0x1F6BB8);
        public static readonly Color LinkVisited = Hex(0x6B3F8A);
        public static readonly Color LinkHover = Hex(0xC8281E);

        public static readonly Color StatusError = Hex(0xC8281E);
        public static readonly Color StatusErrorBackground = Hex(0xFBE5E2);
        public static readonly Color StatusWarning = Hex(0xE8A100);
        public static readonly Color StatusWarningBackground = Hex(0xFFF4D6);
        public static readonly Color StatusInfo = Hex(0x1F6BB8);
        public static readonly Color StatusInfoBackground = Hex(0xE0EDF8);
        public static readonly Color StatusSuccess = Hex(0x2E8B3E);
        public static readonly Color StatusSuccessBackground = Hex(0xE2F1E5);

        public static readonly Color InputFocusBackground = Hex(0xFFFFE1);
        public static readonly Color InputErrorBackground = Hex(0xFFEAE7);

        private static Color Hex(int rgb) =>
            Color.FromRgb((byte)((rgb >> 16) & 0xFF), (byte)((rgb >> 8) & 0xFF), (byte)(rgb & 0xFF));

        internal static SolidColorBrush Frozen(Color c)
        {
            var b = new SolidColorBrush(c);
            b.Freeze();
            return b;
        }
    }
}
