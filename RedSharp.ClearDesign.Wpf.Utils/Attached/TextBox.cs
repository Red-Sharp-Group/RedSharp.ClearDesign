using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class TextBox
    {
        public static DependencyProperty PlaceholderProperty;

        static TextBox()
        {
            PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBox));
        }

        public static void SetPlaceholder(DependencyObject element, string value)
        {
            element.SetValue(PlaceholderProperty, value);
        }

        public static string GetPlaceholder(DependencyObject element)
        {
            return (string)element.GetValue(PlaceholderProperty);
        }
    }
}
