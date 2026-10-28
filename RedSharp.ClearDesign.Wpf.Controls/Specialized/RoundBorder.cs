using System.Windows.Media;
using System.Windows;
using System;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;

namespace RedSharp.ClearDesign.Wpf.Controls.Specialized
{
    public class RoundBorder : LayeredBorder
    {

        public static readonly DependencyProperty DrawBackgroundUnderBorderProperty;

        static RoundBorder()
        {
            var affectMeasureAndRender = FrameworkPropertyMetadataOptions.AffectsMeasure |
                                         FrameworkPropertyMetadataOptions.AffectsRender;

            DrawBackgroundUnderBorderProperty = DependencyProperty.Register(nameof(DrawBackgroundUnderBorder), typeof(bool), typeof(RoundBorder),
                new FrameworkPropertyMetadata(false, affectMeasureAndRender));
        }

        public bool DrawBackgroundUnderBorder
        {
            get => (bool)GetValue(DrawBackgroundUnderBorderProperty);
            set => SetValue(DrawBackgroundUnderBorderProperty, value);
        }

        protected override StreamGeometry GenerateBorderGeometry(Rect innerRectangle, Radii innerRadii, Rect outerRectangle, Radii outerRadii)
        {
            if (!DoubleUtil.IsZero(outerRectangle.Width) && !DoubleUtil.IsZero(outerRectangle.Height))
            {
                var borderGeometry = new StreamGeometry();

                using (var context = borderGeometry.Open())
                {
                    GenerateGeometry(context, outerRectangle, outerRadii);

                    if (!DoubleUtil.IsZero(innerRectangle.Width) && !DoubleUtil.IsZero(innerRectangle.Height))
                        GenerateGeometry(context, innerRectangle, innerRadii);
                }

                borderGeometry.Freeze();

                return borderGeometry;
            }
            else
            {
                return null;
            }
        }

        protected override void UseSimpleRender(DrawingContext context)
        {
            var drawBackgroundUnderBorder = DrawBackgroundUnderBorder;
            var useOuterCornerRadius = UseOuterCornerRadius;
            var actualCorners = ActualCornerRadius;

            var border = GetActualBorder();

            var outerRectangle = new Rect(RenderSize);
            var innerRectangle = default(Rect);

            //I have no idea how I managed this to work,
            //so I assume you don't want to touch it until I do my mind
            if (drawBackgroundUnderBorder)
                innerRectangle = DeflateRect(outerRectangle, new Thickness(0));
            else
                innerRectangle = DeflateRect(outerRectangle, border);

            // Draw background in rectangle inside border.
            // Do not draw background if the borders are so large that they overlap.
            if (!innerRectangle.IsEmpty)
            {
                var background = Background;

                if (background != null)
                {
                    var innerRadii = CalculateCornerRadii(actualCorners, border, drawBackgroundUnderBorder, useOuterCornerRadius);

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

            // If we have a brush with which to draw the border, do so.
            if (!border.IsZero())
            {
                var borderBrush = BorderBrush;

                if (borderBrush != null)
                {
                    if (border.IsUniform())
                    {
                        var halfThickness = border.Left * 0.5;

                        // Create rect w/ border thickness, and round if applying layout rounding.
                        Rect rect = new Rect(new Point(halfThickness, halfThickness),
                                             new Point(RenderSize.Width - halfThickness, RenderSize.Height - halfThickness));

                        Pen pen = new Pen()
                        {
                            Brush = borderBrush,
                            Thickness = border.Left
                        };

                        var outerRadii = CalculateCornerRadiiForPen(actualCorners, border, true, useOuterCornerRadius);

                        if (!DoubleUtil.IsZero(outerRadii.TopLeft))
                        {
                            // Already validated that all corners have the same radius
                            context.DrawRoundedRectangle(null, pen, rect, outerRadii.TopLeft, outerRadii.TopLeft);
                        }
                        else
                        {
                            context.DrawRectangle(null, pen, rect);
                        }
                    }
                    else
                    {
                        if (DoubleUtil.GreaterThan(border.Left, 0))
                        {
                            var rect = new Rect(new Point(),
                                                new Point(border.Left, RenderSize.Height));

                            context.DrawRectangle(borderBrush, null, rect);
                        }

                        if (DoubleUtil.GreaterThan(border.Top, 0))
                        {
                            var rect = new Rect(new Point(0.0, 0.0),
                                                new Point(RenderSize.Width, border.Top));

                            context.DrawRectangle(borderBrush, null, rect);
                        }

                        if (DoubleUtil.GreaterThan(border.Right, 0))
                        {
                            var rect = new Rect(new Point(RenderSize.Width - border.Right, 0.0),
                                                new Point(RenderSize.Width, RenderSize.Height));

                            context.DrawRectangle(borderBrush, null, rect);
                        }

                        if (DoubleUtil.GreaterThan(border.Bottom, 0))
                        {
                            var rect = new Rect(new Point(0.0, RenderSize.Height - border.Bottom),
                                                new Point(RenderSize.Width, RenderSize.Height));

                            context.DrawRectangle(borderBrush, null, rect);
                        }
                    }
                }
            }
        }

        protected Radii CalculateCornerRadiiForPen(CornerRadius radius, Thickness border, bool isOuterShape, bool useOuterCornerRadius)
        {
            var result = new Radii();

            var left = 0.0;
            var top = 0.0;
            var right = 0.0;
            var bottom = 0.0;

            if (useOuterCornerRadius)
            {
                if (isOuterShape)
                {
                    left = -0.5 * border.Left;
                    top = -0.5 * border.Top;
                    right = -0.5 * border.Right;
                    bottom = -0.5 * border.Bottom;
                }
                else
                {
                    left = 0.5 * border.Left;
                    top = 0.5 * border.Top;
                    right = 0.5 * border.Right;
                    bottom = 0.5 * border.Bottom;
                }
            }
            else
            {
                if (isOuterShape)
                {
                    left = 0.5 * border.Left;
                    top = 0.5 * border.Top;
                    right = 0.5 * border.Right;
                    bottom = 0.5 * border.Bottom;
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