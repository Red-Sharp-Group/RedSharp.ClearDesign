using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Wpf.Abstracts;

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
                if (value == null || !(value is string))
                    throw GetConvertFromException(value);

                if (value == null)
                    return base.ConvertFrom(context, culture, value);

                return BlendingModifier.ParseExpression((String)value);
            }
        }

        private static readonly char[] _separators = new char[] { '=', '+' };

        /// <summary>
        /// Parsed and filled an instance of <see cref="BlendingModifier"/>
        /// </summary>
        /// <example>
        /// D_Background1Color_ = D_DefaultBackgroundColor_ + D_Step1Color_
        /// </example>
        public static BlendingModifier ParseExpression(String expression)
        {
            var result = new BlendingModifier();

            if (String.IsNullOrWhiteSpace(expression))
                return result;

            var splitted = expression.Split(_separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            result.ResultKey = splitted[0];

            for (int i = 1; i < splitted.Length; i++)
                result.InputKeys.Add(splitted[i]);

            return result;
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
