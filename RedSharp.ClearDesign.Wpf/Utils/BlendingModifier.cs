using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Shared.Abstracts;

namespace RedSharp.ClearDesign.Utils
{
    [TypeConverter(typeof(BlendingExpressionConverter))]
    public class BlendingModifier : MultiInputModifierBase
    {
        public class BlendingExpressionConverter : TypeConverter
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

                var splitted = source.Split('=', '+')
                                     .Select(item => item.Trim())
                                     .ToList();

                var result = new BlendingModifier()
                {
                    ResultKey = splitted[0]
                };

                for (int i = 1; i < splitted.Count; i++)
                    result.InputKeys.Add(splitted[i]);

                return result;
            }
        }

        protected override object InternalModify(IList<object> values)
        {
            var result = Color.FromArgb(0, 0, 0, 0);

            foreach (var item in values)
                if(item != null)
                    result = ColorHelper.AlphaBlending(result, (Color)item);

            return result;
        }
    }
}
