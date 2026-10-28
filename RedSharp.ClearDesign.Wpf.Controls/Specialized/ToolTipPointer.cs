using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;

namespace RedSharp.ClearDesign.Wpf.Controls.Specialized
{
    public class ToolTipPointer : FrameworkElement
    {
        private static readonly Geometry _mask;
        private static readonly Geometry _line;
        private static readonly Geometry _shape;


        private Pen _outerStrokePen = null;
        private Pen _innerStrokePen = null;
        private Pen _backgroundStrokePen = null;

        public static readonly DependencyProperty FillProperty;

        public static readonly DependencyProperty OuterStrokeProperty;
        public static readonly DependencyProperty OuterStrokeThicknessProperty;

        public static readonly DependencyProperty InnerStrokeProperty;
        public static readonly DependencyProperty InnerStrokeThicknessProperty;

        public static readonly DependencyProperty DrawBackgroundUnderBorderProperty;

        static ToolTipPointer()
        {
            _mask = Geometry.Parse("M11 6L17.8281 12.8284C18.5781 13.5786 19.5957 14 20.6562 14H22V0H0V14H1.34277C2.4043 14 3.4209 13.5786 4.17188 12.8284L11 6Z");
            _line = Geometry.Parse("M22 14H20.6562C19.5957 14 18.5781 13.5786 17.8281 12.8284L11 6L4.17188 12.8284C3.4209 13.5786 2.4043 14 1.34277 14H0");
            _shape = Geometry.Parse("M11 6L17.8281 12.8284C18.5781 13.5786 19.5957 14 20.6562 14H22V19H0V14H1.34277C2.4043 14 3.4209 13.5786 4.17188 12.8284L11 6Z");

            var affectMeasureAndRender = FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender;

            FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(ToolTipPointer),
                       new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, OnFillChanged)); 
            
            OuterStrokeProperty = DependencyProperty.Register(nameof(OuterStroke), typeof(Brush), typeof(ToolTipPointer),
                        new FrameworkPropertyMetadata(null, affectMeasureAndRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, OnOuterPenChanged));
            
            OuterStrokeThicknessProperty = DependencyProperty.Register(nameof(OuterStrokeThickness), typeof(double), typeof(ToolTipPointer),
                        new FrameworkPropertyMetadata(1.0d, affectMeasureAndRender, OnOuterPenChanged));

            InnerStrokeProperty = DependencyProperty.Register(nameof(InnerStroke), typeof(Brush), typeof(ToolTipPointer),
                        new FrameworkPropertyMetadata(null, affectMeasureAndRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, OnInnerPenChanged));

            InnerStrokeThicknessProperty = DependencyProperty.Register(nameof(InnerStrokeThickness), typeof(double), typeof(ToolTipPointer),
                        new FrameworkPropertyMetadata(1.0d, affectMeasureAndRender, OnInnerPenChanged));

            DrawBackgroundUnderBorderProperty = DependencyProperty.Register(nameof(DrawBackgroundUnderBorder), typeof(bool), typeof(ToolTipPointer),
                new FrameworkPropertyMetadata(false, affectMeasureAndRender));
        }
        
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public Brush OuterStroke
        {
            get { return (Brush)GetValue(OuterStrokeProperty); }
            set { SetValue(OuterStrokeProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double OuterStrokeThickness
        {
            get { return (double)GetValue(OuterStrokeThicknessProperty); }
            set { SetValue(OuterStrokeThicknessProperty, value); }
        }

        public Brush InnerStroke
        {
            get { return (Brush)GetValue(InnerStrokeProperty); }
            set { SetValue(InnerStrokeProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double InnerStrokeThickness
        {
            get { return (double)GetValue(InnerStrokeThicknessProperty); }
            set { SetValue(InnerStrokeThicknessProperty, value); }
        }

        public bool DrawBackgroundUnderBorder
        {
            get => (bool)GetValue(DrawBackgroundUnderBorderProperty);
            set => SetValue(DrawBackgroundUnderBorderProperty, value);
        }

        private static void OnOuterPenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ToolTipPointer)d)._outerStrokePen = null;
        }

        private static void OnInnerPenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ToolTipPointer)d)._innerStrokePen = null;
        }

        private static void OnFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ToolTipPointer)d)._backgroundStrokePen = null;
        }

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    return new Size(22, 21);
        //}

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    return new Size(22, 21);
        //}

        protected override void OnRender(DrawingContext drawingContext)
        {
            var outerStroke = OuterStrokeThickness * 2;
            var innerStroke = InnerStrokeThickness * 2;

            var fillBrush = Fill;

            if (_outerStrokePen == null && !DoubleUtil.AreClose(outerStroke, 0.0d))
                _outerStrokePen = new Pen(OuterStroke, outerStroke);

            if (_backgroundStrokePen == null && !DoubleUtil.AreClose(outerStroke, 0.0d))
                _backgroundStrokePen = new Pen(fillBrush, outerStroke);

            if (_innerStrokePen == null && !DoubleUtil.AreClose(innerStroke, 0.0d))
                _innerStrokePen = new Pen(InnerStroke, innerStroke);

            if (_outerStrokePen == null && _innerStrokePen == null && fillBrush == null)
                return;

            if (_backgroundStrokePen != null)
                drawingContext.DrawGeometry(null, _backgroundStrokePen, _line);

            if (_outerStrokePen != null)
            {
                drawingContext.PushClip(_mask);
                drawingContext.DrawGeometry(null, _outerStrokePen, _line);
                drawingContext.Pop();
            }

            if (fillBrush != null)
                drawingContext.DrawGeometry(fillBrush, null, _shape);

            if (_innerStrokePen != null)
            {
                drawingContext.PushClip(_shape);
                drawingContext.DrawGeometry(null, _innerStrokePen, _line);
                drawingContext.Pop();
            }
        }
    }
}
