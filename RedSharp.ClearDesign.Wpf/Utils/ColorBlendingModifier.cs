using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Utils
{
    public class ColorBlendingModifier : DependencyObject
    {
        public static List<ColorBlendingModifier> _modifiers = new List<ColorBlendingModifier>();

        public static readonly DependencyProperty ModifierProperty;
        public static readonly DependencyProperty InputProperty;
        public static readonly DependencyProperty ValueProperty;

        static ColorBlendingModifier()
        {
            ModifierProperty = DependencyProperty.Register(nameof(Modifier), typeof(Color), typeof(ColorBlendingModifier), new PropertyMetadata(PropertyChangedCallback));
            InputProperty = DependencyProperty.Register(nameof(Input), typeof(Color), typeof(ColorBlendingModifier), new PropertyMetadata(PropertyChangedCallback));
            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Color), typeof(ColorBlendingModifier));
        }

        public ColorBlendingModifier()
        {
            _modifiers.Add(this);
        }

        public Color Modifier
        {
            get => (Color)GetValue(ModifierProperty);
            set => SetValue(ModifierProperty, value);
        }

        public Color Input
        {
            get => (Color)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public Color Value
        {
            get => (Color)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject @object, DependencyPropertyChangedEventArgs arguments)
        {
            var thisObject = (ColorBlendingModifier)@object;

            thisObject.Recalculate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recalculate()
        {
            Value = Process(Input, Modifier);
        }

        protected Color Process(Color input, Color modifier)
        {
            return ColorHelper.AlphaBlending(input, modifier);
        }
    }
}
