using System;
using System.Runtime.CompilerServices;
using Avalonia;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ContentHelper
    {
        public static readonly AttachedProperty<bool> LayoutRoundingProperty;

        static ContentHelper()
        {
            LayoutRoundingProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>(MakePropertyName(nameof(LayoutRoundingProperty)), typeof(TogglesHelper));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String MakePropertyName(String input)
        {
            const int ToRemoveLength = 8;

            return input.Substring(0, input.Length - ToRemoveLength);
        }

        public static void SetLayoutRounding(AvaloniaObject element, bool value)
        {
            element.SetValue(LayoutRoundingProperty, value);
        }

        public static bool GetLayoutRounding(AvaloniaObject element)
        {
            return element.GetValue(LayoutRoundingProperty);
        }
    }
}
