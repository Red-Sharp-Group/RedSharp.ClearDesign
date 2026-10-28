using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    /// <summary>
    /// Helper class to convert an ArcSegment to BezierSegment(s).
    /// </summary>
    public static class PathSegmentHelper
    {
        /// <summary>
        /// Updates the collection[index] segment with a poly-Bezier segment matching a given point list.
        /// A given point list must contain 3*N points for N Bezier segments.
        /// </summary>
        public static bool SyncPolyBezierSegment(PathSegmentCollection collection, int index, IList<Point> points, int start, int count)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (index < 0 || index >= collection.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (points == null)
                throw new ArgumentNullException(nameof(points));

            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (points.Count < start + count)
                throw new ArgumentOutOfRangeException(nameof(count));

            var result = false;

            count = count / 3 * 3;

            if (!(collection[index] is PolyBezierSegment polyBezierSegment))
            {
                collection[index] = polyBezierSegment = new PolyBezierSegment();

                result = true;
            }

            polyBezierSegment.Points.EnsureListCount(count);

            for (int i = 0; i < count; ++i)
            {
                if (polyBezierSegment.Points[i] != points[i + start])
                {
                    polyBezierSegment.Points[i] = points[i + start];

                    result = true;
                }
            }

            return result;
        }
    }
}