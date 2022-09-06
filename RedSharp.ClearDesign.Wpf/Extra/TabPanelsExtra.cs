using System.Windows;

namespace RedSharp.ClearDesign.Extra
{
    public static class TabPanelsExtra
    {
        public static DependencyProperty MarginProperty;

        static TabPanelsExtra()
        {
            MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(TabPanelsExtra));
        }

        public static void SetMargin(DependencyObject element, Thickness value)
        {
            element.SetValue(MarginProperty, value);
        }

        public static Thickness GetMargin(DependencyObject element)
        {
            return (Thickness)element.GetValue(MarginProperty);
        }
    }
}
