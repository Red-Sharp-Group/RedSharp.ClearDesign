using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ControlsHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetCornerRadiusForBorder(Border border, Size newSize, Size previousSize)
        {
            var margin = border.Margin;
            var coersedNewSize = CoerseSize(newSize, margin);
            var coersedPreviousSize = CoerseSize(previousSize, margin);

            var newDiameter = Math.Min(coersedNewSize.Width, coersedNewSize.Height);
            var previousDiameter = Math.Min(coersedPreviousSize.Width, coersedPreviousSize.Height);

            if (Math.Abs(newDiameter - previousDiameter) < 0.01)
                return;

            var radius = Math.Floor(newDiameter / 2);

            border.CornerRadius = new CornerRadius(radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Size CoerseSize(Size size, Thickness margin)
        {
            return new Size()
            {
                Width = Math.Max(size.Width + margin.Left + margin.Right, 0),
                Height = Math.Max(size.Height + margin.Top + margin.Bottom, 0)
            };
        }
    }
}
