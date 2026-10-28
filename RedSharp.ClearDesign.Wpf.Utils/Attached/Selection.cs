using System.Windows.Media;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Selection
    {
        public static DependencyProperty FillProperty;
        public static DependencyProperty OpacityProperty;

        static Selection()
        {
            FillProperty = DependencyProperty.RegisterAttached("Fill", typeof(Brush), typeof(Selection));
            OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(Selection));
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
