using System;

namespace RedSharp.ClearDesign.Wpf.Controls.Enums
{
    [Flags]
    public enum MetadataOptions
    {
        None = 0,
        AffectsMeasure = 1,
        AffectsRender = 16,
    }
}
