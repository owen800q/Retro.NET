namespace Retro.Wpf.Controls
{
    /// <summary>The 1px two-tone border styles of the design system.</summary>
    public enum BevelKind
    {
        /// <summary>No border.</summary>
        None,
        /// <summary><c>--bevel-out</c>: white top/left, dark bottom/right (1px).</summary>
        Raised,
        /// <summary><c>--bevel-out-strong</c>: raised plus a 1px black outline (2px).</summary>
        RaisedStrong,
        /// <summary><c>--bevel-in</c>: dark top/left, white bottom/right (1px).</summary>
        Sunken,
        /// <summary><c>--bevel-in-strong</c>: sunken plus a 1px black outline (2px).</summary>
        SunkenStrong,
        /// <summary><c>--bevel-flat</c>: a single 1px mid-gray line (1px).</summary>
        Flat,
        /// <summary><c>--bevel-fieldset</c>: dark outer line, white inner line (2px).</summary>
        Fieldset,
    }
}
