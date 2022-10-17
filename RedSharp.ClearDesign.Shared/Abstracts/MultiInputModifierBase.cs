using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using RedSharp.ClearDesign.Shared.Interfaces;

namespace RedSharp.ClearDesign.Shared.Abstracts
{
    public abstract class MultiInputModifierBase : IResourceModifier
    {
        public class InputKeysConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                    throw GetConvertFromException(value);

                string source = value as string;

                if (source == null)
                    return base.ConvertFrom(context, culture, value);

                return value.ToString()
                            .Split(',')
                            .Select(item => (object)item.Trim())
                            .ToList();
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value == null)
                    return base.ConvertTo(context, culture, value, destinationType);

                return string.Join(", ", ((IEnumerable<object>)value).Select(item => item.ToString()));
            }
        }

        public MultiInputModifierBase()
        {
            InputKeys = new List<object>();
        }

        public object ResultKey { get; set; }

        IEnumerable<object> IResourceModifier.InputKeys => InputKeys;

        [TypeConverter(typeof(InputKeysConverter))]
        public List<object> InputKeys { get; set; }

        object IResourceModifier.Modify(IList<object> values)
        {
            if (InputKeys == null)
                throw new Exception("Input keys weren't initialized.");

            if (values.Count != InputKeys.Count)
                throw new ArgumentException("Values number doesn't match to parameters number.");

            return InternalModify(values);
        }

        protected abstract object InternalModify(IList<object> values);
    }
}
