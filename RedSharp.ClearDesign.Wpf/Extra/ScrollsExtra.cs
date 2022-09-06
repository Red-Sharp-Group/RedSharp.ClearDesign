using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace RedSharp.ClearDesign.Extra
{
    public static class ScrollsExtra
    {
        public static DependencyProperty ThumbColorProperty;
        public static DependencyProperty TrackColorProperty;
        public static DependencyProperty RepeatButtonsVisibleProperty;
        public static DependencyProperty RepeatButtonsOpacityProperty;
        public static DependencyProperty TrackWidthProperty;
        public static DependencyProperty TrackHeightProperty;

        static ScrollsExtra()
        {
            ThumbColorProperty = DependencyProperty.RegisterAttached("ThumbColor", typeof(Brush), typeof(ScrollsExtra));
            TrackColorProperty = DependencyProperty.RegisterAttached("TrackColor", typeof(Brush), typeof(ScrollsExtra));

            RepeatButtonsVisibleProperty = DependencyProperty.RegisterAttached("RepeatButtonsVisible", typeof(Visibility), typeof(ScrollsExtra));
            RepeatButtonsOpacityProperty = DependencyProperty.RegisterAttached("RepeatButtonsOpacity", typeof(double), typeof(ScrollsExtra));

            TrackWidthProperty = DependencyProperty.RegisterAttached("TrackWidth", typeof(double), typeof(ScrollsExtra));
            TrackHeightProperty = DependencyProperty.RegisterAttached("TrackHeight", typeof(double), typeof(ScrollsExtra));
        }

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
