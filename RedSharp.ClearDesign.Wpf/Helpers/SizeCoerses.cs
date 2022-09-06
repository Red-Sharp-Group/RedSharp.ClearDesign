using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Helpers
{
    public static class SizeCoerses
    {
        public static Coerce<Thickness, double> MultiplierThicknessCoerce { get; } = new Coerce<Thickness, double>(InternalMultiplierThicknessCoerce);

        public static Coerce<double, double> FocusOutstandingValueCoerce { get; } = new Coerce<double, double>(InternalFocusOutstandingValueCoerce);

        public static Coerce<double, double> RoundToggleHeightValueCoerce { get; } = new Coerce<double, double>(InternalRoundToggleHeightValueCoerce);

        public static Coerce<double, double> SwitcherToggleHeightValueCoerce { get; } = new Coerce<double, double>(InternalSwitcherToggleHeightValueCoerce);

        private static Thickness InternalMultiplierThicknessCoerce(Thickness input, double modifier)
        {
            return new Thickness
            (
                input.Left * modifier,
                input.Top * modifier,
                input.Right * modifier,
                input.Bottom * modifier
            );
        }

        private static double InternalFocusOutstandingValueCoerce(double input, double modifier)
        {
            return Math.Min(0, modifier - input);
        }

        private static double InternalRoundToggleHeightValueCoerce(double input, double modifier)
        {
            return input + 2 * modifier;
        }

        private static double InternalSwitcherToggleHeightValueCoerce(double input, double modifier)
        {
            return input * 2;
        }
    }
}
