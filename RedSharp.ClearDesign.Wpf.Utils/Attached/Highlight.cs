using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Highlight
    {
        public static DependencyProperty FillProperty;
        public static DependencyProperty OpacityProperty;

        static Highlight()
        {
            FillProperty = DependencyProperty.RegisterAttached("Fill", typeof(Brush), typeof(Highlight));
            OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(Highlight));
        }

        public static void SetFill(DependencyObject element, Brush value)
        {
            element.SetValue(FillProperty, value);
        }

        public static Brush GetFill(DependencyObject element)
        {
            return (Brush)element.GetValue(FillProperty);
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
