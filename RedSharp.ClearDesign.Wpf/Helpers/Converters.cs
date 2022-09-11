using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RedSharp.ClearDesign.Abstractions;

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

    public class TestVM : DefinitionBase, IDataErrorInfo
    {
        private String _text;

        public TestVM() : base(null)
        { }

        public static TestVM Instance { get; private set; } = new TestVM();

        public String Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;

                _text = value;

                RaisePropertyChanged();
            }
        }

        public String this[string columnName]
        {
            get
            {
                if (_text != null && _text.Length > 10)
                    return "Too much characters";

                return null;
            }
        }

        public String Error { get; set; }
    }
}
