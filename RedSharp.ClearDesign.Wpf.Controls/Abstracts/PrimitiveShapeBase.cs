using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using RedSharp.ClearDesign.Wpf.Controls.Enums;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;
using RedSharp.ClearDesign.Wpf.Controls.Interfaces;
using RedSharp.ClearDesign.Wpf.Controls.Utils;

namespace RedSharp.ClearDesign.Wpf.Controls.Abstracts
{
    /// <summary>
    /// Platform-neutral implementation of Shape deriving from WPF:Shape or SL:Path.
    /// </summary>
    /// <summary>
    /// Provides the WPF implementation of Shape that derives from the platform Shape.
    /// </summary>
    public abstract class PrimitiveShapeBase : Shape, IGeometrySourceParameters, IPrimitiveShape
    {
        private IGeometrySource _geometrySource;

        static PrimitiveShapeBase()
        {
            StretchProperty.OverrideMetadata(typeof(PrimitiveShapeBase), new DrawingMetadata(Stretch.Fill, MetadataOptions.AffectsRender));
            StrokeThicknessProperty.OverrideMetadata(typeof(PrimitiveShapeBase), new DrawingMetadata(1.0, MetadataOptions.AffectsRender));
        }

        /// <inheritdoc/>
        public event EventHandler RenderedGeometryChanged;

        private IGeometrySource GeometrySource => _geometrySource ?? (_geometrySource = CreateGeometrySource());

        /// <inheritdoc/>
        public Thickness GeometryMargin => RenderedGeometry == null ? new Thickness() : GeometrySource.LogicalBounds.Subtract(RenderedGeometry.Bounds);

        /// <inheritdoc/>
        public void InvalidateGeometry(InvalidateReasons reasons)
        {
            if (!GeometrySource.InvalidateGeometry(reasons))
                return;

            InvalidateArrange();
        }

        private void RealizeGeometry()
        {
            if (RenderedGeometryChanged == null)
                return;

            RenderedGeometryChanged(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(StrokeThickness, StrokeThickness);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (GeometrySource.UpdateGeometry(this, finalSize.GetBounds()))
                RealizeGeometry();

            base.ArrangeOverride(finalSize);

            return finalSize;
        }

        /// <inheritdoc/>
        protected override sealed Geometry DefiningGeometry => GeometrySource.Geometry ?? Geometry.Empty;

        /// <summary>
        /// Extends how the shape is drawn with creating geometry source.
        /// </summary>
        protected abstract IGeometrySource CreateGeometrySource();
    }
}
