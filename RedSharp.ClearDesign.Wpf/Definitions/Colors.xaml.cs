using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Definitions
{
    public class ResourceNotifier : FrameworkElement
    {
        private static DynamicResourceExtension _extension;
        private static DependencyProperty ValueProperty;

        private static MethodInfo _addOwnerMethod;

        private ResourceSchema _schema;
        private Object _key;

        static ResourceNotifier()
        {
            _extension = new DynamicResourceExtension();

            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Object), typeof(ResourceNotifier), new PropertyMetadata(PropertyChangedCallback));

            _addOwnerMethod = typeof(ResourceDictionary).GetTypeInfo().DeclaredMethods.First(item => item.Name.StartsWith("AddOwner"));
        }

        public ResourceNotifier(ResourceSchema schema, ResourceDictionary dictionary, Object key)
        {
            _addOwnerMethod.Invoke(dictionary, new object[] { this });

            lock (_extension)
            {
                _extension.ResourceKey = key;

                Value = _extension.ProvideValue(null);

                _extension.ResourceKey = String.Empty;
            }

            _schema = schema;
            _key = key;
        }

        public Object Value 
        { 
            get => GetValue(ValueProperty); 
            set => SetValue(ValueProperty, value); 
        }

        private static void PropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs arguments)
        {
            (target as ResourceNotifier).OnValueChanged();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnValueChanged()
        {
            _schema.UpdateResource(_key);
        }
    }

    public interface IResourceModifier
    {
        public Object Result { get; }

        public IEnumerable<Object> Targets { get; }

        public Object Modify(IList<Object> values);
    }

    public abstract class SingleItemModifierBase<TType> : IResourceModifier
    {
        public SingleItemModifierBase(Object resultKey, Object targetKey)
        {
            Result = resultKey;

            Targets = new [] { targetKey };
        }

        public Object Result { get; private set; }

        public IEnumerable<Object> Targets { get; private set; }

        object IResourceModifier.Modify(IList<object> values)
        {
            if (values.Count < 1 || !(values[0] is TType))
                return default(TType);

            return InternalModify((TType)values[0]);
        }

        protected abstract TType InternalModify(TType input);
    }

    public abstract class DoubleItemModifierBase<TType> : IResourceModifier
    {
        public DoubleItemModifierBase(Object resultKey, Object firstKey, Object secondKey)
        {
            Result = resultKey;

            Targets = new [] { firstKey, secondKey };
        }

        public Object Result { get; private set; }

        public IEnumerable<Object> Targets { get; private set; }

        object IResourceModifier.Modify(IList<object> values)
        {
            if (values.Count < 2 || !(values[0] is TType) || !(values[1] is TType))
                return default(TType);

            return InternalModify((TType)values[0], (TType)values[1]);
        }

        protected abstract TType InternalModify(TType first, TType second);
    }

    public class ColorBlenderModifier : DoubleItemModifierBase<Color>
    {
        public ColorBlenderModifier(Object resultKey, Object firstKey, Object secondKey) 
            : base(resultKey, firstKey, secondKey)
        { }

        protected override Color InternalModify(Color first, Color second)
        {
            return ColorHelper.AlphaBlending(first, second);
        }
    }

    public class ColorOpacityModifier : SingleItemModifierBase<Color>
    {
        private double _opacity;

        public ColorOpacityModifier(Object resultKey, Object targetKey, double opacity) 
            : base(resultKey, targetKey)
        {
            _opacity = opacity;
        }

        protected override Color InternalModify(Color input)
        {
            return ColorHelper.ChangeOpacity(input, _opacity);
        }
    }

    public class ResourceSchema : ICollection<IResourceModifier>
    {
        private ResourceDictionary _dictionary;
        private Dictionary<Object, ResourceNotifier> _notifiers;
        private Dictionary<Object, List<IResourceModifier>> _modifiers;
        private HashSet<IResourceModifier> _allModifiers;

        public ResourceSchema(ResourceDictionary dictionary)
        {
            _dictionary = dictionary;
            _notifiers = new Dictionary<Object, ResourceNotifier>();
            _modifiers = new Dictionary<Object, List<IResourceModifier>>();
            _allModifiers = new HashSet<IResourceModifier>();
        }

        public int Count => _allModifiers.Count();

        public bool IsReadOnly => false;

        public void Add(IResourceModifier item)
        {
            if (!_allModifiers.Add(item))
                return;

            foreach (var key in item.Targets)
            {
                if (!_modifiers.TryGetValue(key, out List<IResourceModifier> modifiersList))
                {
                    modifiersList = new List<IResourceModifier>();

                    _modifiers.Add(key, modifiersList);
                }

                modifiersList.Add(item);

                if (!_notifiers.ContainsKey(key))
                    _notifiers.Add(key, new ResourceNotifier(this, _dictionary, key));
            }
        }

        public bool Contains(IResourceModifier item)
        {
            return _allModifiers.Contains(item);
        }

        public bool Remove(IResourceModifier item)
        {
            if (!_allModifiers.Remove(item))
                return false;

            foreach (var key in item.Targets)
            {
                var modifiersList = _modifiers[key];

                modifiersList.Remove(item);

                if (modifiersList.Count == 0)
                    _modifiers.Remove(key);
            }

            return true;
        }

        public void Clear()
        {
            _allModifiers.Clear();
            _modifiers.Clear();
        }

        public void CopyTo(IResourceModifier[] array, int arrayIndex)
        {
            _allModifiers.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _allModifiers.GetEnumerator();
        }

        public IEnumerator<IResourceModifier> GetEnumerator()
        {
            return _allModifiers.GetEnumerator();
        }

        internal void UpdateResource(Object key)
        {
            if (!_modifiers.TryGetValue(key, out List<IResourceModifier> modifiers))
                return;

            var arguments = new List<Object>();

            foreach (var item in modifiers)
            {
                arguments.Clear();

                foreach (var targetKey in item.Targets)
                    arguments.Add(_notifiers[targetKey].Value);

                _dictionary[item.Result] = item.Modify(arguments);
            }
        }
    }

    public partial class Colors : ResourceDictionary
    {
        public static readonly ResourceSchema Schema;

        private const string PrimaryColorKey = "D_DefaultPrimaryColor_";
        private const string SecondaryColorKey = "D_DefaultSecondaryColor_";

        private const string BackgroundColorKey = "D_DefaultBackgroundColor_";
        private const string FieldsColorKey = "D_DefaultFieldsColor_";
        private const string ToggleColorKey = "D_DefaultToggleColor_";

        private const string PrimaryFontColorKey = "D_DefaultPrimaryFontColor_";
        private const string SecondaryFontColorKey = "D_DefaultSecondaryFontColor_";

        private const string ErrorColorKey = "D_DefaultErrorColor_";
        private const string WarningColorKey = "D_DefaultWarningColor_";
        private const string SuccessColorKey = "D_DefaultSuccessColor_";

        private const string LightAccentColorKey = "D_DefaultLightAccentColor_";
        private const string DarkAccentColorKey = "D_DefaultDarkAccentColor_";
        private const string DisabledAccentColorKey = "D_DefaultDisabledAccentColor_";
        private const string StepAccentColorKey = "D_DefaultStepAccentColor_";

        private const string DisabledStateMaskColorKey = "D_DisabledStateMaskColor_";

        private const string Light1ColorKey = "D_Light1Color_";
        private const string Light2ColorKey = "D_Light2Color_";
        private const string Light3ColorKey = "D_Light3Color_";
        private const string Light4ColorKey = "D_Light4Color_";
        private const string Light5ColorKey = "D_Light5Color_";

        private const string Dark1ColorKey = "D_Dark1Color_";
        private const string Dark2ColorKey = "D_Dark2Color_";
        private const string Dark3ColorKey = "D_Dark3Color_";
        private const string Dark4ColorKey = "D_Dark4Color_";
        private const string Dark5ColorKey = "D_Dark5Color_";

        private const string Disabled1ColorKey = "D_Disabled1Color_";
        private const string Disabled2ColorKey = "D_Disabled2Color_";
        private const string Disabled3ColorKey = "D_Disabled3Color_";
        private const string Disabled4ColorKey = "D_Disabled4Color_";
        private const string Disabled5ColorKey = "D_Disabled5Color_";

        private const string Step1ColorKey = "D_Step1Color_";
        private const string Step2ColorKey = "D_Step2Color_";
        private const string Step3ColorKey = "D_Step3Color_";
        private const string Step4ColorKey = "D_Step4Color_";
        private const string Step5ColorKey = "D_Step5Color_";

        private const string FocusBorderColorKey = "D_FocusBorderColor_";
        private const string PrimaryBorderColorKey = "D_PrimaryBorderColor_";
        private const string SecondaryBorderColorKey = "D_SecondaryBorderColor_";
        private const string DisabledBorderColorKey = "D_DisabledBorderColor_";
        private const string DisabledBorderNoOpacityColorKey = "D_DisabledBorderNoOpacityColor_";

        private const string CommonFontColorKey = "D_CommonFontColor_";

        static Colors()
        {
            Schema = new ResourceSchema(Application.Current.Resources)
            {
                new ColorBlenderModifier(FocusBorderColorKey, PrimaryColorKey, Light5ColorKey),
                new ColorBlenderModifier(PrimaryBorderColorKey, PrimaryColorKey, Dark5ColorKey),

                new ColorOpacityModifier(Light1ColorKey, LightAccentColorKey, 0.05),
                new ColorOpacityModifier(Light2ColorKey, LightAccentColorKey, 0.10),
                new ColorOpacityModifier(Light3ColorKey, LightAccentColorKey, 0.15),
                new ColorOpacityModifier(Light4ColorKey, LightAccentColorKey, 0.20),
                new ColorOpacityModifier(Light5ColorKey, LightAccentColorKey, 0.25),

                new ColorOpacityModifier(Dark1ColorKey, DarkAccentColorKey, 0.05),
                new ColorOpacityModifier(Dark2ColorKey, DarkAccentColorKey, 0.10),
                new ColorOpacityModifier(Dark3ColorKey, DarkAccentColorKey, 0.15),
                new ColorOpacityModifier(Dark4ColorKey, DarkAccentColorKey, 0.20),
                new ColorOpacityModifier(Dark5ColorKey, DarkAccentColorKey, 0.25),

                new ColorOpacityModifier(Disabled1ColorKey, DisabledAccentColorKey, 0.10),
                new ColorOpacityModifier(Disabled2ColorKey, DisabledAccentColorKey, 0.25),
                new ColorOpacityModifier(Disabled3ColorKey, DisabledAccentColorKey, 0.40),
                new ColorOpacityModifier(Disabled4ColorKey, DisabledAccentColorKey, 0.55),
                new ColorOpacityModifier(Disabled5ColorKey, DisabledAccentColorKey, 0.70),

                new ColorOpacityModifier(Step1ColorKey, StepAccentColorKey, 0.03),
                new ColorOpacityModifier(Step2ColorKey, StepAccentColorKey, 0.06),
                new ColorOpacityModifier(Step3ColorKey, StepAccentColorKey, 0.09),
                new ColorOpacityModifier(Step4ColorKey, StepAccentColorKey, 0.12),
                new ColorOpacityModifier(Step5ColorKey, StepAccentColorKey, 0.15),
            };
        }

        public static Color PrimaryColor
        {
            get => GetValue<Color>(PrimaryColorKey);
            set => SetValue(PrimaryColorKey, value);
        }

        public static Color SecondaryColor
        {
            get => GetValue<Color>(SecondaryColorKey);
            set => SetValue(SecondaryColorKey, value);
        }


        public static Color BackgroundColor
        {
            get => GetValue<Color>(BackgroundColorKey);
            set => SetValue(BackgroundColorKey, value);
        }

        public static Color FieldsColor
        {
            get => GetValue<Color>(FieldsColorKey);
            set => SetValue(FieldsColorKey, value);
        }

        public static Color ToggleColor
        {
            get => GetValue<Color>(ToggleColorKey);
            set => SetValue(ToggleColorKey, value);
        }


        public static Color PrimaryFontColor
        {
            get => GetValue<Color>(PrimaryFontColorKey);
            set => SetValue(PrimaryFontColorKey, value);
        }

        public static Color SecondaryFontColor
        {
            get => GetValue<Color>(SecondaryFontColorKey);
            set => SetValue(SecondaryFontColorKey, value);
        }


        public static Color ErrorColor
        {
            get => GetValue<Color>(ErrorColorKey);
            set => SetValue(ErrorColorKey, value);
        }

        public static Color WarningColor
        {
            get => GetValue<Color>(WarningColorKey);
            set => SetValue(WarningColorKey, value);
        }

        public static Color SuccessColor
        {
            get => GetValue<Color>(SuccessColorKey);
            set => SetValue(SuccessColorKey, value);
        }


        public static Color LightAccentColor
        {
            get => GetValue<Color>(LightAccentColorKey);
            set => SetValue(LightAccentColorKey, value);
        }

        public static Color DarkAccentColor
        {
            get => GetValue<Color>(DarkAccentColorKey);
            set => SetValue(DarkAccentColorKey, value);
        }

        public static Color DisabledAccentColor
        {
            get => GetValue<Color>(DisabledAccentColorKey);
            set => SetValue(DisabledAccentColorKey, value);
        }

        public static Color StepAccentColor
        {
            get => GetValue<Color>(StepAccentColorKey);
            set => SetValue(StepAccentColorKey, value);
        }


        public static Color DisabledStateMaskColor
        {
            get => GetValue<Color>(DisabledStateMaskColorKey);
            set => SetValue(DisabledStateMaskColorKey, value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TValue GetValue<TValue>(String key)
        {
            return (TValue)Application.Current.Resources[key];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetValue<TValue>(String key, TValue value)
        {
            if (EqualityComparer<TValue>.Default.Equals(GetValue<TValue>(key), value))
                return;

            Application.Current.Resources[key] = value;
        }
    }
}
