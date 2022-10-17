using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Shared.Abstracts;

namespace RedSharp.ClearDesign.Utils
{
    [TypeConverter(typeof(OpacityExpressionConverter))]
    public class OpacityModifier : SingleInputModifierBase
    {
        public class OpacityExpressionConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                    throw GetConvertFromException(value);

                string source = value as string;

                if (source == null)
                    return base.ConvertFrom(context, culture, value);

                var splitted = source.Split('=', '(', '%', ')')
                                     .Select(item => item.Trim())
                                     .Where(item => !String.IsNullOrEmpty(item))
                                     .ToList();

                return new OpacityModifier()
                {
                    ResultKey = splitted[0],
                    InputKey = splitted[1],
                    Parameter = double.Parse(splitted[2]) / 100.0
                };
            }
        }

        public double Parameter { get; set; }

        protected override object InternalModify(object value)
        {
            Color inputColor = new Color();

            if (value != null)
                inputColor = (Color)value;

            return ColorHelper.ChangeOpacity(inputColor, Parameter);
        }
    }
}
