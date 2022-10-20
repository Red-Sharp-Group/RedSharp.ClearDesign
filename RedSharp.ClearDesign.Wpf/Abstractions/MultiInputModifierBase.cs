using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using RedSharp.ClearDesign.Wpf.Interfaces;

namespace RedSharp.ClearDesign.Wpf.Abstracts
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

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null || !(value is string))
                    throw GetConvertFromException(value);

                if (value == null)
                    return base.ConvertFrom(context, culture, value);

                return ((String)value).Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                      .Cast<object>()
                                      .ToList();
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
