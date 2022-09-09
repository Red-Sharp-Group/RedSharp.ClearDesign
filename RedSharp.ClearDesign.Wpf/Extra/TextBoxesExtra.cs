using System;
using System.Windows;

namespace RedSharp.ClearDesign.Extra
{
    public static class TextBoxesExtra
    {
        public static DependencyProperty PlaceholderProperty;
        public static DependencyProperty PlaceholderVisibilityProperty;
        public static DependencyProperty PresenterVisibilityProperty;
        public static DependencyProperty WrapperVisibilityProperty;

        static TextBoxesExtra()
        {
            PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(String), typeof(TextBoxesExtra));
            PlaceholderVisibilityProperty = DependencyProperty.RegisterAttached("PlaceholderVisibility", typeof(Visibility), typeof(TextBoxesExtra));
            PresenterVisibilityProperty = DependencyProperty.RegisterAttached("PresenterVisibility", typeof(Visibility), typeof(TextBoxesExtra));
            WrapperVisibilityProperty = DependencyProperty.RegisterAttached("WrapperVisibility", typeof(Visibility), typeof(TextBoxesExtra));
        }

        public static void SetPlaceholder(DependencyObject element, String value)
        {
            element.SetValue(PlaceholderProperty, value);
        }

        public static String GetPlaceholder(DependencyObject element)
        {
            return (String)element.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholderVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(PlaceholderVisibilityProperty, value);
        }

        public static Visibility GetPlaceholderVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(PlaceholderVisibilityProperty);
        }

        public static void SetPresenterVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(PlaceholderProperty, value);
        }

        public static Visibility GetPresenterVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(PlaceholderProperty);
        }

        public static void SetWrapperVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(WrapperVisibilityProperty, value);
        }

        public static Visibility GetWrapperVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(WrapperVisibilityProperty);
        }
    }
}
