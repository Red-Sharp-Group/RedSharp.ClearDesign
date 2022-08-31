using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Helpers
{
    public static class TogglesHelper
    {
        public static readonly DependencyProperty FalseIconColorProperty;
        public static readonly DependencyProperty NullIconColorProperty;
        public static readonly DependencyProperty TrueIconColorProperty;

        public static readonly DependencyProperty FalseIconVisibilityProperty;
        public static readonly DependencyProperty NullIconVisibilityProperty;
        public static readonly DependencyProperty TrueIconVisibilityProperty;

        static TogglesHelper()
        {
            FalseIconColorProperty = DependencyProperty.RegisterAttached("FalseIconColor", typeof(Brush), typeof(TogglesHelper));
            NullIconColorProperty = DependencyProperty.RegisterAttached("NullIconColor", typeof(Brush), typeof(TogglesHelper));
            TrueIconColorProperty = DependencyProperty.RegisterAttached("TrueIconColor", typeof(Brush), typeof(TogglesHelper));

            FalseIconVisibilityProperty = DependencyProperty.RegisterAttached("FalseIconVisibility", typeof(Visibility), typeof(TogglesHelper));
            NullIconVisibilityProperty = DependencyProperty.RegisterAttached("NullIconVisibility", typeof(Visibility), typeof(TogglesHelper));
            TrueIconVisibilityProperty = DependencyProperty.RegisterAttached("TrueIconVisibility", typeof(Visibility), typeof(TogglesHelper));
        }

        public static void SetFalseIconColor(DependencyObject element, Brush value)
        {
            element.SetValue(FalseIconColorProperty, value);
        }

        public static Brush GetFalseIconColor(DependencyObject element)
        {
            return (Brush)element.GetValue(FalseIconColorProperty);
        }

        public static void SetNullIconColor(DependencyObject element, Brush value)
        {
            element.SetValue(NullIconColorProperty, value);
        }

        public static Brush GetNullIconColor(DependencyObject element)
        {
            return (Brush)element.GetValue(NullIconColorProperty);
        }

        public static void SetTrueIconColor(DependencyObject element, Brush value)
        {
            element.SetValue(TrueIconColorProperty, value);
        }

        public static Brush GetTrueIconColor(DependencyObject element)
        {
            return (Brush)element.GetValue(TrueIconColorProperty);
        }

        public static void SetFalseIconVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(FalseIconVisibilityProperty, value);
        }

        public static Visibility GetFalseIconVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(FalseIconVisibilityProperty);
        }

        public static void SetNullIconVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(NullIconVisibilityProperty, value);
        }

        public static Visibility GetNullIconVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(NullIconVisibilityProperty);
        }

        public static void SetTrueIconVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(TrueIconVisibilityProperty, value);
        }

        public static Visibility GetTrueIconVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(TrueIconVisibilityProperty);
        }
    }
}
