using System.Windows.Media;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Loader
    {
        public static DependencyProperty IsActiveProperty;
        public static DependencyProperty FillProperty;
        public static DependencyProperty SizeProperty;
        public static DependencyProperty ThicknessProperty;

        static Loader()
        {
            IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(Loader));
            FillProperty = DependencyProperty.RegisterAttached("Fill", typeof(Brush), typeof(Loader));
            SizeProperty = DependencyProperty.RegisterAttached("Size", typeof(double), typeof(Loader));
            ThicknessProperty = DependencyProperty.RegisterAttached("Thickness", typeof(double), typeof(Loader));
        }

        public static void SetIsActive(DependencyObject element, bool value)
        {
            element.SetValue(IsActiveProperty, value);
        }

        public static bool GetIsActive(DependencyObject element)
        {
            return (bool)element.GetValue(IsActiveProperty);
        }

        public static void SetSize(DependencyObject element, double value)
        {
            element.SetValue(SizeProperty, value);
        }

        public static double GetSize(DependencyObject element)
        {
            return (double)element.GetValue(SizeProperty);
        }

        public static void SetFill(DependencyObject element, Brush value)
        {
            element.SetValue(FillProperty, value);
        }

        public static Brush GetFill(DependencyObject element)
        {
            return (Brush)element.GetValue(FillProperty);
        }

        public static void SetThickness(DependencyObject element, double value)
        {
            element.SetValue(ThicknessProperty, value);
        }

        public static double GetThickness(DependencyObject element)
        {
            return (double)element.GetValue(ThicknessProperty);
        }
    }
}
