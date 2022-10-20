using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Wpf.Abstracts;

namespace RedSharp.ClearDesign.Utils
{
    [TypeConverter(typeof(OpacityExpressionConverter))]
    public class OpacityModifier : SingleInputModifierBase
    {
        public class OpacityExpressionConverter : StringConverterBase
        {
            protected override object Parse(string value)
            {
                return ParseExpression(value);
            }
        }

        private static readonly char[] _separators = new char[] { '=', '(', '%', ')' };

        public double Parameter { get; set; }

        /// <summary>
        /// Parsed and filled an instance of <see cref="OpacityModifier"/>
        /// </summary>
        /// <example>
        /// D_Disabled5Color_ = D_DefaultStepAccentColor_ (15%)
        /// </example>
        public static OpacityModifier ParseExpression(String expression)
        {
            var result = new OpacityModifier();

            if (String.IsNullOrWhiteSpace(expression))
                return result;

            var splitted = expression.Split(_separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            result.ResultKey = splitted[0];
            result.InputKey = splitted[1];
            result.Parameter = double.Parse(splitted[2]) / 100.0;

            return result;
        }

        protected override object InternalModify(object value)
        {
            Color inputColor = new Color();

            if (value != null)
                inputColor = (Color)value;

            return ColorHelper.ChangeOpacity(inputColor, Parameter);
        }
    }
}
