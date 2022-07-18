using Avalonia;

namespace RedSharp.ClearDesign.Abstractions
{
    public delegate TValue Coerce<TValue, TModifier>(TValue value, TModifier modifier);

    public abstract class ModifierBase<TValue, TModifier> : AvaloniaObject
    {
        public static readonly DirectProperty<ModifierBase<TValue, TModifier>, TModifier> ModifierProperty;
        public static readonly DirectProperty<ModifierBase<TValue, TModifier>, Coerce<TValue, TModifier>> CoerceProperty;
        public static readonly DirectProperty<ModifierBase<TValue, TModifier>, TValue> InputProperty;
        public static readonly DirectProperty<ModifierBase<TValue, TModifier>, TValue> ValueProperty;

        private TModifier _modifier;
        private Coerce<TValue, TModifier> _coerce;
        private TValue _input;
        private TValue _value;

        static ModifierBase()
        {
            ModifierProperty = AvaloniaProperty.RegisterDirect<ModifierBase<TValue, TModifier>, TModifier>(nameof(Modifier),
                item => item.Modifier, (item, value) => item.Modifier = value);

            CoerceProperty = AvaloniaProperty.RegisterDirect<ModifierBase<TValue, TModifier>, Coerce<TValue, TModifier>>(nameof(Coerce),
                item => item.Coerce, (item, value) => item.Coerce = value);

            InputProperty = AvaloniaProperty.RegisterDirect<ModifierBase<TValue, TModifier>, TValue>(nameof(Input),
                item => item.Input, (item, value) => item.Input = value);

            ValueProperty = AvaloniaProperty.RegisterDirect<ModifierBase<TValue, TModifier>, TValue>(nameof(Value), 
                item => item.Value);
        }

        public TModifier Modifier
        {
            get => _modifier;
            set
            {
                if (SetAndRaise(ModifierProperty, ref _modifier, value))
                    Recalculate();
            }
        }

        public Coerce<TValue, TModifier> Coerce
        {
            get => _coerce;
            set
            {
                if (SetAndRaise(CoerceProperty, ref _coerce, value))
                    Recalculate();
            }
        }

        public TValue Input
        {
            get => _input;
            set
            {
                if (SetAndRaise(InputProperty, ref _input, value))
                    Recalculate();
            }
        }

        public TValue Value
        {
            get => _value;
            private set => SetAndRaise(ValueProperty, ref _value, value);
        }

        private void Recalculate()
        {
            if(_coerce != null)
                Value = _coerce.Invoke(Input, Modifier);
            else
                Value = Input;
        }
    }
}
