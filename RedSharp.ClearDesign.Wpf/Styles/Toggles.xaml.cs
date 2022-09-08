using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Styles
{
    public partial class Toggles : ResourceDictionary
    {
        private void OnBorderSizeChanged(Object sender, SizeChangedEventArgs arguments)
        {
            ControlsHelper.SetCornerRadiusForBorder((Border)sender, arguments.NewSize, arguments.PreviousSize);
        }
    }
}
