using System.Runtime.CompilerServices;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    public static class ThicknessHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Thickness thickness)
        {
                return DoubleUtil.IsZero(thickness.Left) && 
                       DoubleUtil.IsZero(thickness.Top) && 
                       DoubleUtil.IsZero(thickness.Right) && 
                       DoubleUtil.IsZero(thickness.Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this CornerRadius radius)
        {
                return DoubleUtil.IsZero(radius.TopLeft) && 
                       DoubleUtil.IsZero(radius.TopRight) && 
                       DoubleUtil.IsZero(radius.BottomRight) && 
                       DoubleUtil.IsZero(radius.BottomLeft);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUniform(this Thickness thickness)
        {
                return DoubleUtil.AreClose(thickness.Left, thickness.Top) && 
                       DoubleUtil.AreClose(thickness.Left, thickness.Right) && 
                       DoubleUtil.AreClose(thickness.Left, thickness.Bottom);
        }

        /// <summary>
        /// Verifies if this Thickness contains only valid values.
        /// The set of validity checks is passed as parameters.
        /// </summary>
        /// <param name='allowNegative'>allows negative values</param>
        /// <param name='allowNaN'>allows Double.NaN</param>
        /// <param name='allowPositiveInfinity'>allows Double.PositiveInfinity</param>
        /// <param name='allowNegativeInfinity'>allows Double.NegativeInfinity</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this Thickness thickness, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative && IsNegative(thickness.Left, thickness.Right, thickness.Top, thickness.Bottom))
                return false;

            if (!allowNaN && IsNaN(thickness.Left, thickness.Right, thickness.Top, thickness.Bottom))
                return false;

            if (!allowPositiveInfinity && IsPositiveInfinity(thickness.Left, thickness.Right, thickness.Top, thickness.Bottom))
                return false;

            if (!allowNegativeInfinity && IsNegativeInfinity(thickness.Left, thickness.Right, thickness.Top, thickness.Bottom))
                return false;

            return true;
        }

        /// <summary>
        /// Compares two thicknesses for fuzzy equality. 
        /// This function helps compensate for the fact that double values can acquire error when operated upon.
        /// </summary>
        /// <param name='allowNegative'>allows negative values</param>
        /// <param name='allowNaN'>allows Double.NaN</param>
        /// <param name='allowPositiveInfinity'>allows Double.PositiveInfinity</param>
        /// <param name='allowNegativeInfinity'>allows Double.NegativeInfinity</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsClose(this Thickness thickness, Thickness another)
        {
            return DoubleUtil.AreClose(thickness.Left, another.Left) && 
                   DoubleUtil.AreClose(thickness.Top, another.Top) && 
                   DoubleUtil.AreClose(thickness.Right, another.Right) && 
                   DoubleUtil.AreClose(thickness.Bottom, another.Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this CornerRadius corner, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative && IsNegative(corner.TopLeft, corner.TopRight, corner.BottomLeft, corner.BottomRight))
                return false;

            if (!allowNaN && IsNaN(corner.TopLeft, corner.TopRight, corner.BottomLeft, corner.BottomRight))
                return false;

            if (!allowPositiveInfinity && IsPositiveInfinity(corner.TopLeft, corner.TopRight, corner.BottomLeft, corner.BottomRight))
                return false;

            if (!allowNegativeInfinity && IsNegativeInfinity(corner.TopLeft, corner.TopRight, corner.BottomLeft, corner.BottomRight))
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNegative(double first, double second, double third, double fourth)
        {
            return first < 0d ||
                   second < 0d ||
                   third < 0d ||
                   fourth < 0d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNaN(double first, double second, double third, double fourth)
        {
            return double.IsNaN(first) ||
                   double.IsNaN(second) ||
                   double.IsNaN(third) ||
                   double.IsNaN(fourth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPositiveInfinity(double first, double second, double third, double fourth)
        {
            return double.IsPositiveInfinity(first) ||
                   double.IsPositiveInfinity(second) ||
                   double.IsPositiveInfinity(third) ||
                   double.IsPositiveInfinity(fourth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNegativeInfinity(double first, double second, double third, double fourth)
        {
            return double.IsNegativeInfinity(first) ||
                   double.IsNegativeInfinity(second) ||
                   double.IsNegativeInfinity(third) ||
                   double.IsNegativeInfinity(fourth);
        }
    }
}