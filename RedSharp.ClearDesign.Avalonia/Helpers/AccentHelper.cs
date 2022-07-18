using Avalonia;
using Avalonia.Media;

namespace RedSharp.ClearDesign.Helpers
{
    public static class AccentHelper
    {
        public static readonly AttachedProperty<IBrush> ColorProperty;
        public static readonly AttachedProperty<double> OpacityProperty;
        public static readonly AttachedProperty<Thickness> MarginProperty;

        static AccentHelper()
        {
            ColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(ColorProperty)), typeof(AccentHelper));
            OpacityProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, double>(ContentHelper.MakePropertyName(nameof(OpacityProperty)), typeof(AccentHelper));
            MarginProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, Thickness>(ContentHelper.MakePropertyName(nameof(MarginProperty)), typeof(AccentHelper));
        }

        public static void SetColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(ColorProperty, value);
        }

        public static IBrush GetColor(AvaloniaObject element)
        {
            return element.GetValue(ColorProperty);
        }

        public static void SetOpacity(AvaloniaObject element, double value)
        {
            element.SetValue(OpacityProperty, value);
        }

        public static double GetOpacity(AvaloniaObject element)
        {
            return element.GetValue(OpacityProperty);
        }

        public static void SetMargin(AvaloniaObject element, Thickness value)
        {
            element.SetValue(MarginProperty, value);
        }

        public static Thickness GetMargin(AvaloniaObject element)
        {
            return element.GetValue(MarginProperty);
        }
    }
}
