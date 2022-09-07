using System.Windows;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class MultiplierThicknessModifier : ModifierBase<Thickness, double>
    {
        protected override Thickness Process(Thickness input, double modifier)
        {
            return new Thickness
            (
                input.Left * modifier,
                input.Top * modifier,
                input.Right * modifier,
                input.Bottom * modifier
            );
        }
    }
}
