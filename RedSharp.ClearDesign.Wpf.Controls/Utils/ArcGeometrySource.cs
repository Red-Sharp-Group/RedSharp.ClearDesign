using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Wpf.Controls.Abstracts;
using RedSharp.ClearDesign.Wpf.Controls.Enums;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;
using RedSharp.ClearDesign.Wpf.Controls.Interfaces;

namespace RedSharp.ClearDesign.Wpf.Controls.Utils
{
    public class ArcGeometrySource : GeometrySourceBase<IArcGeometrySourceParameters>
    {
        private double _relativeThickness;
        private double _absoluteThickness;

        /// <summary>
        /// Arc recognizes Stretch.None as the same as Stretch.Fill, assuming aspect ratio = 1:1.
        /// </summary>
        protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
        {
            return GeometryHelper.GetStretchBound(base.ComputeLogicalBounds(layoutBounds, parameters), parameters.Stretch, new Size(1.0, 1.0));
        }

        protected override bool UpdateCachedGeometry(IArcGeometrySourceParameters parameters)
        {
            bool flag1 = false;

            NormalizeThickness(parameters);

            bool relativeMode = parameters.ArcThicknessUnit == UnitType.Percent;
            bool flag2 = MathHelper.AreClose(parameters.StartAngle, parameters.EndAngle);

            double num = NormalizeAngle(parameters.StartAngle);
            double end = NormalizeAngle(parameters.EndAngle);

            if (end < num)
                end += 360.0;

            bool isFilled = _relativeThickness == 1.0;
            bool flag3 = _relativeThickness == 0.0;

            return !flag2 ? !MathHelper.IsVerySmall((end - num) % 360.0) ? !isFilled ? !flag3 ? flag1 | UpdateRingArcGeometry(relativeMode, num, end) : flag1 | UpdateOpenArcGeometry(num, end) : flag1 | UpdatePieGeometry(num, end) : flag3 || isFilled ? flag1 | UpdateEllipseGeometry(isFilled) : flag1 | UpdateFullRingGeometry(relativeMode) : flag1 | UpdateZeroAngleGeometry(relativeMode, num);
        }

        /// <summary>
        /// Normalize thickness, both relative to the bounding box and the absolute pixel.
        /// Relative thickness = 0 -&gt; full circle radius or clamped.
        /// Relative thickness = 1 -&gt; shrank to a dot, or degenerated.
        /// </summary>
        private void NormalizeThickness(IArcGeometrySourceParameters parameters)
        {
            double rhs = Math.Min(LogicalBounds.Width / 2.0, LogicalBounds.Height / 2.0);
            double lhs = parameters.ArcThickness;

            if (parameters.ArcThicknessUnit == UnitType.Pixel)
                lhs = MathHelper.SafeDivide(lhs, rhs, 0.0);

            _relativeThickness = MathHelper.EnsureRange(lhs, new double?(0.0), new double?(1.0));
            _absoluteThickness = rhs * _relativeThickness;
        }

        /// <summary>
        /// The arc is degenerated to a line pointing to center / normal inward.
        /// </summary>
        private bool UpdateZeroAngleGeometry(bool relativeMode, double angle)
        {
            bool flag = false;

            Point arcPoint = GeometryHelper.GetArcPoint(angle, LogicalBounds);
            Rect logicalBounds = LogicalBounds;

            double radiusX = logicalBounds.Width / 2.0;
            double radiusY = logicalBounds.Height / 2.0;

            Point point;

            if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
            {
                Rect bound = LogicalBounds.Resize(1.0 - _relativeThickness);
                point = GeometryHelper.GetArcPoint(angle, bound);
            }
            else
            {
                double intersect = InnerCurveSelfIntersect(radiusX, radiusY, _absoluteThickness);
                double[] angleRanges = ComputeAngleRanges(radiusX, radiusY, intersect, angle, angle);
                double num = angleRanges[0] * Math.PI / 180.0;

                Vector vector = new Vector(radiusY * Math.Sin(num), -radiusX * Math.Cos(num));

                point = GeometryHelper.GetArcPoint(angleRanges[0], LogicalBounds) - vector.Normalized() * _absoluteThickness;
            }

            LineGeometry result;

            return flag | GeometryHelper.EnsureGeometryType(out result, ref _cachedGeometry, () => new LineGeometry()) | result.TrySet(LineGeometry.StartPointProperty, arcPoint) | result.TrySet(LineGeometry.EndPointProperty, point);
        }

        private bool UpdateEllipseGeometry(bool isFilled)
        {
            bool flag1 = false;

            double y = MathHelper.Lerp(LogicalBounds.Top, LogicalBounds.Bottom, 0.5);

            Point point1 = new Point(LogicalBounds.Left, y);
            Point point2 = new Point(LogicalBounds.Right, y);

            PathGeometry result1;

            bool flag2 = flag1 | GeometryHelper.EnsureGeometryType(out result1, ref _cachedGeometry, () => new PathGeometry()) | result1.Figures.EnsureListCount(1, () => new PathFigure());

            PathFigure figure = result1.Figures[0];

            ArcSegment result2;
            ArcSegment result3;

            bool flag3 = flag2 | figure.TrySet(PathFigure.IsClosedProperty, true) | figure.TrySet(PathFigure.IsFilledProperty, isFilled) | figure.Segments.EnsureListCount(2, () => new ArcSegment()) | figure.TrySet(PathFigure.StartPointProperty, point1) | GeometryHelper.EnsureSegmentType(out result2, figure.Segments, 0, () => new ArcSegment()) | GeometryHelper.EnsureSegmentType(out result3, figure.Segments, 1, () => new ArcSegment());

            Size size = new Size(LogicalBounds.Width / 2.0, LogicalBounds.Height / 2.0);

            return flag3 | result2.TrySet(ArcSegment.IsLargeArcProperty, false) | result2.TrySet(ArcSegment.SizeProperty, size) | result2.TrySet(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise) | result2.TrySet(ArcSegment.PointProperty, point2) | result3.TrySet(ArcSegment.IsLargeArcProperty, false) | result3.TrySet(ArcSegment.SizeProperty, size) | result3.TrySet(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise) | result3.TrySet(ArcSegment.PointProperty, point1);
        }

        private bool UpdateFullRingGeometry(bool relativeMode)
        {
            PathGeometry result;

            bool flag1 = false | GeometryHelper.EnsureGeometryType(out result, ref _cachedGeometry, () => new PathGeometry()) | result.TrySet(PathGeometry.FillRuleProperty, FillRule.EvenOdd) | result.Figures.EnsureListCount(2, () => new PathFigure()) | PathFigureHelper.SyncEllipseFigure(result.Figures[0], LogicalBounds, SweepDirection.Clockwise);

            Rect logicalBounds = LogicalBounds;

            double radiusX = logicalBounds.Width / 2.0;
            double radiusY = logicalBounds.Height / 2.0;

            bool flag2;

            if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
            {
                Rect bounds = LogicalBounds.Resize(1.0 - _relativeThickness);

                flag2 = flag1 | PathFigureHelper.SyncEllipseFigure(result.Figures[1], bounds, SweepDirection.Counterclockwise);
            }
            else
            {
                bool flag3 = flag1 | result.Figures[1].TrySet(PathFigure.IsClosedProperty, true) | result.Figures[1].TrySet(PathFigure.IsFilledProperty, true);

                Point firstPoint = new Point();

                double intersect = InnerCurveSelfIntersect(radiusX, radiusY, _absoluteThickness);
                double[] angleRanges = ComputeAngleRanges(radiusX, radiusY, intersect, 360.0, 0.0);

                flag2 = flag3 | SyncPieceWiseInnerCurves(result.Figures[1], 0, ref firstPoint, angleRanges) | result.Figures[1].TrySet(PathFigure.StartPointProperty, firstPoint);
            }

            return flag2;
        }

        private static void IncreaseDuplicatedIndex(IList<double> values, ref int index)
        {
            while (index < values.Count - 1 && values[index] == values[index + 1])
                ++index;
        }

        private static void DecreaseDuplicatedIndex(IList<double> values, ref int index)
        {
            while (index > 0 && values[index] == values[index - 1])
                --index;
        }

        /// <summary>
        /// Compute a list of angle pairs, defining the ranges in which arc sample should locate.
        /// The return value have 2, 4, or 6 double values, each pair defines a range and they are in the order
        /// to span the angles from given start to end angles.  The ranges will break at the self-intersect angle.
        /// If input start/end are within the invalid range between self intersect angle, it will be moved to neighboring self intersect.
        /// </summary>
        internal static double[] ComputeAngleRanges(double radiusX, double radiusY, double intersect, double start, double end)
        {
            List<double> values = new List<double>()
            {
              start,
              end,
              intersect,
              180.0 - intersect,
              180.0 + intersect,
              360.0 - intersect,
              360.0 + intersect,
              540.0 - intersect,
              540.0 + intersect,
              720.0 - intersect
            };

            values.Sort();

            int index1 = values.IndexOf(start);
            int index2 = values.IndexOf(end);

            if (index2 == index1)
                ++index2;

            else if (start < end)
            {
                IncreaseDuplicatedIndex(values, ref index1);
                DecreaseDuplicatedIndex(values, ref index2);
            }
            else if (start > end)
            {
                DecreaseDuplicatedIndex(values, ref index1);
                IncreaseDuplicatedIndex(values, ref index2);
            }

            List<double> list = new List<double>();

            if (index1 < index2)
            {
                for (int index3 = index1; index3 <= index2; ++index3)
                    list.Add(values[index3]);
            }
            else
            {
                for (int index4 = index1; index4 >= index2; --index4)
                    list.Add(values[index4]);
            }

            double num = EnsureFirstQuadrant((list[0] + list[1]) / 2.0);

            if (radiusX < radiusY && num < intersect || radiusX > radiusY && num > intersect)
                list.RemoveAt(0);

            if (list.Count % 2 == 1)
                list.RemoveLast();

            if (list.Count == 0)
            {
                int index5 = Math.Min(index1, index2) - 1;

                if (index5 < 0)
                    index5 = Math.Max(index1, index2) + 1;

                list.Add(values[index5]);
                list.Add(values[index5]);
            }

            return list.ToArray();
        }

        /// <summary>Move angle to 0-90 range.</summary>
        internal static double EnsureFirstQuadrant(double angle)
        {
            angle = Math.Abs(angle % 180.0);

            return angle <= 90.0 ? angle : 180.0 - angle;
        }

        private bool UpdatePieGeometry(double start, double end)
        {
            bool flag = false;

            PathFigure dependencyObject1;

            if (!(_cachedGeometry is PathGeometry cachedGeometry) || cachedGeometry.Figures.Count != 1 || (dependencyObject1 = cachedGeometry.Figures[0]).Segments.Count != 2 || !(dependencyObject1.Segments[0] is ArcSegment dependencyObject2) || !(dependencyObject1.Segments[1] is LineSegment dependencyObject3))
            {
                ((PathGeometry)(_cachedGeometry = new PathGeometry())).Figures.Add(dependencyObject1 = new PathFigure());

                dependencyObject1.Segments.Add(dependencyObject2 = new ArcSegment());
                dependencyObject1.Segments.Add(dependencyObject3 = new LineSegment());
                dependencyObject1.IsClosed = true;

                dependencyObject2.SweepDirection = SweepDirection.Clockwise;

                flag = true;
            }

            return flag | dependencyObject1.TrySet(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, LogicalBounds)) | dependencyObject2.TrySet(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, LogicalBounds)) | dependencyObject2.TrySet(ArcSegment.SizeProperty, GetArcSize(LogicalBounds)) | dependencyObject2.TrySet(ArcSegment.IsLargeArcProperty, end - start > 180.0) | dependencyObject3.TrySet(LineSegment.PointProperty, LogicalBounds.GetCenter());
        }

        private bool UpdateOpenArcGeometry(double start, double end)
        {
            bool flag = false;
            PathFigure dependencyObject1;

            if (!(_cachedGeometry is PathGeometry cachedGeometry) || cachedGeometry.Figures.Count != 1 || (dependencyObject1 = cachedGeometry.Figures[0]).Segments.Count != 1 || !(dependencyObject1.Segments[0] is ArcSegment dependencyObject2))
            {
                ((PathGeometry)(_cachedGeometry = new PathGeometry())).Figures.Add(dependencyObject1 = new PathFigure());

                dependencyObject1.Segments.Add(dependencyObject2 = new ArcSegment());
                dependencyObject1.IsClosed = false;

                dependencyObject2.SweepDirection = SweepDirection.Clockwise;

                flag = true;
            }

            return flag | dependencyObject1.TrySet(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, LogicalBounds)) | dependencyObject1.TrySet(PathFigure.IsFilledProperty, false) | dependencyObject2.TrySet(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, LogicalBounds)) | dependencyObject2.TrySet(ArcSegment.SizeProperty, GetArcSize(LogicalBounds)) | dependencyObject2.TrySet(ArcSegment.IsLargeArcProperty, end - start > 180.0);
        }

        private bool UpdateRingArcGeometry(bool relativeMode, double start, double end)
        {
            PathGeometry result1;

            bool flag1 = false | GeometryHelper.EnsureGeometryType(out result1, ref _cachedGeometry, () => new PathGeometry()) | result1.TrySet(PathGeometry.FillRuleProperty, FillRule.Nonzero) | result1.Figures.EnsureListCount(1, () => new PathFigure());

            PathFigure figure = result1.Figures[0];

            ArcSegment result2;
            LineSegment result3;

            bool flag2 = flag1 | figure.TrySet(PathFigure.IsClosedProperty, true) | figure.TrySet(PathFigure.IsFilledProperty, true) | figure.TrySet(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, LogicalBounds)) | figure.Segments.EnsureListCountAtLeast(3, () => new ArcSegment()) | GeometryHelper.EnsureSegmentType(out result2, figure.Segments, 0, () => new ArcSegment()) | result2.TrySet(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, LogicalBounds)) | result2.TrySet(ArcSegment.SizeProperty, new Size(LogicalBounds.Width / 2.0, LogicalBounds.Height / 2.0)) | result2.TrySet(ArcSegment.IsLargeArcProperty, end - start > 180.0) | result2.TrySet(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise) | GeometryHelper.EnsureSegmentType(out result3, figure.Segments, 1, () => new LineSegment());

            Rect logicalBounds = LogicalBounds;

            double radiusX = logicalBounds.Width / 2.0;
            double radiusY = logicalBounds.Height / 2.0;

            bool flag3;

            if (relativeMode || MathHelper.AreClose(radiusX, radiusY))
            {
                Rect bound = LogicalBounds.Resize(1.0 - _relativeThickness);

                ArcSegment result4;

                flag3 = flag2 | result3.TrySet(LineSegment.PointProperty, GeometryHelper.GetArcPoint(end, bound)) | figure.Segments.EnsureListCount(3, () => new ArcSegment()) | GeometryHelper.EnsureSegmentType(out result4, figure.Segments, 2, () => new ArcSegment()) | result4.TrySet(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(start, bound)) | result4.TrySet(ArcSegment.SizeProperty, GetArcSize(bound)) | result4.TrySet(ArcSegment.IsLargeArcProperty, end - start > 180.0) | result4.TrySet(ArcSegment.SweepDirectionProperty, SweepDirection.Counterclockwise);
            }
            else
            {
                Point firstPoint = new Point();

                double intersect = InnerCurveSelfIntersect(radiusX, radiusY, _absoluteThickness);
                double[] angleRanges = ComputeAngleRanges(radiusX, radiusY, intersect, end, start);

                flag3 = flag2 | SyncPieceWiseInnerCurves(figure, 2, ref firstPoint, angleRanges) | result3.TrySet(LineSegment.PointProperty, firstPoint);
            }

            return flag3;
        }

        /// <summary>
        /// Compute all pieces of inner curves with each pair of input angles, and connect them with poly Bezier segments.
        /// The new segments are output to given figure.Segments list from the given index.  The start point is output seperately.
        /// </summary>
        private bool SyncPieceWiseInnerCurves(PathFigure figure, int index, ref Point firstPoint, params double[] angles)
        {
            bool flag1 = false;
            int length = angles.Length;
            Rect logicalBounds = LogicalBounds;
            double absoluteThickness = _absoluteThickness;
            bool flag2 = flag1 | figure.Segments.EnsureListCount(index + length / 2, () => new PolyBezierSegment());

            for (int index1 = 0; index1 < length / 2; ++index1)
            {
                IList<Point> oneInnerCurve = ComputeOneInnerCurve(angles[index1 * 2], angles[index1 * 2 + 1], logicalBounds, absoluteThickness);

                if (index1 == 0)
                    firstPoint = oneInnerCurve[0];

                flag2 |= PathSegmentHelper.SyncPolyBezierSegment(figure.Segments, index + index1, oneInnerCurve, 1, oneInnerCurve.Count - 1);
            }

            return flag2;
        }

        /// <summary>
        /// Compute one piece of inner curve with given angle range, and output one piece of smooth curve in format of poly Beizer semgents.
        /// </summary>
        private static IList<Point> ComputeOneInnerCurve(double start, double end, Rect bounds, double offset)
        {
            double num1 = bounds.Width / 2.0;
            double num2 = bounds.Height / 2.0;

            Point point1 = bounds.GetCenter();

            start = start * Math.PI / 180.0;
            end = end * Math.PI / 180.0;

            double num3 = Math.PI / 18.0;
            int capacity = Math.Max(2, (int)Math.Ceiling(Math.Abs(end - start) / num3));

            List<Point> pointList = new List<Point>(capacity);
            List<Vector> vectorList = new List<Vector>(capacity);

            Point point2 = new Point();
            Point point3 = new Point();

            Vector vector1 = new Vector();
            Vector vector2 = new Vector();
            Vector vector3 = new Vector();
            Vector vector4 = new Vector();

            for (int index = 0; index < capacity; ++index)
            {
                double num4 = MathHelper.Lerp(start, end, index / (double)(capacity - 1));
                double num5 = Math.Sin(num4);
                double num6 = Math.Cos(num4);

                point2.X = point1.X + num1 * num5;
                point2.Y = point1.Y - num2 * num6;

                vector1.X = num1 * num6;
                vector1.Y = num2 * num5;
                vector2.X = -num2 * num5;
                vector2.Y = num1 * num6;

                double d = num2 * num2 * num5 * num5 + num1 * num1 * num6 * num6;
                double num7 = Math.Sqrt(d);
                double num8 = 2.0 * num5 * num6 * (num2 * num2 - num1 * num1);

                vector3.X = -num2 * num6;
                vector3.Y = -num1 * num5;

                point3.X = point2.X + offset * vector2.X / num7;
                point3.Y = point2.Y + offset * vector2.Y / num7;

                vector4.X = vector1.X + offset / num7 * (vector3.X - 0.5 * vector2.X / d * num8);
                vector4.Y = vector1.Y + offset / num7 * (vector3.Y - 0.5 * vector2.Y / d * num8);

                pointList.Add(point3);

                vectorList.Add(-vector4.Normalized());
            }

            List<Point> oneInnerCurve = new List<Point>(capacity * 3 + 1);

            oneInnerCurve.Add(pointList[0]);

            for (int index = 1; index < capacity; ++index)
            {
                Point lhs = pointList[index - 1];
                Point rhs = pointList[index];

                double num9 = GeometryHelper.Distance(lhs, rhs) / 3.0;

                oneInnerCurve.Add(lhs + vectorList[index - 1] * num9);
                oneInnerCurve.Add(rhs - vectorList[index] * num9);
                oneInnerCurve.Add(rhs);
            }

            return oneInnerCurve;
        }

        /// <summary>
        /// Compute the parameter (angle) of the self-intersect point for given ellipse with given thickness.
        /// The result is always in first quadrant, and might be 0 or 90 indicating no self-intersect.
        /// Basic algorithm is to binary search for the angle that sample point is not in first quadrant.
        /// </summary>
        internal static double InnerCurveSelfIntersect(double radiusX, double radiusY, double thickness)
        {
            double angleA1 = 0.0;
            double angleB = Math.PI / 2.0;

            bool flag = radiusX <= radiusY;

            Vector vector = new Vector();

            while (!AreCloseEnough(angleA1, angleB))
            {
                double num1 = (angleA1 + angleB) / 2.0;
                double num2 = Math.Cos(num1);
                double num3 = Math.Sin(num1);

                vector.X = radiusY * num3;
                vector.Y = radiusX * num2;

                vector.Normalize();

                if (flag)
                {
                    double num4 = radiusX * num3 - vector.X * thickness;

                    if (num4 > 0.0)
                        angleB = num1;
                    else if (num4 < 0.0)
                        angleA1 = num1;
                }
                else
                {
                    double num5 = radiusY * num2 - vector.Y * thickness;

                    if (num5 < 0.0)
                        angleB = num1;
                    else if (num5 > 0.0)
                        angleA1 = num1;
                }
            }

            double angleA2 = (angleA1 + angleB) / 2.0;

            if (AreCloseEnough(angleA2, 0.0))
                return 0.0;

            return !AreCloseEnough(angleA2, Math.PI / 2.0) ? angleA2 * 180.0 / Math.PI : 90.0;
        }

        private static bool AreCloseEnough(double angleA, double angleB)
        {
            return Math.Abs(angleA - angleB) < 0.001;
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        private static Size GetArcSize(Rect bound)
        {
            return new Size(bound.Width / 2.0, bound.Height / 2.0);
        }

        private static double NormalizeAngle(double degree)
        {
            if (degree < 0.0 || degree > 360.0)
            {
                degree %= 360.0;

                if (degree < 0.0)
                    degree += 360.0;
            }

            return degree;
        }
    }
}
