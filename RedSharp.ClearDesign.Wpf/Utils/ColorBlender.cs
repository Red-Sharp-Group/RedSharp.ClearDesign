using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Utils
{
    public class ColorBlender : DependencyObject
    {
        public static readonly DependencyProperty BackgroundColorProperty;
        public static readonly DependencyProperty ForegroundColorProperty;
        public static readonly DependencyProperty ColorProperty;

        static ColorBlender()
        {
            BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor), typeof(Color), typeof(ColorBlender), new PropertyMetadata(PropertyChangedCallback));
            ForegroundColorProperty = DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(ColorBlender), new PropertyMetadata(PropertyChangedCallback));

            ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorBlender));
        }

        public Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            private set => SetValue(ColorProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject @object, DependencyPropertyChangedEventArgs arguments)
        {
            var thisObject = (ColorBlender)@object;

            thisObject.Color = ColorHelper.AlphaBlending(thisObject.BackgroundColor, thisObject.ForegroundColor);
        }
    }
}
