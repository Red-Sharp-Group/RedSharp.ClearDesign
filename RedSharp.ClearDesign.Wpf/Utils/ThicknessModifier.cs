using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using RedSharp.ClearDesign.Wpf.Abstracts;

namespace RedSharp.ClearDesign.Utils
{
    [TypeConverter(typeof(ThicknessModifierExpressionConverter))]
    public class ThicknessModifier : SingleInputModifierBase
    {
        public class ThicknessModifierExpressionConverter : TypeConverter
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

                return ThicknessModifier.ParseExpression((String)value);
            }
        }

        private static readonly char[] _separators = new char[] { '=', '(', ')' };
        private static readonly char[] _thicknessSeparators = new char[] { ',' };

        public Thickness Mask { get; set; }

        /// <summary>
        /// Parsed and filled an instance of <see cref="ThicknessModifier"/>
        /// </summary>
        /// <example>
        /// D_DefaultMarginThicknessFull_ = D_DefaultMargin_ (1)
        /// </example>
        public static ThicknessModifier ParseExpression(String expression)
        {
            var result = new ThicknessModifier();

            if (String.IsNullOrWhiteSpace(expression))
                return result;

            var splitted = expression.Split(_separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            result.ResultKey = splitted[0];
            result.InputKey = splitted[1];

            var tokens = splitted[2].Split(_thicknessSeparators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                    .Select(item => double.Parse(item))
                                    .ToArray();

            Thickness mask;

            switch (tokens.Length)
            {
                case 1:
                    mask = new Thickness(tokens[0]);
                    break;
                case 2:
                    mask = new Thickness(tokens[0], tokens[1], tokens[0], tokens[1]);
                    break;
                case 4:
                    mask = new Thickness(tokens[0], tokens[1], tokens[2], tokens[3]);
                    break;
            }

            result.InputKey = mask;

            return result;
        }

        protected override object InternalModify(object value)
        {
            if (value == null)
                return Mask;

            var input = (double)value;

            return new Thickness
            (
                Mask.Left * input,
                Mask.Top * input,
                Mask.Right * input,
                Mask.Bottom * input
            );
        }
    }
}
