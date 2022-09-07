using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RedSharp.ClearDesign.Styles
{
    public partial class Toggles : ResourceDictionary
    {
        private void OnBorderSizeChanged(Object sender, SizeChangedEventArgs arguments)
        {
            var border = (Border)sender;

            var newDiameter = Math.Min(arguments.NewSize.Width, arguments.NewSize.Height);
            var previousDiameter = Math.Min(arguments.PreviousSize.Width, arguments.PreviousSize.Height);

            if (Math.Abs(newDiameter - previousDiameter) < 0.01)
                return;

            var radius = Math.Floor(newDiameter / 2);

            border.CornerRadius = new CornerRadius(radius);
        }
    }
}
