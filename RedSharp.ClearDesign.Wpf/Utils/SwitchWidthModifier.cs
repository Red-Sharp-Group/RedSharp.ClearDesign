using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class SwitchWidthModifier : ModifierBase<double, double>
    {
        protected override double Process(double input, double modifier)
        {
            return input * 2;
        }
    }
}
