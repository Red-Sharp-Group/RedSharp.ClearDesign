using System.Windows.Media;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Panel
    {
        public static DependencyProperty BorderBrushProperty;
        public static DependencyProperty BorderThicknessProperty;

        static Panel()
        {
            BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(Panel));
            BorderThicknessProperty = DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(Panel));
        }

        public static void SetBorderBrush(DependencyObject element, Brush value)
        {
            element.SetValue(BorderBrushProperty, value);
        }

        public static Brush GetBorderBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(BorderBrushProperty);
        }

        public static void SetBorderThickness(DependencyObject element, Thickness value)
        {
            element.SetValue(BorderThicknessProperty, value);
        }

        public static Thickness GetBorderThickness(DependencyObject element)
        {
            return (Thickness)element.GetValue(BorderThicknessProperty);
        }
    }
}
