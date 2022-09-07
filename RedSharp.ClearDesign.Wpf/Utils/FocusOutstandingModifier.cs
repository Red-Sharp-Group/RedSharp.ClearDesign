using System;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class FocusOutstandingModifier : ModifierBase<double, double>
    {
        protected override double Process(double input, double modifier)
        {
            return Math.Min(0, modifier - input);
        }
    }
}
