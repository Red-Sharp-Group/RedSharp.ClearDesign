using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Extra
{
    public static class AccentsExtra
    {
        public static DependencyProperty ColorProperty;
        public static DependencyProperty OpacityProperty;

        static AccentsExtra()
        {
            ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(Brush), typeof(AccentsExtra));
            OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(AccentsExtra));
        }

        public static void SetColor(DependencyObject element, Brush value)
        {
            element.SetValue(ColorProperty, value);
        }

        public static Brush GetColor(DependencyObject element)
        {
            return (Brush)element.GetValue(ColorProperty);
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
