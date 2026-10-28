using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Control
    {
        public static DependencyProperty CornerRadiusProperty;

        static Control()
        {
            CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(Control));
        }

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }
    }
}
