using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace RedSharp.ClearDesign.Utils
{
    public class OpasityBrushColor : Brush, ISolidColorBrush
    {
        public static readonly DirectProperty<OpasityBrushColor, Color> InputColorProperty;
        public static readonly DirectProperty<OpasityBrushColor, double> InputOpacityProperty;
        public static readonly DirectProperty<OpasityBrushColor, Color> ColorProperty;

        private Color _inputColor;
        private double _inputOpacity;
        private Color _resultColor;

        static OpasityBrushColor()
        {
            InputColorProperty = AvaloniaProperty.RegisterDirect<OpasityBrushColor, Color>(nameof(InputColor),
                item => item.InputColor, (item, value) => item.InputColor = value);

            InputOpacityProperty = AvaloniaProperty.RegisterDirect<OpasityBrushColor, double>(nameof(InputOpacity),
                item => item.InputOpacity, (item, value) => item.InputOpacity = value);

            ColorProperty = AvaloniaProperty.RegisterDirect<OpasityBrushColor, Color>(nameof(Color),
                item => item.Color);

            AffectsRender<OpasityBrushColor>(ColorProperty);
        }

        public Color InputColor
        {
            get => _inputColor;
            set
            {
                if (SetAndRaise(InputColorProperty, ref _inputColor, value))
                    Color = CalculateColor(_inputColor, _inputOpacity);
            }
        }

        public double InputOpacity
        {
            get => _inputOpacity;
            set
            {
                if (SetAndRaise(InputOpacityProperty, ref _inputOpacity, value))
                    Color = CalculateColor(_inputColor, _inputOpacity);
            }
        }

        public Color Color
        {
            get => _resultColor;
            private set => SetAndRaise(ColorProperty, ref _resultColor, value);
        }

        /// <inheritdoc/>
        public override IBrush ToImmutable()
        {
            return new ImmutableSolidColorBrush(this);
        }
        
        private static Color CalculateColor(Color input, double opacity)
        {
            byte alpha = (byte)Math.Ceiling(byte.MaxValue * opacity);

            return new Color(alpha, input.R, input.G, input.B);
        }
    }
}
