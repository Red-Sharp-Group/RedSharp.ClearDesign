using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class DockContent
    {
        public static DependencyProperty LeftProperty;
        public static DependencyProperty TopProperty;
        public static DependencyProperty RightProperty;
        public static DependencyProperty BottomProperty;

        static DockContent()
        {
            LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(object), typeof(DockContent));
            TopProperty = DependencyProperty.RegisterAttached("Top", typeof(object), typeof(DockContent));
            RightProperty = DependencyProperty.RegisterAttached("Right", typeof(object), typeof(DockContent));
            BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(object), typeof(DockContent));
        }

        public static void SetLeft(DependencyObject element, object value)
        {
            element.SetValue(LeftProperty, value);
        }

        public static object GetLeft(DependencyObject element)
        {
            return element.GetValue(LeftProperty);
        }


        public static void SetTop(DependencyObject element, object value)
        {
            element.SetValue(TopProperty, value);
        }

        public static object GetTop(DependencyObject element)
        {
            return element.GetValue(TopProperty);
        }


        public static void SetRight(DependencyObject element, object value)
        {
            element.SetValue(RightProperty, value);
        }

        public static object GetRight(DependencyObject element)
        {
            return element.GetValue(RightProperty);
        }


        public static void SetBottom(DependencyObject element, object value)
        {
            element.SetValue(BottomProperty, value);
        }

        public static object GetBottom(DependencyObject element)
        {
            return element.GetValue(BottomProperty);
        }
    }
}
