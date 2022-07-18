using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Utils
{
    public class BlendColorBrush : Brush, ISolidColorBrush
    {
        public static readonly DirectProperty<BlendColorBrush, Color> BackgroundColorProperty;
        public static readonly DirectProperty<BlendColorBrush, Color> ForegroundColorProperty;
        public static readonly DirectProperty<BlendColorBrush, Color> ColorProperty;

        private Color _backgroundColor;
        private Color _foregroundColor;
        private Color _resultColor;

        static BlendColorBrush()
        {
            BackgroundColorProperty = AvaloniaProperty.RegisterDirect<BlendColorBrush, Color>(nameof(BackgroundColor),
                item => item.BackgroundColor, (item, value) => item.BackgroundColor = value);

            ForegroundColorProperty = AvaloniaProperty.RegisterDirect<BlendColorBrush, Color>(nameof(ForegroundColor),
                item => item.ForegroundColor, (item, value) => item.ForegroundColor = value);

            ColorProperty = AvaloniaProperty.RegisterDirect<BlendColorBrush, Color>(nameof(Color),
                item => item.Color);

            AffectsRender<BlendColorBrush>(ColorProperty);
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (SetAndRaise(BackgroundColorProperty, ref _backgroundColor, value))
                    Color = ColorHelper.AlphaBlending(_backgroundColor, _foregroundColor);
            }
        }

        public Color ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                if (SetAndRaise(ForegroundColorProperty, ref _foregroundColor, value))
                    Color = ColorHelper.AlphaBlending(_backgroundColor, _foregroundColor);
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
    }
}
