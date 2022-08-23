using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Definitions
{
    public partial class Sizes : ResourceDictionary
    {
        public Sizes()
        {
            Add("S_MultiplierThicknessCoerceCallback_", new Coerce<Thickness, double>(MultiplierThicknessCoerce));
            Add("S_OutstandingFocusValueCoerceCallback_", new Coerce<double, double>(FocusOutstandingValueCoerce));
            Add("S_RoundToggleHeightValueCoerceCallback_", new Coerce<double, double>(RoundToggleHeightValueCoerce));
            Add("S_SwitchToggleWidthValueCoerceCallback_", new Coerce<double, double>(SwitcherToggleHeightValueCoerce));
        }

        public Thickness MultiplierThicknessCoerce(Thickness input, double modifier)
        {
            return new Thickness
            (
                input.Left * modifier,
                input.Top * modifier,
                input.Right * modifier,
                input.Bottom * modifier
            );
        }

        public double FocusOutstandingValueCoerce(double input, double modifier)
        {
            return Math.Min(0, modifier - input);
        }

        public double RoundToggleHeightValueCoerce(double input, double modifier)
        {
            return input + 2 * modifier;
        }

        public double SwitcherToggleHeightValueCoerce(double input, double modifier)
        {
            return input * 2;
        }
    }
}
