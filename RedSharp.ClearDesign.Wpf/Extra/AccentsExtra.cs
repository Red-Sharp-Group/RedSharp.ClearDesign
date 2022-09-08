using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Extra
{
    public static class AccentsExtra
    {
        public static DependencyProperty ColorProperty;
        public static DependencyProperty OpacityProperty;

        static AccentsExtra()
        {
            ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(Brush), typeof(AccentsExtra));
            OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(AccentsExtra));
        }

        public static void SetColor(DependencyObject element, Brush value)
        {
            element.SetValue(ColorProperty, value);
        }

        public static Brush GetColor(DependencyObject element)
        {
            return (Brush)element.GetValue(ColorProperty);
        }

        public static void SetOpacity(DependencyObject element, double value)
        {
            element.SetValue(OpacityProperty, value);
        }

        public static double GetOpacity(DependencyObject element)
        {
            return (double)element.GetValue(OpacityProperty);
        }
    }
    public static class ComponentExtra
    {
        public class ColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (!(value is Color))
                    return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                return new SolidColorBrush((Color)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public static IValueConverter Converter => new ColorConverter();
    }
}
