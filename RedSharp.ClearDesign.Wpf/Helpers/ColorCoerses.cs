using System;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ColorCoerses
    {
        public static Coerce<Color, Color> AlphaBlendingCoerce { get; } = new Coerce<Color, Color>(ColorHelper.AlphaBlending);

        public static Coerce<Color, double> ColorOpacityCoerce { get; } = new Coerce<Color, double>(InternalColorOpacityCoerce);

        private static Color InternalColorOpacityCoerce(Color input, double modifier)
        {
            var alpha = (byte)Math.Round(byte.MaxValue * modifier);

            return Color.FromArgb(alpha, input.R, input.G, input.B);
        }
    }
}
