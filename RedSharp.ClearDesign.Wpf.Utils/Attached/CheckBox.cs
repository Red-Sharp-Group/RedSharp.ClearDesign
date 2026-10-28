using System.Windows.Media;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Attached
{
    public static class CheckBox
    {
        public static readonly DependencyProperty FalseStateFillProperty;
        public static readonly DependencyProperty NullStateFillProperty;
        public static readonly DependencyProperty TrueStateFillProperty;

        static CheckBox()
        {
            FalseStateFillProperty = DependencyProperty.RegisterAttached("FalseStateFill", typeof(Brush), typeof(CheckBox));
            NullStateFillProperty = DependencyProperty.RegisterAttached("NullStateFill", typeof(Brush), typeof(CheckBox));
            TrueStateFillProperty = DependencyProperty.RegisterAttached("TrueStateFill", typeof(Brush), typeof(CheckBox));
        }

        public static void SetFalseStateFill(DependencyObject element, Brush value)
        {
            element.SetValue(FalseStateFillProperty, value);
        }

        public static Brush GetFalseStateFill(DependencyObject element)
        {
            return (Brush)element.GetValue(FalseStateFillProperty);
        }

        public static void SetNullStateFill(DependencyObject element, Brush value)
        {
            element.SetValue(NullStateFillProperty, value);
        }

        public static Brush GetNullStateFill(DependencyObject element)
        {
            return (Brush)element.GetValue(NullStateFillProperty);
        }

        public static void SetTrueStateFill(DependencyObject element, Brush value)
        {
            element.SetValue(TrueStateFillProperty, value);
        }

        public static Brush GetTrueStateFill(DependencyObject element)
        {
            return (Brush)element.GetValue(TrueStateFillProperty);
        }
    }
}
