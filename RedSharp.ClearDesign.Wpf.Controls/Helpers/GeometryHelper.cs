using System;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    /// <summary>
    /// Extension methods for geometry-related data structures (Point/Vector/Size/Rect).
    /// </summary>
    public static class GeometryHelper
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetBounds(this Size size)
        {
            return new Rect(0.0, 0.0, size.Width, size.Height);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasValidArea(this Size size)
        {
            return size.Width > 0.0 && size.Height > 0.0 &&
                   !double.IsInfinity(size.Width) &&
                   !double.IsInfinity(size.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetCenter(this Rect rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2.0, rectangle.Y + rectangle.Height / 2.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Thickness Subtract(this Rect left, Rect right)
        {
            return new Thickness(right.Left - left.Left, right.Top - left.Top, left.Right - right.Right, left.Bottom - right.Bottom);
        }

        /// <summary>
        /// Resizes the rectangle to a relative size while keeping the center invariant.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect Resize(this Rect rectangle, double ratio)
        {
            return rectangle.Resize(ratio, ratio);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect Resize(this Rect rectangle, double ratioX, double ratioY)
        {
            var point = rectangle.GetCenter();

            var width = rectangle.Width * ratioX;
            var height = rectangle.Height * ratioY;

            return new Rect(point.X - width / 2.0, point.Y - height / 2.0, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector Normalized(this Vector vector)
        {
            var length = vector.Length;

            return MathHelper.IsVerySmall(length) ? new Vector(0.0, 1.0) : vector / length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect Inflate(Rect rectangle, double offset)
        {
            return Inflate(rectangle, new Thickness(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect Inflate(Rect rect, Thickness thickness)
        {
            var width = rect.Width + thickness.Left + thickness.Right;
            var height = rect.Height + thickness.Top + thickness.Bottom;

            var x = rect.X - thickness.Left;

            if (width < 0.0)
            {
                x += width / 2.0;

                width = 0.0;
            }

            double y = rect.Y - thickness.Top;

            if (height < 0.0)
            {
                y += height / 2.0;

                height = 0.0;
            }

            return new Rect(x, y, width, height);
        }

        /// <summary>
        /// Gets the normalized arc in a (0,0)(1,1) box.
        /// Zero degrees is mapped to [0.5, 0] (up), and clockwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetArcPoint(double degree)
        {
            var temp = degree * Math.PI / 180.0;

            return new Point(0.5 + 0.5 * Math.Sin(temp), 0.5 - 0.5 * Math.Cos(temp));
        }

        /// <summary>
        /// Gets the absolute arc point in a given bound with a given relative radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetArcPoint(double degree, Rect bound)
        {
            var arcPoint = GetArcPoint(degree);

            return RelativeToAbsolutePoint(bound, arcPoint);
        }


        /// <summary>
        /// Maps a relative point to an absolute point using the mapping from a given bound to a (0,0)(1,1) box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point RelativeToAbsolutePoint(Rect bound, Point relative)
        {
            return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
        }

        /// <summary>
        /// Computes the bound after stretching within a given logical bound.
        /// If stretch to uniform, use given aspectRatio.
        /// If aspectRatio is empty, it's equivalent to Fill.
        /// If stretch is None, it's equivalent to Fill or Uniform.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetStretchBound(Rect logicalBound, Stretch stretch, Size aspectRatio)
        {
            if (stretch == Stretch.None)
                stretch = Stretch.Fill;

            if (stretch == Stretch.Fill || !aspectRatio.HasValidArea())
                return logicalBound;

            var point = logicalBound.GetCenter();

            switch (stretch)
            {
                case Stretch.Uniform:
                    if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                    {
                        logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
                        break;
                    }

                    logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;

                    break;
                case Stretch.UniformToFill:
                    if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                    {
                        logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
                        break;
                    }

                    logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;

                    break;
            }
            return new Rect(point.X - logicalBound.Width / 2.0, point.Y - logicalBound.Height / 2.0, logicalBound.Width, logicalBound.Height);
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The distance between <paramref name="left" /> and <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Point left, Point right)
        {
            var xDifference = left.X - right.X;
            var yDifference = left.Y - right.Y;

            return Math.Sqrt(xDifference * xDifference + yDifference * yDifference);
        }

        /// <summary>
        /// Ensures the value is an instance of result type (T). 
        /// If not, replace with a new instance of type (T).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EnsureGeometryType<TResult>(out TResult result, ref Geometry value, Func<TResult> factory) where TResult : Geometry
        {
            result = value as TResult;

            if (result != null)
                return false;

            result = factory();
            value = result;

            return true;
        }

        /// <summary>
        /// Ensures the list[index] is an instance of result type (T). If not, replace with a new instance of type (T).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EnsureSegmentType<TResult>(out TResult result, IList<PathSegment> list, int index, Func<TResult> factory) where TResult : PathSegment
        {
            result = list[index] as TResult;

            if (result != null)
                return false;

            result = factory();

            list[index] = result;

            return true;
        }
    }
}