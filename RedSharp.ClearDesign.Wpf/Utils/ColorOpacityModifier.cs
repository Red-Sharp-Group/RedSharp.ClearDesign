using System;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class ColorOpacityModifier : ModifierBase<Color, double>
    {
        protected override Color Process(Color input, double modifier)
        {
            var alpha = (byte)Math.Round(byte.MaxValue * modifier);

            return Color.FromArgb(alpha, input.R, input.G, input.B);
        }
    }
}
