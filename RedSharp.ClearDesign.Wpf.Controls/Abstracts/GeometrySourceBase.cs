using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using RedSharp.ClearDesign.Wpf.Controls.Enums;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;
using RedSharp.ClearDesign.Wpf.Controls.Interfaces;

namespace RedSharp.ClearDesign.Wpf.Controls.Abstracts
{
    /// <summary>
    /// Provides the base class of a source of geometry.
    /// Generates and caches the geometry based on the input parameters and the layout bounds.
    /// </summary>
    public abstract class GeometrySourceBase<TParameters> : IGeometrySource where TParameters : IGeometrySourceParameters
    {
        private bool _geometryInvalidated;
        protected Geometry _cachedGeometry;

        /// <summary>
        /// Gets or sets the resulting geometry after the latest UpdateGeometry().
        /// </summary>
        public Geometry Geometry { get; private set; }

        /// <summary>
        /// Gets the bounding box that the geometry should stretch to.
        /// The actual geometry might be smaller or larger than this.
        /// <see cref="LogicalBounds" /> should already take stroke thickness and stretch into consideration.
        /// </summary>
        public Rect LogicalBounds { get; private set; }

        /// <summary>
        /// Gets the actual bounds of FrameworkElement.
        /// <see cref="LayoutBounds" /> includes logical bounds, stretch and stroke thickness.
        /// </summary>
        public Rect LayoutBounds { get; private set; }

        /// <summary>
        /// Notifies that the geometry has been invalidated because of external changes.
        /// </summary>
        /// <remarks>
        /// The geometry is typically invalidated when parameters are changed.
        /// If any geometry has been invalidated externally, the geometry will be recomputed regardless if the layout bounds change.
        /// </remarks>
        public bool InvalidateGeometry(InvalidateReasons reasons)
        {
            if ((reasons & InvalidateReasons.TemplateChanged) != 0)
                _cachedGeometry = null;

            if (_geometryInvalidated)
                return false;

            _geometryInvalidated = true;

            return true;
        }

        /// <summary>
        /// Update the geometry based on the given parameters and layoutBounds.
        /// Returns false if the geometry hasn't been changed.
        /// </summary>
        public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
        {
            var flag = false;

            if (parameters is TParameters)
            {
                var logicalBounds = ComputeLogicalBounds(layoutBounds, parameters);

                flag = LayoutBounds != layoutBounds || LayoutBounds != logicalBounds;

                if (_geometryInvalidated || flag)
                {
                    LayoutBounds = layoutBounds;
                    LogicalBounds = logicalBounds;

                    UpdateCachedGeometry((TParameters)parameters);

                    if (Geometry != _cachedGeometry)
                        Geometry = _cachedGeometry;
                }
            }

            _geometryInvalidated = false;

            return flag;
        }

        private static double GetHalfStrokeThickness(IGeometrySourceParameters parameter)
        {
            if (parameter.Stroke != null)
            {
                var strokeThickness = parameter.StrokeThickness;

                if (!double.IsNaN(strokeThickness) && !double.IsInfinity(strokeThickness))
                    return Math.Abs(strokeThickness) / 2.0;
            }

            return 0.0;
        }

        /// <summary>
        /// Extends the way to provide geometry by implementing this function.
        /// Returns true when any of the geometry is changed.
        /// </summary>
        protected abstract bool UpdateCachedGeometry(TParameters parameters);

        /// <summary>
        /// Extends the way to handle stretch mode.
        /// The default is to always use Stretch.Fill and center stroke.
        /// </summary>
        protected virtual Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
        {
            return GeometryHelper.Inflate(layoutBounds, -GetHalfStrokeThickness(parameters));
        }
    }
}
