using System;
using System.Windows.Media;
using System.Windows;
using RedSharp.ClearDesign.Wpf.Controls.Enums;

namespace RedSharp.ClearDesign.Wpf.Controls.Interfaces
{
    /// <summary>
    /// Provides the necessary interface to define a Shape.
    /// </summary>
    public interface IPrimitiveShape
    {
        /// <summary>
        /// Occurs when RenderedGeometry is changed.
        /// </summary>
        event EventHandler RenderedGeometryChanged;

        /// <summary>
        /// Gets or sets the <see cref="Brush" /> that specifies how to paint the interior of the shape.
        /// </summary>
        Brush Fill { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush" /> that specifies how the <see cref="Shape" /> outline is painted.
        /// </summary>
        Brush Stroke { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="Shape" /> stroke outline. 
        /// </summary>
        double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Stretch" /> enumeration value that describes how the shape fills its allocated space.
        /// </summary>
        Stretch Stretch { get; set; }

        /// <summary>
        /// Gets the rendered geometry presented by the rendering engine.
        /// </summary>
        Geometry RenderedGeometry { get; }

        /// <summary>
        /// Gets the margin between logical bounds and actual geometry bounds.
        /// </summary>
        Thickness GeometryMargin { get; }

        /// <summary>
        /// Invalidates the geometry for a <see cref="IPrimitiveShape" />. 
        /// After the invalidation, the <see cref="IPrimitiveShape" /> will recompute the geometry, which will occur asynchronously.
        /// </summary>
        void InvalidateGeometry(InvalidateReasons reasons);
    }
}
