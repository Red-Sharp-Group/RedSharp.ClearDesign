using System;
using System.Runtime.CompilerServices;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    /// <summary>
    /// Helper class that provides static properties and methods related to floating point arithmetic.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// The minimum distance to consider that two values are same.
        /// Note: internal floating point in MIL/SL is float, not double.
        /// </summary>
        public const double Epsilon = 1E-06;

        /// <summary>The value of the angle of a full circle.</summary>
        public const double TwoPI = Math.PI * 2.0;

        /// <summary>
        /// The inner radius for a pentagram polygon shape, at precision of three digits in percentage.
        /// (1 - Sin36 * Sin72 / Sin54) / (Cos36) ^ 2, which is 0.47210998990512996761913067272407
        /// </summary>
        public const double PentagramInnerRadius = 0.47211;

        /// <summary>
        /// Determines whether a <c>System.Double</c> value is small enough to be considered
        /// equivalent to zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVerySmall(double value)
        {
            return Math.Abs(value) < 1E-06;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreClose(double left, double right)
        {
            return left == right || IsVerySmall(left - right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SafeDivide(double left, double right, double alternative)
        {
            if (IsVerySmall(right))
                return left / right;
            else
                return alternative;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double x, double y, double alpha)
        {
            return x * (1.0 - alpha) + y * alpha;
        }

        /// <summary>
        /// Returns the value that's within the given range.
        /// A given min/max that is null equals no limit.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double EnsureRange(double value, double? min, double? max)
        {
            if (min.HasValue && value < min.Value)
                return min.Value;

            return max.HasValue && value > max.Value ? max.Value : value;
        }
    }
}