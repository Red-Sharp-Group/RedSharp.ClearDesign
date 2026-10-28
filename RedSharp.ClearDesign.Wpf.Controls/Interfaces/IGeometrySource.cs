using System.Windows.Media;
using System.Windows;
using RedSharp.ClearDesign.Wpf.Controls.Enums;

namespace RedSharp.ClearDesign.Wpf.Controls.Interfaces
{
    /// <summary>
    /// Provides an interface to describe the source of a geometry.
    /// </summary>
    public interface IGeometrySource
    {
        /// <summary>
        /// Gets or sets the resulting geometry after the latest UpdateGeometry().
        /// </summary>
        Geometry Geometry { get; }

        /// <summary>
        /// Gets the bounding box where the geometry should stretch to.
        /// The actual geometry might be smaller or larger than this.
        /// </summary>
        Rect LogicalBounds { get; }

        /// <summary>
        /// Gets the actual bounds of FrameworkElement.
        /// </summary>
        Rect LayoutBounds { get; }

        /// <summary>
        /// Notifies that the geometry has been invalidated because of external changes.
        /// </summary>
        bool InvalidateGeometry(InvalidateReasons reasons);

        /// <summary>
        /// Update the geometry using the given parameters and the layout bounds.
        /// Returns false if nothing has been updated.
        /// </summary>
        bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds);
    }
}
