using System.Windows;
using RedSharp.ClearDesign.Wpf.Controls.Abstracts;
using RedSharp.ClearDesign.Wpf.Controls.Enums;
using RedSharp.ClearDesign.Wpf.Controls.Interfaces;
using RedSharp.ClearDesign.Wpf.Controls.Utils;

namespace RedSharp.ClearDesign.Wpf.Controls.Shapes
{
    /// <summary>
    /// Renders an arc shape supporting Arc, Ring, and Pie mode controlled by ArcThickness.
    /// </summary>
    public sealed class Arc : PrimitiveShapeBase, IArcGeometrySourceParameters
    {
        public static readonly DependencyProperty StartAngleProperty;
        public static readonly DependencyProperty EndAngleProperty;
        public static readonly DependencyProperty ArcThicknessProperty;
        public static readonly DependencyProperty ArcThicknessUnitProperty;

        static Arc()
        {
            StartAngleProperty = DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc), new DrawingMetadata(0.0, MetadataOptions.AffectsRender));
            EndAngleProperty = DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc), new DrawingMetadata(90.0, MetadataOptions.AffectsRender));
            ArcThicknessProperty = DependencyProperty.Register(nameof(ArcThickness), typeof(double), typeof(Arc), new DrawingMetadata(0.0, MetadataOptions.AffectsRender));
            ArcThicknessUnitProperty = DependencyProperty.Register(nameof(ArcThicknessUnit), typeof(UnitType), typeof(Arc), new DrawingMetadata(UnitType.Pixel, MetadataOptions.AffectsRender));
        }

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>
        /// The start angle in degrees. Zero degrees is pointing up.
        /// </value>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        /// <value>
        /// The end angle in degrees. Zero degrees is pointing up.
        /// </value>
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        /// <summary>
        /// Gets or sets the arc thickness.
        /// </summary>
        /// <value>
        /// The arc thickness in pixels or percentage depending on "ArcThicknessUnit".
        /// </value>
        public double ArcThickness
        {
            get => (double)GetValue(ArcThicknessProperty);
            set => SetValue(ArcThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the arc thickness unit.
        /// </summary>
        /// <value>
        /// The arc thickness unit in pixels or percentage.
        /// </value>
        public UnitType ArcThicknessUnit
        {
            get => (UnitType)GetValue(ArcThicknessUnitProperty);
            set => SetValue(ArcThicknessUnitProperty, value);
        }

        protected override IGeometrySource CreateGeometrySource()
        {
            return new ArcGeometrySource();
        }
    }
}
