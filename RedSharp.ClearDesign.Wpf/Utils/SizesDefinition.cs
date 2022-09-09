using System.Runtime.CompilerServices;
using System.Windows;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class SizesDefinition : DefinitionBase
    {
        public SizesDefinition(ResourceDictionary resources) : base(resources)
        { }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--SIZES DEFINITIONS-->

        private const string HeightKey = "D_DefaultHeight_";
        private const string WidthKey = "D_DefaultWidth_";

        private const string ToggleHeightKey = "D_DefaultToggleHeight_";

        private const string MarginKey = "D_DefaultMargin_";
        private const string PaddingKey = "D_DefaultPadding_";

        private const string BorderKey = "D_DefaultBorder_";
        private const string FocusKey = "D_DefaultFocus_";

        public double Height
        {
            get => GetValue<double>(HeightKey);
            set => SetAndRaise(HeightKey, value);
        }

        public double Width
        {
            get => GetValue<double>(WidthKey);
            set => SetAndRaise(WidthKey, value);
        }


        public double ToggleHeight
        {
            get => GetValue<double>(ToggleHeightKey);
            set
            {
                if (!SetAndRaise(ToggleHeightKey, value))
                    return;

                RoundToggleHeight = value + 2 * Border;
                SwitchToggleWidth = value * 2;
            }
        }


        public double Margin
        {
            get => GetValue<double>(MarginKey);
            set
            {
                if (!SetAndRaise(MarginKey, value))
                    return;

                MarginThicknessFull = Modify(new Thickness(1), value);
                MarginThicknessLeftRight = Modify(new Thickness(1, 0, 1, 0), value);
                MarginThicknessTopBottom = Modify(new Thickness(0, 1, 0, 1), value);
                MarginThicknessRight = Modify(new Thickness(0, 0, 1, 0), value);
            }
        }

        public double Padding
        {
            get => GetValue<double>(PaddingKey);
            set
            {
                if (SetAndRaise(PaddingKey, value))
                    return;

                PaddingThicknessFull = Modify(new Thickness(1), value);
                PaddingThicknessLeft = Modify(new Thickness(1, 0, 0, 0), value);
                PaddingThicknessLeftRight = Modify(new Thickness(1, 0, 1, 0), value);
            }
        }


        public double Border
        {
            get => GetValue<double>(BorderKey);
            set
            {
                if (!SetAndRaise(BorderKey, value))
                    return;

                OutstandingFocus = value - Focus;
                RoundToggleHeight = ToggleHeight + 2 * value;

                BorderThicknessFull = Modify(new Thickness(1), value);
                BorderThicknessTop = Modify(new Thickness(0, 1, 0, 0), value);
                BorderThicknessLeftRight = Modify(new Thickness(1, 0, 1, 0), value);
                BorderThicknessLeftTopRight = Modify(new Thickness(1, 1, 1, 0), value);
            }
        }

        public double Focus
        {
            get => GetValue<double>(FocusKey);
            set
            {
                if (!SetAndRaise(FocusKey, value))
                    return;

                OutstandingFocus = Border - value;

                FocusThicknessFull = Modify(new Thickness(1), value);
            }
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--COMPUTED SIZES-->

        private const string OutstandingFocusKey = "D_OutstandingFocus_";
        private const string RoundToggleHeightKey = "D_RoundToggleHeight_";
        private const string SwitchToggleWidthKey = "D_SwitchToggleWidth_";

        /// <summary>
        /// COMPUTED: D_DefaultBorder_ - D_DefaultFocus_
        /// </summary>
        public double OutstandingFocus
        {
            get => GetValue<double>(OutstandingFocusKey);
            private set
            {
                if (!SetAndRaise(OutstandingFocusKey, value))
                    return;

                OutstandingFocusThicknessFull = Modify(new Thickness(1), value);
            }
        }

        /// <summary>
        /// COMPUTED: D_DefaultToggleHeight_ + 2 * D_DefaultBorder_
        /// </summary>
        public double RoundToggleHeight
        {
            get => GetValue<double>(RoundToggleHeightKey);
            private set => SetAndRaise(RoundToggleHeightKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultToggleHeight_ * 2
        /// </summary>
        public double SwitchToggleWidth
        {
            get => GetValue<double>(SwitchToggleWidthKey);
            private set => SetAndRaise(SwitchToggleWidthKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--MARGINS-->

        private const string MarginThicknessFullKey = "D_DefaultMarginThicknessFull_";
        private const string MarginThicknessLeftRightKey = "D_DefaultMarginThicknessLeftRight_";
        private const string MarginThicknessTopBottomKey = "D_DefaultMarginThicknessTopBottom_";
        private const string MarginThicknessRightKey = "D_DefaultMarginThicknessRight_";
        private const string OutstandingFocusThicknessFullKey = "D_OutstandingFocusThicknessFull_";

        /// <summary>
        /// COMPUTED: Thickness(1) * D_DefaultMargin_
        /// </summary>
        public Thickness MarginThicknessFull
        {
            get => GetValue<Thickness>(MarginThicknessFullKey);
            private set => SetAndRaise(MarginThicknessFullKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1,0) * D_DefaultMargin_
        /// </summary>
        public Thickness MarginThicknessLeftRight
        {
            get => GetValue<Thickness>(MarginThicknessLeftRightKey);
            private set => SetAndRaise(MarginThicknessLeftRightKey, value);
        }

        /// <summary>
        /// COMPUTED: TThickness(0,1) * D_DefaultMargin_
        /// </summary>
        public Thickness MarginThicknessTopBottom
        {
            get => GetValue<Thickness>(MarginThicknessTopBottomKey);
            private set => SetAndRaise(MarginThicknessTopBottomKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(0,0,1,0) * D_DefaultMargin_
        /// </summary>
        public Thickness MarginThicknessRight
        {
            get => GetValue<Thickness>(MarginThicknessRightKey);
            private set => SetAndRaise(MarginThicknessRightKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1) * D_OutstandingFocus_
        /// </summary>
        public Thickness OutstandingFocusThicknessFull
        {
            get => GetValue<Thickness>(OutstandingFocusThicknessFullKey);
            private set => SetAndRaise(OutstandingFocusThicknessFullKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--PADDINGS-->

        private const string PaddingThicknessFullKey = "D_DefaultPaddingThicknessFull_";
        private const string PaddingThicknessLeftKey = "D_DefaultPaddingThicknessLeft_";
        private const string PaddingThicknessLeftRightKey = "D_DefaultPaddingThicknessLeftRight_";

        /// <summary>
        /// COMPUTED: Thickness(1) * D_DefaultPadding_
        /// </summary>
        public Thickness PaddingThicknessFull
        {
            get => GetValue<Thickness>(PaddingThicknessFullKey);
            private set => SetAndRaise(PaddingThicknessFullKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1,0,0,0) * D_DefaultPadding_
        /// </summary>
        public Thickness PaddingThicknessLeft
        {
            get => GetValue<Thickness>(PaddingThicknessLeftKey);
            private set => SetAndRaise(PaddingThicknessLeftKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1,0) * D_DefaultPadding_
        /// </summary>
        public Thickness PaddingThicknessLeftRight
        {
            get => GetValue<Thickness>(PaddingThicknessLeftRightKey);
            private set => SetAndRaise(PaddingThicknessLeftRightKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--BORDERS-->

        private const string BorderThicknessFullKey = "D_DefaultBorderThicknessFull_";
        private const string BorderThicknessTopKey = "D_DefaultBorderThicknessTop_";
        private const string BorderThicknessLeftRightKey = "D_DefaultBorderThicknessLeftRight_";
        private const string BorderThicknessLeftTopRightKey = "D_DefaultBorderThicknessLeftTopRight_";
        private const string FocusThicknessFullKey = "D_DefaultFocusThicknessFull_";

        /// <summary>
        /// COMPUTED: Thickness(1) * D_DefaultBorder_
        /// </summary>
        public Thickness BorderThicknessFull
        {
            get => GetValue<Thickness>(BorderThicknessFullKey);
            private set => SetAndRaise(BorderThicknessFullKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(0,1,0,0) * D_DefaultBorder_
        /// </summary>
        public Thickness BorderThicknessTop
        {
            get => GetValue<Thickness>(BorderThicknessTopKey);
            private set => SetAndRaise(BorderThicknessTopKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1,0) * D_DefaultBorder_
        /// </summary>
        public Thickness BorderThicknessLeftRight
        {
            get => GetValue<Thickness>(BorderThicknessLeftRightKey);
            private set => SetAndRaise(BorderThicknessLeftRightKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1,1,1,0) * D_DefaultBorder_
        /// </summary>
        public Thickness BorderThicknessLeftTopRight
        {
            get => GetValue<Thickness>(BorderThicknessLeftTopRightKey);
            private set => SetAndRaise(BorderThicknessLeftTopRightKey, value);
        }

        /// <summary>
        /// COMPUTED: Thickness(1) * D_DefaultFocus_
        /// </summary>
        public Thickness FocusThicknessFull
        {
            get => GetValue<Thickness>(FocusThicknessFullKey);
            private set => SetAndRaise(FocusThicknessFullKey, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Thickness Modify(Thickness input, double modifier)
        {
            return new Thickness
            (
                input.Left * modifier,
                input.Top * modifier,
                input.Right * modifier,
                input.Bottom * modifier
            );
        }
    }
}
