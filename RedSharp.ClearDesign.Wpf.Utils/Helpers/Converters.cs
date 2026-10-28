using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace RedSharp.ClearDesign.Wpf.Utils.Helpers
{
    public static class Converters
    {
        internal class DelegateValueConvertor : IValueConverter
        {
            public delegate object Conversion(object value, object parameter);

            private Conversion _forward;
            private Conversion _backward;

            public DelegateValueConvertor(Conversion forward, Conversion backward = null)
            {
                _forward = forward;
                _backward = backward;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => _forward.Invoke(value, parameter);

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => _backward != null ? _backward.Invoke(value, parameter) : throw new NotSupportedException();
        }

        internal class DelegateMultiValueConvertor : IMultiValueConverter
        {
            public delegate object ForwardConversion(object[] values, object parameter);
            public delegate object[] BackwardConversion(object values, object parameter);

            private ForwardConversion _forward;
            private BackwardConversion _backward;

            public DelegateMultiValueConvertor(ForwardConversion forward, BackwardConversion backward = null)
            {
                _forward = forward;
                _backward = backward;
            }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
                => _forward.Invoke(values, parameter);

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
                => _backward != null ? _backward.Invoke(value, parameter) : throw new NotSupportedException();
        }


        public static IValueConverter BoolToCollapsed { get; } = Create(ConvertBoolToCollapsed);

        public static IValueConverter BoolToVisible { get; } = Create(ConvertBoolToVisible);


        public static IValueConverter EqualsToCollapsed { get; } = Create(ConvertEqualsToCollapsed);

        public static IValueConverter EqualsToVisible { get; } = Create(ConvertEqualsToVisible);


        public static IValueConverter IsNullOrEmptyToCollapsed { get; } = Create(ConvertIsNullOrEmptyToCollapsed);

        public static IValueConverter IsNullOrEmptyToVisible { get; } = Create(ConvertIsNullOrEmptyToVisible);


        public static IValueConverter ExtractEnumValues { get; } = Create(GetEnumValues);


        public static IValueConverter IsNullOrEmpty { get; } = Create(ConvertIsNullOrEmpty);

        public static IValueConverter IsNotNullOrEmpty { get; } = Create(ConvertIsNotNullOrEmpty);


        public static IValueConverter NotConverter { get; } = Create(InvertBoolean);

        public static IMultiValueConverter AndConverter { get; } = CreateMulti(CompareAsAnd);

        public static IMultiValueConverter OrConverter { get; } = CreateMulti(CompareAsOr);


        public static IMultiValueConverter StringJoin { get; } = CreateMulti(JoinInputStrings);


        public static IValueConverter TrimNumber { get; } = Create(TrimInputNumber);


        public static object JoinInputStrings(object[] values, object parameter = null)
        {
            if (values == null)
                return string.Empty;

            var lines = new List<string>();

            foreach (var item in values)
            {
                if (item == null)
                    continue;

                var line = item.ToString();

                if (string.IsNullOrEmpty(line))
                    continue;

                lines.Add(line);
            }

            if (lines.Count == 0)
                return string.Empty;

            if (lines.Count == 1)
                return lines[0];

            return string.Join(parameter?.ToString() ?? string.Empty, values);
        }

        public static object ConvertIsNullOrEmpty(object value, object parameter)
        {
            if (value == null)
                return true;

            if (value is string castedString)
                return string.IsNullOrEmpty(castedString);

            if (value is short castedShort)
                return castedShort == 0;

            if (value is int castedInt)
                return castedInt == 0;

            if (value is long castedLong)
                return castedLong == 0;

            if (value is ushort castedUShort)
                return castedUShort == 0;

            if (value is uint castedUInt)
                return castedUInt == 0;

            if (value is ulong castedULong)
                return castedULong == 0;

            if (value is Array castedArray)
                return castedArray.Length == 0;

            if (value is ICollection castedCollection)
                return castedCollection.Count == 0;

            if (value is IEnumerable castedEnumerable)
                return !castedEnumerable.GetEnumerator().MoveNext();

            return false;
        }

        public static object ConvertIsNullOrEmptyToVisible(object value, object parameter)
        {
            return ConvertBoolToVisible(ConvertIsNullOrEmpty(value, parameter), parameter);
        }

        public static object ConvertIsNotNullOrEmpty(object value, object parameter)
        {
            return !(bool)ConvertIsNullOrEmpty(value, parameter);
        }

        public static object ConvertIsNullOrEmptyToCollapsed(object value, object parameter)
        {
            return ConvertBoolToVisible(ConvertIsNotNullOrEmpty(value, parameter), parameter);
        }

        public static object InvertBoolean(object value, object parameter)
        {
            return !(bool)value;
        }

        public static object ConvertBoolToVisible(object value, object parameter)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public static object ConvertBoolToCollapsed(object value, object parameter)
        {
            return ConvertBoolToVisible(!(bool)value, parameter);
        }

        public static object CompareAsAnd(object[] values, object parameter = null)
        {
            var result = true;

            for (int i = 0; i < values.Length; i++)
            {
                if (!(values[i] is bool temp))
                    temp = false;

                result = result && temp;

                if (!result)
                    break;
            }

            return result;
        }

        public static object CompareAsOr(object[] values, object parameter = null)
        {
            var result = false;

            for (int i = 0; i < values.Length; i++)
            {
                if (!(values[i] is bool temp))
                    temp = false;

                result = result || temp;

                if (result)
                    break;
            }

            return result;
        }

        public static object GetEnumValues(object value, object parameter)
        {
            if (!(value is Enum))
                return Array.Empty<object>();

            return Enum.GetValues(value.GetType());
        }

        public static object ConvertEqualsToVisible(object value, object parameter)
        {
            if (object.Equals(value, parameter))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public static object ConvertEqualsToCollapsed(object value, object parameter)
        {
            if (object.Equals(value, parameter))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public static object TrimInputNumber(object value, object parameter)
        {
            if (value == null || parameter == null)
                return string.Empty;

            var intValue = (int)value;
            var length = 0;

            if (parameter is string)
                length = int.Parse((string)parameter);
            else
                length = (int)parameter;

            if (length <= 0)
                throw new ArgumentOutOfRangeException();

            var maxValue = 10;

            for (int i = 1; i < length; i++)
                maxValue *= 10;

            if (intValue >= maxValue)
                return $"{maxValue - 1}+";
            else
                return intValue.ToString();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IValueConverter Create(DelegateValueConvertor.Conversion forward, DelegateValueConvertor.Conversion backward = null)
            => new DelegateValueConvertor(forward, backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMultiValueConverter CreateMulti(DelegateMultiValueConvertor.ForwardConversion forward, DelegateMultiValueConvertor.BackwardConversion backward = null)
            => new DelegateMultiValueConvertor(forward, backward);
    }
}
