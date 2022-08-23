using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSharp.ClearDesign.Helpers
{
    public static class AccentHelper
    {
        public static DependencyProperty OpacityProperty;

        static AccentHelper()
        {
            OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(AccentHelper));
        }

        public static void SetOpacity(DependencyObject element, double value)
        {
            element.SetValue(OpacityProperty, value);
        }

        public static double GetOpacity(DependencyObject element)
        {
            return (double)element.GetValue(OpacityProperty);
        }
    }
}
