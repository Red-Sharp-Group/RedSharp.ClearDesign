using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Utils
{
    public class ColorBlendingModifier : ModifierBase<Color, Color>
    {
        protected override Color Process(Color input, Color modifier)
        {
            return ColorHelper.AlphaBlending(input, modifier);
        }
    }
}
