using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class Content
    {
        public static DependencyProperty AlternativeProperty;

        static Content()
        {
            AlternativeProperty = DependencyProperty.RegisterAttached("Alternative", typeof(object), typeof(Content));
        }

        public static void SetAlternative(DependencyObject element, object value)
        {
            element.SetValue(AlternativeProperty, value);
        }

        public static object GetAlternative(DependencyObject element)
        {
            return element.GetValue(AlternativeProperty);
        }
    }
}
