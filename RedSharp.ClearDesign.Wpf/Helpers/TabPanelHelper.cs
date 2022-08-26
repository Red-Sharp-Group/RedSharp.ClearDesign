using System.Windows;

namespace RedSharp.ClearDesign.Helpers
{
    public static class TabPanelHelper
    {
        public static DependencyProperty MarginProperty;

        static TabPanelHelper()
        {
            MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(TabPanelHelper));
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
