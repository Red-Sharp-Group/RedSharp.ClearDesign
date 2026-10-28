using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.ClearDesign.Abstractions
{
    public abstract class StringConverterBase : TypeConverter
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

            return Parse((string)value);
        }

        protected abstract object Parse(String value);
    }
}
