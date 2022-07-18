using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ColorHelper
    {
        public static Color AlphaBlending(Color background, Color foreground)
        {
            var alphaF = (float)foreground.A / byte.MaxValue;
            var alphaB = (float)background.A / byte.MaxValue;

            var resultColor = SumColor(MultiplyColor(foreground, alphaF), MultiplyColor(background, (1 - alphaF) * alphaB));

            var resultAlpha = (byte)((alphaF + (1 - alphaF) * alphaB) * byte.MaxValue);

            return Color.FromArgb(resultAlpha, resultColor.R, resultColor.G, resultColor.B);
        }

        public static Color SumColor(Color first, Color second)
        {
            var alpha = SumChannel(first.A, second.A);
            var red = SumChannel(first.R, second.R);
            var green = SumChannel(first.G, second.G);
            var blue = SumChannel(first.B, second.B);

            return Color.FromArgb(alpha, red, green, blue);
        }

        public static Color MultiplyColor(Color input, float coefficient)
        {
            var alpha = MultiplyChannel(input.A, coefficient);
            var red = MultiplyChannel(input.R, coefficient);
            var green = MultiplyChannel(input.G, coefficient);
            var blue = MultiplyChannel(input.B, coefficient);

            return Color.FromArgb(alpha, red, green, blue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte SumChannel(byte first, byte second)
        {
            return (byte)Math.Min(first + second, byte.MaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte MultiplyChannel(byte channel, float coefficient)
        {
            return (byte)Math.Ceiling(channel * coefficient);
        }
    }
}
