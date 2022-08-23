using System;
using System.Windows;
using System.Windows.Media;

namespace RedSharp.ClearDesign.Utils
{
    public class OpacityBlender : DependencyObject
    {
        public static readonly DependencyProperty InputColorProperty;
        public static readonly DependencyProperty OpacityProperty;
        public static readonly DependencyProperty ColorProperty;

        static OpacityBlender()
        {
            InputColorProperty = DependencyProperty.Register(nameof(InputColor), typeof(Color), typeof(OpacityBlender), new PropertyMetadata(PropertyChangedCallback));
            OpacityProperty = DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(OpacityBlender), new PropertyMetadata(PropertyChangedCallback));

            ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(OpacityBlender));
        }

        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        public Color InputColor
        {
            get => (Color)GetValue(InputColorProperty);
            set => SetValue(InputColorProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            private set => SetValue(ColorProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject @object, DependencyPropertyChangedEventArgs arguments)
        {
            var thisObject = (OpacityBlender)@object;

            var color = thisObject.InputColor;
            var alpha = (byte)Math.Round((double)byte.MaxValue * thisObject.Opacity);

            thisObject.Color = Color.FromArgb(alpha, color.R, color.G, color.B);
        }
    }
}
