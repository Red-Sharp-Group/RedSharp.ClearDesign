using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RedSharp.ClearDesign.Wpf.Utils
{
    public class ThicknessToTopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness) 
                return thickness.Top;
            
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
