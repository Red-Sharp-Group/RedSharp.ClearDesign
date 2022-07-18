using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ScrollHelper
    {
        public static readonly AttachedProperty<IBrush> ThumbColorProperty;
        public static readonly AttachedProperty<IBrush> TrackColorProperty;
        public static readonly AttachedProperty<bool> RepeatButtonsVisibleProperty;
        public static readonly AttachedProperty<double> RepeatButtonsOpacityProperty;
        public static readonly AttachedProperty<double> TrackWidthProperty;
        public static readonly AttachedProperty<double> TrackHeightProperty;

        static ScrollHelper()
        {
            ThumbColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(ThumbColorProperty)), typeof(ScrollHelper));
            TrackColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(TrackColorProperty)), typeof(ScrollHelper));
            
            RepeatButtonsVisibleProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>(ContentHelper.MakePropertyName(nameof(RepeatButtonsVisibleProperty)), typeof(ScrollHelper));
            RepeatButtonsOpacityProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, double>(ContentHelper.MakePropertyName(nameof(RepeatButtonsOpacityProperty)), typeof(ScrollHelper));
            
            TrackWidthProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, double>(ContentHelper.MakePropertyName(nameof(TrackWidthProperty)), typeof(ScrollHelper));
            TrackHeightProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, double>(ContentHelper.MakePropertyName(nameof(TrackHeightProperty)), typeof(ScrollHelper));
        }

        public static void GetThumbColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(ThumbColorProperty, value);
        }

        public static IBrush SetThumbColor(AvaloniaObject element)
        {
            return element.GetValue(ThumbColorProperty);
        }

        public static void GetTrackColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(TrackColorProperty, value);
        }

        public static IBrush SetTrackColor(AvaloniaObject element)
        {
            return element.GetValue(TrackColorProperty);
        }

        public static void GetRepeatButtonsVisible(AvaloniaObject element, bool value)
        {
            element.SetValue(RepeatButtonsVisibleProperty, value);
        }

        public static bool SetRepeatButtonsVisible(AvaloniaObject element)
        {
            return element.GetValue(RepeatButtonsVisibleProperty);
        }

        public static void GetRepeatButtonsOpacity(AvaloniaObject element, double value)
        {
            element.SetValue(RepeatButtonsOpacityProperty, value);
        }

        public static double SetRepeatButtonsOpacity(AvaloniaObject element)
        {
            return element.GetValue(RepeatButtonsOpacityProperty);
        }

        public static void GetTrackWidth(AvaloniaObject element, double value)
        {
            element.SetValue(TrackWidthProperty, value);
        }

        public static double SetTrackWidth(AvaloniaObject element)
        {
            return element.GetValue(TrackWidthProperty);
        }

        public static void GetTrackHeight(AvaloniaObject element, double value)
        {
            element.SetValue(TrackHeightProperty, value);
        }

        public static double SetTrackHeight(AvaloniaObject element)
        {
            return element.GetValue(TrackHeightProperty);
        }
    }
}
