using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Reflection;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;

namespace RedSharp.ClearDesign.Wpf.Controls.Specialized
{
    /// <summary>
    /// The Border decorator is used to draw a border and/or background around another element.
    /// </summary>
    public class LayeredBorder : Decorator
    {
        protected struct BorderParameters
        {
            public Rect OuterRectangle;
            public Rect InnerRectangle;
            public Radii OuterRadii;
            public Radii InnerRadii;
        }

        protected struct Radii
        {
            public double LeftTop;
            public double TopLeft;
            public double TopRight;
            public double RightTop;
            public double RightBottom;
            public double BottomRight;
            public double BottomLeft;
            public double LeftBottom;
        }

        public static readonly DependencyProperty BackgroundProperty;

        public static readonly DependencyProperty BorderBrushProperty;
        public static readonly DependencyProperty BorderThicknessProperty;

        public static readonly DependencyProperty PaddingProperty;

        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty ActualCornerRadiusProperty;

        public static readonly DependencyProperty UseOuterCornerRadiusProperty;

        public static readonly DependencyProperty EnableChildClippingProperty;

        private bool _useComplexRenderCodePath;

        private StreamGeometry _borderGeometryCache;
        private StreamGeometry _backgroundGeometryCache;

        static LayeredBorder()
        {
            var affectMeasureAndRender = FrameworkPropertyMetadataOptions.AffectsMeasure | 
                                         FrameworkPropertyMetadataOptions.AffectsRender;

            var affectRenderWithoutSubProperties = FrameworkPropertyMetadataOptions.AffectsRender | 
                                                   FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender;

            BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(LayeredBorder),
                new FrameworkPropertyMetadata(null, affectMeasureAndRender));

            BorderBrushProperty = DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(null, affectRenderWithoutSubProperties));

            BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(new Thickness(), affectMeasureAndRender), new ValidateValueCallback(IsThicknessValid));


            PaddingProperty = DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(new Thickness(), affectMeasureAndRender), new ValidateValueCallback(IsThicknessValid));


            CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(new CornerRadius(), affectMeasureAndRender), new ValidateValueCallback(IsCornerRadiusValid));

            ActualCornerRadiusProperty = DependencyProperty.Register(nameof(ActualCornerRadius), typeof(CornerRadius), typeof(LayeredBorder));


            UseOuterCornerRadiusProperty = DependencyProperty.Register(nameof(UseOuterCornerRadius), typeof(bool), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(true, affectMeasureAndRender));

            EnableChildClippingProperty = DependencyProperty.Register(nameof(EnableChildClipping), typeof(bool), typeof(LayeredBorder),
                new FrameworkPropertyMetadata(false, EnableAutoClipChanged));
        }


        /// <summary>
        /// The Background property defines the brush used to fill the area within the border.
        /// </summary>
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// The BorderBrush property defines the brush used to fill the border region.
        /// </summary>
        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        /// <summary>
        /// The BorderThickness property defined how thick a border to draw.  The property's value is a
        /// <see cref="Thickness" /> containing values for each of the Left, Top, Right,
        /// and Bottom sides.  Values of Auto are interpreted as zero.
        /// </summary>
        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }


        /// <summary>
        /// The Padding property inflates the effective size of the child by the specified thickness.  This
        /// achieves the same effect as adding margin on the child, but is present here for convenience.
        /// </summary>
        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }


        /// <summary>
        /// The CornerRadius property allows users to control the roundness of the corners independently by
        /// setting a radius value for each corner.  Radius values that are too large are scaled so that they
        /// smoothly blend from corner to corner.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// The actual corner radius that is used for the rendering. 
        /// <br/>Each value is limited by the half of minimal dimension of the border (width/height). 
        /// That allows to keep the border in a "circle" shape instead of "oval" shape
        /// </summary>
        public CornerRadius ActualCornerRadius
        {
            get => (CornerRadius)GetValue(ActualCornerRadiusProperty);
            private set => SetValue(ActualCornerRadiusProperty, value);
        }


        /// <summary>
        /// Determines the point from the corner radius is measured.
        /// <br/>By default in the <see cref="Border"/> measuring for the corner radius 
        /// is applied for the inner shape (like if this property is false), so on the edge of the border
        /// the corner radius will be bigger that the value in the <see cref="CornerRadius"/>
        /// </summary>
        /// <remarks>
        /// It is True by default
        /// </remarks>
        public bool UseOuterCornerRadius
        {
            get => (bool)GetValue(UseOuterCornerRadiusProperty);
            set => SetValue(UseOuterCornerRadiusProperty, value);
        }

        /// <summary>
        /// Creates and push geometry for clipping child content
        /// </summary>
        public bool EnableChildClipping
        {
            get => (bool)GetValue(EnableChildClippingProperty);
            set => SetValue(EnableChildClippingProperty, value);
        }

        private static bool IsThicknessValid(object value)
        {
            var thickness = (Thickness)value;

            return thickness.IsValid(false, false, false, false);
        }

        private static bool IsCornerRadiusValid(object value)
        {
            var radius = (CornerRadius)value;

            return radius.IsValid(false, false, false, false);
        }

        /// <summary>
        /// Helper function to add up the left and right size as width, as well as the top and bottom size as height
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Size CollapseThickness(Thickness thickness)
        {
            return new Size(thickness.Left + thickness.Right, thickness.Top + thickness.Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreUniformCorners(CornerRadius radius)
        {
            double topLeft = radius.TopLeft;

            return DoubleUtil.AreClose(topLeft, radius.TopRight) &&
                   DoubleUtil.AreClose(topLeft, radius.BottomLeft) &&
                   DoubleUtil.AreClose(topLeft, radius.BottomRight);
        }

        private static void EnableAutoClipChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arguments)
        {
            ((LayeredBorder)obj).OnEnableAutoClipChanged((bool)arguments.NewValue, (bool)arguments.OldValue);
        }

        /// <summary>
        /// Helper to deflate rectangle by thickness
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static Rect DeflateRect(Rect reactangle, Thickness thickness)
        {
            return new Rect(reactangle.Left + thickness.Left,
                            reactangle.Top + thickness.Top,
                            Math.Max(0.0, reactangle.Width - thickness.Left - thickness.Right),
                            Math.Max(0.0, reactangle.Height - thickness.Top - thickness.Bottom));
        }

        protected virtual void OnEnableAutoClipChanged(bool newValue, bool oldValue)
        {
            if (Child == null)
                return;

            if (newValue)
            {
                var border = GetActualBorder();
                var innerRectangle = DeflateRect(new Rect(RenderSize), border);

                if (innerRectangle.IsEmpty)
                    return;

                var useOuterCornerRadius = UseOuterCornerRadius;

                var actualCorners = GetActualCornerRadius(RenderSize, border, useOuterCornerRadius);
                var innerRadii = CalculateCornerRadii(actualCorners, border, false, useOuterCornerRadius);

                TryPushClipGeometryOnChildElement(innerRectangle, innerRadii);
            }
            else
            {
                Child.Clip = null;
            }
        }

        protected virtual void TryPushClipGeometryOnChildElement(Rect innerRectangle, Radii innerRadii)
        {
            var childRectangle = new Rect(0.0, 0.0, innerRectangle.Width, innerRectangle.Height);

            if (_useComplexRenderCodePath)  //Complex shape
            {
                var geometry = new StreamGeometry();

                using (var context = geometry.Open())
                    GenerateGeometry(context, childRectangle, innerRadii);

                geometry.Freeze();

                Child.Clip = geometry;
            }
            else
            {
                Child.Clip = new RectangleGeometry(childRectangle, innerRadii.TopLeft, innerRadii.TopLeft);
            }
        }

        /// <summary>
        /// Generates a StreamGeometry.
        /// </summary>
        /// <param name="context">An already opened StreamGeometryContext.</param>
        /// <param name="rectangle">Rectangle for geometry conversion.</param>
        /// <param name="innerRadius">Corner radii.</param>
        /// <returns>Result geometry.</returns>
        protected static void GenerateGeometry(StreamGeometryContext context, Rect rectangle, Radii innerRadius)
        {
            //  compute the coordinates of the key points`

            Point topLeft = new Point(innerRadius.LeftTop, 0);
            Point topRight = new Point(rectangle.Width - innerRadius.RightTop, 0);
            Point rightTop = new Point(rectangle.Width, innerRadius.TopRight);
            Point rightBottom = new Point(rectangle.Width, rectangle.Height - innerRadius.BottomRight);
            Point bottomRight = new Point(rectangle.Width - innerRadius.RightBottom, rectangle.Height);
            Point bottomLeft = new Point(innerRadius.LeftBottom, rectangle.Height);
            Point leftBottom = new Point(0, rectangle.Height - innerRadius.BottomLeft);
            Point leftTop = new Point(0, innerRadius.TopLeft);

            //  check keypoints for overlap and resolve by partitioning radii according to
            //  the percentage of each one.  

            //  top edge is handled here
            if (topLeft.X > topRight.X)
            {
                double v = innerRadius.LeftTop / (innerRadius.LeftTop + innerRadius.RightTop) * rectangle.Width;
                topLeft.X = v;
                topRight.X = v;
            }

            //  right edge
            if (rightTop.Y > rightBottom.Y)
            {
                double v = innerRadius.TopRight / (innerRadius.TopRight + innerRadius.BottomRight) * rectangle.Height;
                rightTop.Y = v;
                rightBottom.Y = v;
            }

            //  bottom edge
            if (bottomRight.X < bottomLeft.X)
            {
                double v = innerRadius.LeftBottom / (innerRadius.LeftBottom + innerRadius.RightBottom) * rectangle.Width;
                bottomRight.X = v;
                bottomLeft.X = v;
            }

            // left edge
            if (leftBottom.Y < leftTop.Y)
            {
                double v = innerRadius.TopLeft / (innerRadius.TopLeft + innerRadius.BottomLeft) * rectangle.Height;
                leftBottom.Y = v;
                leftTop.Y = v;
            }

            //  add on offsets

            Vector offset = new Vector(rectangle.TopLeft.X, rectangle.TopLeft.Y);
            topLeft += offset;
            topRight += offset;
            rightTop += offset;
            rightBottom += offset;
            bottomRight += offset;
            bottomLeft += offset;
            leftBottom += offset;
            leftTop += offset;

            //  create the border geometry

            context.BeginFigure(topLeft, true /* is filled */, true /* is closed */);

            // Top line
            context.LineTo(topRight, true /* is stroked */, false /* is smooth join */);

            // Upper-right corner
            double radiusX = rectangle.TopRight.X - topRight.X;
            double radiusY = rightTop.Y - rectangle.TopRight.Y;

            if (!DoubleUtil.IsZero(radiusX) || !DoubleUtil.IsZero(radiusY))
                context.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);

            // Right line
            context.LineTo(rightBottom, true /* is stroked */, false /* is smooth join */);

            // Lower-right corner
            radiusX = rectangle.BottomRight.X - bottomRight.X;
            radiusY = rectangle.BottomRight.Y - rightBottom.Y;

            if (!DoubleUtil.IsZero(radiusX) || !DoubleUtil.IsZero(radiusY))
                context.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);

            // Bottom line
            context.LineTo(bottomLeft, true /* is stroked */, false /* is smooth join */);

            // Lower-left corner
            radiusX = bottomLeft.X - rectangle.BottomLeft.X;
            radiusY = rectangle.BottomLeft.Y - leftBottom.Y;

            if (!DoubleUtil.IsZero(radiusX) || !DoubleUtil.IsZero(radiusY))
                context.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);

            // Left line
            context.LineTo(leftTop, true /* is stroked */, false /* is smooth join */);

            // Upper-left corner
            radiusX = topLeft.X - rectangle.TopLeft.X;
            radiusY = leftTop.Y - rectangle.TopLeft.Y;

            if (!DoubleUtil.IsZero(radiusX) || !DoubleUtil.IsZero(radiusY))
                context.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
        }

        /// <summary>
        /// Updates DesiredSize of the Border.  Called by parent UIElement.  This is the first pass of layout.
        /// </summary>
        /// <remarks>
        /// Border determines its desired size it needs from the specified border the child: its sizing
        /// properties, margin, and requested size.
        /// </remarks>
        /// <param name="constraint">Constraint size is an "upper limit" that the return value should not exceed.</param>
        /// <returns>The Decorator's desired size.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var child = Child;
            var result = new Size();
            var borders = BorderThickness;

            if (UseLayoutRounding)
            {
                var dpi = VisualTreeHelper.GetDpi(this);

                borders = new Thickness(DoubleUtil.RoundLayoutValue(borders.Left, dpi.DpiScaleX), 
                                        DoubleUtil.RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                                        DoubleUtil.RoundLayoutValue(borders.Right, dpi.DpiScaleX), 
                                        DoubleUtil.RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }

            // Compute the chrome size added by the various elements
            var border = CollapseThickness(borders);
            var padding = CollapseThickness(Padding);

            //If we have a child
            if (child != null)
            {
                // Combine into total decorating size
                var combined = new Size(border.Width + padding.Width, border.Height + padding.Height);

                // Remove size of border only from child's reference size.
                var childConstraint = new Size(Math.Max(0.0, constraint.Width - combined.Width),
                                               Math.Max(0.0, constraint.Height - combined.Height));


                child.Measure(childConstraint);

                var childSize = child.DesiredSize;

                // Now use the returned size to drive our size, by adding back the margins, etc.
                result.Width = childSize.Width + combined.Width;
                result.Height = childSize.Height + combined.Height;
            }
            else
            {
                // Combine into total decorating size
                result = new Size(border.Width + padding.Width, border.Height + padding.Height);
            }

            return result;
        }

        protected Thickness GetActualBorder()
        {
            var border = BorderThickness;

            if (!border.IsZero() && UseLayoutRounding)
            {
                var dpi = VisualTreeHelper.GetDpi(this);

                border = new Thickness(DoubleUtil.RoundLayoutValue(border.Left, dpi.DpiScaleX),
                                       DoubleUtil.RoundLayoutValue(border.Top, dpi.DpiScaleY),
                                       DoubleUtil.RoundLayoutValue(border.Right, dpi.DpiScaleX),
                                       DoubleUtil.RoundLayoutValue(border.Bottom, dpi.DpiScaleY));
            }

            return border;
        }

        protected CornerRadius GetActualCornerRadius(Size size, Thickness border, bool isOuter)
        {
            var currentCorners = CornerRadius;

            if (currentCorners.IsZero())
                return currentCorners;

            var width = size.Width;
            var height = size.Height;

            if (width == double.NaN)
            {
                width = 0.0;
            }
            else if (!isOuter)
            {
                width = width - border.Left - border.Right;
            }

            if (height == double.NaN)
            {
                height = 0.0;
            }
            else if (!isOuter)
            {
                height = height - border.Top - border.Bottom;
            }

            var radius = Math.Max(Math.Min(width, height), 0.0) / 2;

            return new CornerRadius(Math.Min(currentCorners.TopLeft, radius),
                                    Math.Min(currentCorners.TopRight, radius),
                                    Math.Min(currentCorners.BottomRight, radius),
                                    Math.Min(currentCorners.BottomLeft, radius));
        }
                
        /// <summary>
        /// Border computes the position of its single child and applies its child's alignments to the child.
        /// 
        /// </summary>
        /// <param name="size">The size reserved for this element by the parent</param>
        /// <returns>The actual ink area of the element, typically the same as finalSize</returns>
        protected override Size ArrangeOverride(Size size)
        {
            //====================================================================================//
            //MEASURING AND CACHING

            var border = GetActualBorder();
            var useOuterCornerRadius = UseOuterCornerRadius;

            var outerRectangle = new Rect(size);
            var innerRectangle = DeflateRect(outerRectangle, border);

            var actualCorners = GetActualCornerRadius(size, border, useOuterCornerRadius);

            var innerRadii = CalculateCornerRadii(actualCorners, border, false, useOuterCornerRadius);
            var outerRadii = CalculateCornerRadii(actualCorners, border, true, useOuterCornerRadius);

            ActualCornerRadius = actualCorners;

            //====================================================================================//
            //ACTIONS

            //Arrange child
            var child = Child;

            if (child != null)
                child.Arrange(DeflateRect(innerRectangle, Padding));

            //decide which code path to execute. complex (geometry path based) rendering 
            //is used if one of the following is true:

            //1. there are non-uniform rounded corners
            _useComplexRenderCodePath = !AreUniformCorners(actualCorners);

            if (!_useComplexRenderCodePath)
            {
                var borderBrush = BorderBrush;

                if (borderBrush != null)
                {
                    SolidColorBrush originIndependentBrush = borderBrush as SolidColorBrush;

                    bool uniformBorders = border.IsUniform();

                    _useComplexRenderCodePath =
                    //2. the border brush is origin dependent (the only origin independent brush is a solid color brush)
                           originIndependentBrush == null
                    //3. the border brush is semi-transtarent solid color brush AND border thickness is not uniform
                    //     (for uniform semi-transparent border Border.OnRender draws rectangle outline - so it works fine)
                        || originIndependentBrush.Color.A < 0xff && !uniformBorders
                    //4. there are rounded corners AND the border thickness is not uniform
                        || !DoubleUtil.IsZero(actualCorners.TopLeft) && !uniformBorders;
                }
            }

            if (_useComplexRenderCodePath)
            {
                _backgroundGeometryCache = GenerateBackgroundGeometry(innerRectangle, innerRadii);
                _borderGeometryCache = GenerateBorderGeometry(innerRectangle, innerRadii, outerRectangle, outerRadii);
            }
            else
            {
                _backgroundGeometryCache = null;
                _borderGeometryCache = null;
            }

            if (Child != null && EnableChildClipping && !innerRectangle.IsEmpty)
                TryPushClipGeometryOnChildElement(innerRectangle, innerRadii);

            return size;
        }

        protected virtual StreamGeometry GenerateBackgroundGeometry(Rect innerRectangle, Radii innerRadii)
        {
            if (!DoubleUtil.IsZero(innerRectangle.Width) && !DoubleUtil.IsZero(innerRectangle.Height))
            {
                var backgroundGeometry = new StreamGeometry();

                using (var context = backgroundGeometry.Open())
                    GenerateGeometry(context, innerRectangle, innerRadii);

                backgroundGeometry.Freeze();

                return backgroundGeometry;
            }
            else
            {
                return null;
            }
        }

        protected virtual StreamGeometry GenerateBorderGeometry(Rect innerRectangle, Radii innerRadii, Rect outerRectangle, Radii outerRadii)
        {
            if (!DoubleUtil.IsZero(outerRectangle.Width) && !DoubleUtil.IsZero(outerRectangle.Height))
            {
                var borderGeometry = new StreamGeometry();

                using (var context = borderGeometry.Open())
                    GenerateGeometry(context, outerRectangle, outerRadii);

                borderGeometry.Freeze();
                
                return borderGeometry;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// In addition to the child, Border renders a background + border.  The background is drawn inside the border.
        /// </summary>
        protected override void OnRender(DrawingContext context)
        {
            if (_useComplexRenderCodePath)
                UseComplexRender(context);
            else
                UseSimpleRender(context);
        }

        protected virtual void UseComplexRender(DrawingContext context)
        {
            if (_borderGeometryCache != null)
            {
                var brush = BorderBrush;

                if (brush != null)
                    context.DrawGeometry(brush, null, _borderGeometryCache);
            }

            if (_backgroundGeometryCache != null)
            {
                var brush = Background;

                if (brush != null)
                    context.DrawGeometry(brush, null, _backgroundGeometryCache);
            }
        }

        protected virtual void UseSimpleRender(DrawingContext context)
        {
            var useOuterCornerRadius = UseOuterCornerRadius;
            var actualCorners = ActualCornerRadius;

            var border = GetActualBorder();

            var outerRectangle = new Rect(RenderSize);
            var innerRectangle = DeflateRect(outerRectangle, border);

            // If we have a brush with which to draw the border, do so.
            if (!border.IsZero())
            {
                var borderBrush = BorderBrush;

                if (borderBrush != null)
                {
                    var outerRadii = CalculateCornerRadii(actualCorners, border, true, useOuterCornerRadius);

                    if (!DoubleUtil.IsZero(outerRadii.TopLeft))
                    {
                        // Already validated that all corners have the same radius
                        context.DrawRoundedRectangle(borderBrush, null, outerRectangle, outerRadii.TopLeft, outerRadii.TopLeft);
                    }
                    else
                    {
                        context.DrawRectangle(borderBrush, null, outerRectangle);
                    }
                }
            }

            // Draw background in rectangle inside border.
            // Do not draw background if the borders are so large that they overlap.
            if (!innerRectangle.IsEmpty)
            {
                var background = Background;

                if (background != null)
                {
                    var innerRadii = CalculateCornerRadii(actualCorners, border, false, useOuterCornerRadius);

                    if (!DoubleUtil.IsZero(innerRadii.TopLeft))
                    {
                        // Already validated that all corners have the same radius
                        context.DrawRoundedRectangle(background, null, innerRectangle, innerRadii.TopLeft, innerRadii.TopLeft);
                    }
                    else
                    {
                        context.DrawRectangle(background, null, innerRectangle);
                    }
                }
            }
        }

        protected Radii CalculateCornerRadii(CornerRadius radius, Thickness border, bool isOuterShape, bool useOuterCornerRadius)
        {
            var result = new Radii();

            var left = 0.0;
            var top = 0.0;
            var right = 0.0;
            var bottom = 0.0;

            if (useOuterCornerRadius)
            {
                if (!isOuterShape)
                {
                    left = border.Left;
                    top = border.Top;
                    right = border.Right;
                    bottom = border.Bottom;
                }
            }
            else
            {
                if (isOuterShape)
                {
                    left = border.Left;
                    top = border.Top;
                    right = border.Right;
                    bottom = border.Bottom;
                }
            }

            if (isOuterShape)
            {
                if (DoubleUtil.IsZero(radius.TopLeft))
                {
                    result.LeftTop = result.TopLeft = 0.0;
                }
                else
                {
                    result.LeftTop = radius.TopLeft + left;
                    result.TopLeft = radius.TopLeft + top;
                }

                if (DoubleUtil.IsZero(radius.TopRight))
                {
                    result.TopRight = result.RightTop = 0.0;
                }
                else
                {
                    result.TopRight = radius.TopRight + top;
                    result.RightTop = radius.TopRight + right;
                }

                if (DoubleUtil.IsZero(radius.BottomRight))
                {
                    result.RightBottom = result.BottomRight = 0.0;
                }
                else
                {
                    result.RightBottom = radius.BottomRight + right;
                    result.BottomRight = radius.BottomRight + bottom;
                }

                if (DoubleUtil.IsZero(radius.BottomLeft))
                {
                    result.BottomLeft = result.LeftBottom = 0.0;
                }
                else
                {
                    result.BottomLeft = radius.BottomLeft + bottom;
                    result.LeftBottom = radius.BottomLeft + left;
                }
            }
            else
            {
                result.LeftTop = Math.Max(0.0, radius.TopLeft - left);
                result.TopLeft = Math.Max(0.0, radius.TopLeft - top);
                result.TopRight = Math.Max(0.0, radius.TopRight - top);
                result.RightTop = Math.Max(0.0, radius.TopRight - right);
                result.RightBottom = Math.Max(0.0, radius.BottomRight - right);
                result.BottomRight = Math.Max(0.0, radius.BottomRight - bottom);
                result.BottomLeft = Math.Max(0.0, radius.BottomLeft - bottom);
                result.LeftBottom = Math.Max(0.0, radius.BottomLeft - left);
            }

            return result;
        }
    }
}