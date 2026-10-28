using System.Windows;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    /// <summary>
    /// Helper class to work with PathFigure.
    /// </summary>
    public static class PathFigureHelper
    {
        /// <summary>
        /// Synchronizes the given figure to be a closed ellipse with two arc segments.
        /// </summary>
        public static bool SyncEllipseFigure(PathFigure figure, Rect bounds, SweepDirection sweepDirection, bool isFilled = true)
        {
            var flag = false;
            var pointArray = new Point[2];
            var size = new Size(bounds.Width / 2.0, bounds.Height / 2.0);
            var point = bounds.GetCenter();

            if (size.Width > size.Height)
            {
                pointArray[0] = new Point(bounds.Left, point.Y);
                pointArray[1] = new Point(bounds.Right, point.Y);
            }
            else
            {
                pointArray[0] = new Point(point.X, bounds.Top);
                pointArray[1] = new Point(point.X, bounds.Bottom);
            }

            ArcSegment result;

            flag |= figure.TrySet(PathFigure.IsClosedProperty, true);
            flag |= figure.TrySet(PathFigure.IsFilledProperty, isFilled);
            flag |= figure.TrySet(PathFigure.StartPointProperty, pointArray[0]);

            flag |= figure.Segments.EnsureListCount(2, () => new ArcSegment());

            flag |= GeometryHelper.EnsureSegmentType(out result, figure.Segments, 0, () => new ArcSegment());

            flag |= result.TrySet(ArcSegment.PointProperty, pointArray[1]);
            flag |= result.TrySet(ArcSegment.SizeProperty, size);
            flag |= result.TrySet(ArcSegment.IsLargeArcProperty, false);
            flag |= result.TrySet(ArcSegment.SweepDirectionProperty, sweepDirection);

            flag |= GeometryHelper.EnsureSegmentType(out result, figure.Segments, 1, () => new ArcSegment());

            flag |= result.TrySet(ArcSegment.PointProperty, pointArray[0]);
            flag |= result.TrySet(ArcSegment.SizeProperty, size);
            flag |= result.TrySet(ArcSegment.IsLargeArcProperty, false);
            flag |= result.TrySet(ArcSegment.SweepDirectionProperty, sweepDirection);

            return flag;
        }
    }
}