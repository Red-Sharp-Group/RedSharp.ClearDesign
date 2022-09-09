using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace RedSharp.ClearDesign.Helpers
{
    public static class Converters
    {
        public class ObjectIsNullOrEmpty : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                    return true;

                if (value is String castedString)
                    return String.IsNullOrEmpty(castedString);

                if (value is Array castedArray)
                    return castedArray.Length == 0;

                if (value is ICollection castedCollection)
                    return castedCollection.Count == 0;

                if (value is IEnumerable castedEnumerable)
                    return !castedEnumerable.GetEnumerator().MoveNext();

                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }

        public static IValueConverter IsNullOrEmpty { get; private set; } = new ObjectIsNullOrEmpty();
    }
}
