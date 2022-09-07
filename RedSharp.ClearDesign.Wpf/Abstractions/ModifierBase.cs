using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSharp.ClearDesign.Abstractions
{
    public delegate TValue Coerce<TValue, TModifier>(TValue value, TModifier modifier);

    public abstract class ModifierBase<TValue, TModifier> : DependencyObject
    {
        public static readonly DependencyProperty ModifierProperty;
        public static readonly DependencyProperty InputProperty;
        public static readonly DependencyProperty ValueProperty;

        static ModifierBase()
        {
            ModifierProperty = DependencyProperty.Register(nameof(Modifier), typeof(TModifier), typeof(ModifierBase<TValue, TModifier>), new PropertyMetadata(PropertyChangedCallback));
            InputProperty = DependencyProperty.Register(nameof(Input), typeof(TValue), typeof(ModifierBase<TValue, TModifier>), new PropertyMetadata(PropertyChangedCallback));
            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TValue), typeof(ModifierBase<TValue, TModifier>));
        }

        public TModifier Modifier
        {
            get => (TModifier)GetValue(ModifierProperty);
            set => SetValue(ModifierProperty, value);
        }

        public TValue Input
        {
            get => (TValue)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public TValue Value
        {
            get => (TValue)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject @object, DependencyPropertyChangedEventArgs arguments)
        {
            var thisObject = (ModifierBase<TValue, TModifier>)@object;

            thisObject.Recalculate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Recalculate()
        {
            Value = Process(Input, Modifier);
        }

        protected abstract TValue Process(TValue input, TModifier modifier);
    }
}
