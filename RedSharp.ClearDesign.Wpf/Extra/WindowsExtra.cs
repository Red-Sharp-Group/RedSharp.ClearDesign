using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Extra
{
    public class WindowsExtra
    {
        public static DependencyProperty TitleContentProperty;
        public static DependencyProperty MinimizeVisibilityProperty;
        public static DependencyProperty MaximizeVisibilityProperty;
        public static DependencyProperty CloseVisibilityProperty;

        static WindowsExtra()
        {
            TitleContentProperty = DependencyProperty.RegisterAttached("TitleContent", typeof(Object), typeof(WindowsExtra));

            MinimizeVisibilityProperty = DependencyProperty.RegisterAttached("MinimizeVisibility", typeof(Visibility), typeof(WindowsExtra));
            MaximizeVisibilityProperty = DependencyProperty.RegisterAttached("MaximizeVisibility", typeof(Visibility), typeof(WindowsExtra));
            CloseVisibilityProperty = DependencyProperty.RegisterAttached("CloseVisibility", typeof(Visibility), typeof(WindowsExtra));
        }

        public static void SetTitleContent(DependencyObject element, Object value)
        {
            element.SetValue(TitleContentProperty, value);
        }

        public static Object GetTitleContent(DependencyObject element)
        {
            return element.GetValue(TitleContentProperty);
        }

        public static void SetMinimizeVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(MinimizeVisibilityProperty, value);
        }

        public static Visibility GetMinimizeVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(MinimizeVisibilityProperty);
        }

        public static void SetMaximizeVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(MaximizeVisibilityProperty, value);
        }

        public static Visibility GetMaximizeVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(MaximizeVisibilityProperty);
        }

        public static void SetCloseVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(CloseVisibilityProperty, value);
        }

        public static Visibility GetCloseVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(CloseVisibilityProperty);
        }
    }
}
