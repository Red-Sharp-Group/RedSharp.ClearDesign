using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Wpf.Helpers
{
    public static class ControlsHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TType FindParent<TType>(DependencyObject child) where TType : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;
            else if (parent is TType)
                return (TType)parent;
            else
                return FindParent<TType>(parent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRoundCornerRadiusForBorder(Border border, Size newSize, Size previousSize)
        {
            if (!TryGetNewDiameter(border, newSize, previousSize, out double newDiameter))
                return;

            var radius = Math.Floor(newDiameter / 2);

            border.CornerRadius = new CornerRadius(radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRoundCornerRadiusForBorderWithClipping(Border border, Size newSize, Size previousSize)
        {
            if (!TryGetNewDiameter(border, newSize, previousSize, out double newDiameter))
                return;

            var radius = Math.Floor(newDiameter / 2);

            border.CornerRadius = new CornerRadius(radius);

            var clipGeometry = new EllipseGeometry();

            clipGeometry.RadiusX = radius;
            clipGeometry.RadiusY = radius;
            clipGeometry.Center = new Point(radius, radius);

            border.Clip = clipGeometry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRoundRectangleClippingForBorder(Border border, Size newSize, Size previousSize)
        {
            if (!AreDifferent(newSize.Width, previousSize.Width) &&
                !AreDifferent(newSize.Height, previousSize.Height))
            {
                return;
            }

            var cornerRadius = border.CornerRadius;
            var margin = border.Margin;

            //TODO remove next two lines
            var left = 0;
            var top = 0;

            var width = newSize.Width;
            var height = newSize.Height;

            //I left this as optimization.
            if (IsCornerRadiusUniform(cornerRadius))
            {
                var radius = cornerRadius.TopLeft;

                var clipGeometry = new RectangleGeometry();

                clipGeometry.RadiusX = radius;
                clipGeometry.RadiusY = radius;
                clipGeometry.Rect = new Rect(left, top, width, height);

                border.Clip = clipGeometry;
            }
            else
            {
                var pathFigure = new PathFigure()
                {
                    StartPoint = new Point(left + cornerRadius.TopLeft, top),
                    IsClosed = true,
                    IsFilled = true
                };

                pathFigure.Segments.Add(new LineSegment()
                {
                    Point = new Point(left + width - cornerRadius.TopRight, top)
                });

                if (!AreClose(cornerRadius.TopRight, 0))
                {
                    pathFigure.Segments.Add(new ArcSegment()
                    {
                        Point = new Point(left + width, top + cornerRadius.TopRight),
                        Size = new Size(cornerRadius.TopRight, cornerRadius.TopRight),
                        RotationAngle = 45,
                        SweepDirection = SweepDirection.Clockwise
                    });
                }

                pathFigure.Segments.Add(new LineSegment()
                {
                    Point = new Point(left + width, top + height - cornerRadius.BottomRight)
                });

                if (!AreClose(cornerRadius.BottomRight, 0))
                {
                    pathFigure.Segments.Add(new ArcSegment()
                    {
                        Point = new Point(left + width - cornerRadius.BottomRight, top + height),
                        Size = new Size(cornerRadius.BottomRight, cornerRadius.BottomRight),
                        RotationAngle = 45,
                        SweepDirection = SweepDirection.Clockwise
                    });
                }

                pathFigure.Segments.Add(new LineSegment()
                {
                    Point = new Point(left + cornerRadius.BottomLeft, top + height)
                });

                if (!AreClose(cornerRadius.BottomLeft, 0))
                {
                    pathFigure.Segments.Add(new ArcSegment()
                    {
                        Point = new Point(left, top + height - cornerRadius.BottomLeft),
                        Size = new Size(cornerRadius.BottomLeft, cornerRadius.BottomLeft),
                        RotationAngle = 45,
                        SweepDirection = SweepDirection.Clockwise
                    });
                }

                pathFigure.Segments.Add(new LineSegment()
                {
                    Point = new Point(left, top + cornerRadius.TopLeft)
                });

                if (!AreClose(cornerRadius.TopLeft, 0))
                {
                    pathFigure.Segments.Add(new ArcSegment()
                    {
                        Point = new Point(left + cornerRadius.TopLeft, top),
                        Size = new Size(cornerRadius.TopLeft, cornerRadius.TopLeft),
                        RotationAngle = 45,
                        SweepDirection = SweepDirection.Clockwise
                    });
                }

                border.Clip = new PathGeometry()
                {
                    Figures =
                    {
                        pathFigure
                    }
                };
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsCornerRadiusUniform(CornerRadius radius)
        {
            return AreClose(radius.TopLeft, radius.TopRight) &&
                   AreClose(radius.TopRight, radius.BottomRight) &&
                   AreClose(radius.BottomRight, radius.BottomLeft) &&
                   AreClose(radius.BottomLeft, radius.TopLeft);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreClose(double first, double second)
        {
            return Math.Abs(first - second) < 0.0001;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetNewDiameter(Border border, Size newSize, Size previousSize, out double newDiameter)
        {
            var margin = border.Margin;
            var coersedNewSize = CoerseSize(newSize, margin);
            var coersedPreviousSize = CoerseSize(previousSize, margin);

            newDiameter = Math.Min(coersedNewSize.Width, coersedNewSize.Height);

            var previousDiameter = Math.Min(coersedPreviousSize.Width, coersedPreviousSize.Height);

            return AreDifferent(newDiameter, previousDiameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Size CoerseSize(Size size, Thickness margin)
        {
            return new Size()
            {
                Width = Math.Max(size.Width + Math.Min(0, margin.Left) + Math.Min(0, margin.Right), 0),
                Height = Math.Max(size.Height + Math.Min(0, margin.Top) + Math.Min(0, margin.Bottom), 0)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreDifferent(double first, double second)
        {
            return Math.Abs(first - second) > 0.001;
        }
    }
}
