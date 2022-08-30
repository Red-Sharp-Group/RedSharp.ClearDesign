using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace RedSharp.ClearDesign.Helpers
{
    public class CornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radius = 0.0;

            try
            {
                var diameter = System.Convert.ToDouble(value);
                
                radius = diameter / 2;
            }
            catch
            { }

            return new CornerRadius(radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public static class ScrollHelper
    {
        public static DependencyProperty ThumbColorProperty;
        public static DependencyProperty TrackColorProperty;
        public static DependencyProperty RepeatButtonsVisibleProperty;
        public static DependencyProperty RepeatButtonsOpacityProperty;
        public static DependencyProperty TrackWidthProperty;
        public static DependencyProperty TrackHeightProperty;

        static ScrollHelper()
        {
            ThumbColorProperty = DependencyProperty.RegisterAttached("ThumbColor", typeof(Brush), typeof(ScrollHelper));
            TrackColorProperty = DependencyProperty.RegisterAttached("TrackColor", typeof(Brush), typeof(ScrollHelper));

            RepeatButtonsVisibleProperty = DependencyProperty.RegisterAttached("RepeatButtonsVisible", typeof(Visibility), typeof(ScrollHelper));
            RepeatButtonsOpacityProperty = DependencyProperty.RegisterAttached("RepeatButtonsOpacity", typeof(double), typeof(ScrollHelper));

            TrackWidthProperty = DependencyProperty.RegisterAttached("TrackWidth", typeof(double), typeof(ScrollHelper));
            TrackHeightProperty = DependencyProperty.RegisterAttached("TrackHeight", typeof(double), typeof(ScrollHelper));

            RadiusConverter = new CornerRadiusConverter();
        }

        public static CornerRadiusConverter RadiusConverter { get; private set; }

        public static void SetThumbColor(DependencyObject element, Brush value)
        {
            element.SetValue(ThumbColorProperty, value);
        }

        public static Brush GetThumbColor(DependencyObject element)
        {
            return (Brush)element.GetValue(ThumbColorProperty);
        }

        public static void SetTrackColor(DependencyObject element, Brush value)
        {
            element.SetValue(TrackColorProperty, value);
        }

        public static Brush GetTrackColor(DependencyObject element)
        {
            return (Brush)element.GetValue(TrackColorProperty);
        }

        public static void SetRepeatButtonsVisible(DependencyObject element, Visibility value)
        {
            element.SetValue(RepeatButtonsVisibleProperty, value);
        }

        public static Visibility GetRepeatButtonsVisible(DependencyObject element)
        {
            return (Visibility)element.GetValue(RepeatButtonsVisibleProperty);
        }

        public static void SetRepeatButtonsOpacity(DependencyObject element, double value)
        {
            element.SetValue(RepeatButtonsOpacityProperty, value);
        }

        public static double GetRepeatButtonsOpacity(DependencyObject element)
        {
            return (double)element.GetValue(RepeatButtonsOpacityProperty);
        }

        public static void SetTrackWidth(DependencyObject element, double value)
        {
            element.SetValue(TrackWidthProperty, value);
        }

        public static double GetTrackWidth(DependencyObject element)
        {
            return (double)element.GetValue(TrackWidthProperty);
        }

        public static void SetTrackHeight(DependencyObject element, double value)
        {
            element.SetValue(TrackHeightProperty, value);
        }

        public static double GetTrackHeight(DependencyObject element)
        {
            return (double)element.GetValue(TrackHeightProperty);
        }
    }
}
