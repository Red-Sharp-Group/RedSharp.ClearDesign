using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RedSharp.ClearDesign.Utils
{
    public class ResourceNotifier : FrameworkElement
    {
        private static DynamicResourceExtension _extension;
        private static DependencyProperty ValueProperty;

        private static MethodInfo _addOwnerMethod;

        private ResourceSchema _schema;
        private object _key;

        static ResourceNotifier()
        {
            _extension = new DynamicResourceExtension();

            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(ResourceNotifier), new PropertyMetadata(PropertyChangedCallback));

            _addOwnerMethod = typeof(ResourceDictionary).GetTypeInfo().DeclaredMethods.First(item => item.Name.StartsWith("AddOwner"));
        }

        public ResourceNotifier(ResourceSchema schema, object key)
        {
            _schema = schema;
            _key = key;
        }

        public void Initialize(ResourceDictionary dictionary)
        {
            _addOwnerMethod.Invoke(dictionary, new object[] { this });

            lock (_extension)
            {
                _extension.ResourceKey = _key;

                Value = _extension.ProvideValue(null);

                _extension.ResourceKey = string.Empty;
            }
        }

        public object Value
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
}
