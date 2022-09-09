using System;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;
using RedSharp.ClearDesign.Helpers;

namespace RedSharp.ClearDesign.Utils
{
    public class ColorsDefinition : DefinitionBase
    {
        public ColorsDefinition(ResourceDictionary resources) : base(resources)
        { }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--MAIN COLOR-->

        private const string PrimaryColorKey = "D_DefaultPrimaryColor_";
        private const string SecondaryColorKey = "D_DefaultSecondaryColor_";

        private const string BackgroundColorKey = "D_DefaultBackgroundColor_";
        private const string FieldsColorKey = "D_DefaultFieldsColor_";
        private const string ToggleColorKey = "D_DefaultToggleColor_";

        private const string PrimaryFontColorKey = "D_DefaultPrimaryFontColor_";
        private const string SecondaryFontColorKey = "D_DefaultSecondaryFontColor_";

        private const string ErrorColorKey = "D_DefaultErrorColor_";
        private const string WarningColorKey = "D_DefaultWarningColor_";
        private const string SuccessColorKey = "D_DefaultSuccessColor_";

        private const string LightAccentColorKey = "D_DefaultLightAccentColor_";
        private const string DarkAccentColorKey = "D_DefaultDarkAccentColor_";
        private const string DisabledAccentColorKey = "D_DefaultDisabledAccentColor_";
        private const string StepAccentColorKey = "D_DefaultStepAccentColor_";

        private const string DisabledStateMaskColorKey = "D_DisabledStateMaskColor_";

        public Color PrimaryColor
        {
            get => GetValue<Color>(PrimaryColorKey);
            set
            {
                if (!SetAndRaise(PrimaryColorKey, value))
                    return;

                FocusBorderColor = ColorHelper.AlphaBlending(value, Light5Color);
                PrimaryBorderColor = ColorHelper.AlphaBlending(value, Dark5Color);
            }
        }

        public Color SecondaryColor
        {
            get => GetValue<Color>(SecondaryColorKey);
            set => SetAndRaise(SecondaryColorKey, value);
        }


        public Color BackgroundColor
        {
            get => GetValue<Color>(BackgroundColorKey);
            set => SetAndRaise(BackgroundColorKey, value);
        }

        public Color FieldsColor
        {
            get => GetValue<Color>(FieldsColorKey);
            set => SetAndRaise(FieldsColorKey, value);
        }

        public Color ToggleColor
        {
            get => GetValue<Color>(ToggleColorKey);
            set => SetAndRaise(ToggleColorKey, value);
        }


        public Color PrimaryFontColor
        {
            get => GetValue<Color>(PrimaryFontColorKey);
            set => SetAndRaise(PrimaryFontColorKey, value);
        }

        public Color SecondaryFontColor
        {
            get => GetValue<Color>(SecondaryFontColorKey);
            set
            {
                if (!SetAndRaise(SecondaryFontColorKey, value))
                    return;

                CommonFontColor = ColorHelper.AlphaBlending(value, Light3Color);
            }
        }


        public Color ErrorColor
        {
            get => GetValue<Color>(ErrorColorKey);
            set => SetAndRaise(ErrorColorKey, value);
        }

        public Color WarningColor
        {
            get => GetValue<Color>(WarningColorKey);
            set => SetAndRaise(WarningColorKey, value);
        }

        public Color SuccessColor
        {
            get => GetValue<Color>(SuccessColorKey);
            set => SetAndRaise(SuccessColorKey, value);
        }


        public Color LightAccentColor
        {
            get => GetValue<Color>(LightAccentColorKey);
            set
            {
                if (!SetAndRaise(LightAccentColorKey, value))
                    return;

                Light1Color = ColorHelper.ChangeOpacity(value, 0.05);
                Light2Color = ColorHelper.ChangeOpacity(value, 0.10);
                Light3Color = ColorHelper.ChangeOpacity(value, 0.15);
                Light4Color = ColorHelper.ChangeOpacity(value, 0.20);
                Light5Color = ColorHelper.ChangeOpacity(value, 0.25);
            }
        }

        public Color DarkAccentColor
        {
            get => GetValue<Color>(DarkAccentColorKey);
            set
            {
                if (!SetAndRaise(DarkAccentColorKey, value))
                    return;

                Dark1Color = ColorHelper.ChangeOpacity(value, 0.05);
                Dark2Color = ColorHelper.ChangeOpacity(value, 0.10);
                Dark3Color = ColorHelper.ChangeOpacity(value, 0.15);
                Dark4Color = ColorHelper.ChangeOpacity(value, 0.20);
                Dark5Color = ColorHelper.ChangeOpacity(value, 0.25);
            }
        }

        public Color DisabledAccentColor
        {
            get => GetValue<Color>(DisabledAccentColorKey);
            set
            {
                if (!SetAndRaise(DisabledAccentColorKey, value))
                    return;

                Disabled1Color = ColorHelper.ChangeOpacity(value, 0.10);
                Disabled2Color = ColorHelper.ChangeOpacity(value, 0.25);
                Disabled3Color = ColorHelper.ChangeOpacity(value, 0.40);
                Disabled4Color = ColorHelper.ChangeOpacity(value, 0.55);
                Disabled5Color = ColorHelper.ChangeOpacity(value, 0.70);
            }
        }

        public Color StepAccentColor
        {
            get => GetValue<Color>(StepAccentColorKey);
            set
            {
                if (!SetAndRaise(StepAccentColorKey, value))
                    return;

                Step1Color = ColorHelper.ChangeOpacity(value, 0.03);
                Step2Color = ColorHelper.ChangeOpacity(value, 0.06);
                Step3Color = ColorHelper.ChangeOpacity(value, 0.09);
                Step4Color = ColorHelper.ChangeOpacity(value, 0.12);
                Step5Color = ColorHelper.ChangeOpacity(value, 0.15);
            }
        }


        public Color DisabledStateMaskColor
        {
            get => GetValue<Color>(DisabledStateMaskColorKey);
            set => SetAndRaise(DisabledStateMaskColorKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--LIGHT ACCENT COLOR-->

        private const string Light1ColorKey = "D_Light1Color_";
        private const string Light2ColorKey = "D_Light2Color_";
        private const string Light3ColorKey = "D_Light3Color_";
        private const string Light4ColorKey = "D_Light4Color_";
        private const string Light5ColorKey = "D_Light5Color_";

        /// <summary>
        /// COMPUTED: D_DefaultLightAccentColor_(Alpha 5%)
        /// </summary>
        public Color Light1Color
        {
            get => GetValue<Color>(Light1ColorKey);
            private set => SetAndRaise(Light1ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultLightAccentColor_(Alpha 10%)
        /// </summary>
        public Color Light2Color
        {
            get => GetValue<Color>(Light2ColorKey);
            private set => SetAndRaise(Light2ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultLightAccentColor_(Alpha 15%)
        /// </summary>
        public Color Light3Color
        {
            get => GetValue<Color>(Light3ColorKey);
            private set
            {
                if (!SetAndRaise(Light3ColorKey, value))
                    return;

                CommonFontColor = ColorHelper.AlphaBlending(SecondaryFontColor, value);
            }
        }

        /// <summary>
        /// COMPUTED: D_DefaultLightAccentColor_(Alpha 20%)
        /// </summary>
        public Color Light4Color
        {
            get => GetValue<Color>(Light4ColorKey);
            private set => SetAndRaise(Light4ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultLightAccentColor_(Alpha 25%)
        /// </summary>
        public Color Light5Color
        {
            get => GetValue<Color>(Light5ColorKey);
            private set
            {
                if (!SetAndRaise(Light5ColorKey, value))
                    return;

                FocusBorderColor = ColorHelper.AlphaBlending(PrimaryColor, value);
            }
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--DARK ACCENT COLOR-->

        private const string Dark1ColorKey = "D_Dark1Color_";
        private const string Dark2ColorKey = "D_Dark2Color_";
        private const string Dark3ColorKey = "D_Dark3Color_";
        private const string Dark4ColorKey = "D_Dark4Color_";
        private const string Dark5ColorKey = "D_Dark5Color_";

        /// <summary>
        /// COMPUTED: D_DefaultDarkAccentColor_(Alpha 5%)
        /// </summary>
        public Color Dark1Color
        {
            get => GetValue<Color>(Dark1ColorKey);
            private set => SetAndRaise(Dark1ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDarkAccentColor_(Alpha 10%)
        /// </summary>
        public Color Dark2Color
        {
            get => GetValue<Color>(Dark2ColorKey);
            private set => SetAndRaise(Dark2ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDarkAccentColor_(Alpha 15%)
        /// </summary>
        public Color Dark3Color
        {
            get => GetValue<Color>(Dark3ColorKey);
            private set => SetAndRaise(Dark3ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDarkAccentColor_(Alpha 20%)
        /// </summary>
        public Color Dark4Color
        {
            get => GetValue<Color>(Dark4ColorKey);
            private set
            {
                if (!SetAndRaise(Dark4ColorKey, value))
                    return;

                DisabledBorderColor = ColorHelper.AlphaBlending(Disabled3Color, value);
            }
        }

        /// <summary>
        /// COMPUTED: D_DefaultDarkAccentColor_(Alpha 25%)
        /// </summary>
        public Color Dark5Color
        {
            get => GetValue<Color>(Dark5ColorKey);
            private set
            {
                if (!SetAndRaise(Dark5ColorKey, value))
                    return;

                ScrollThumbBackgroundColor = ColorHelper.AlphaBlending(value, value);
                PrimaryBorderColor = ColorHelper.AlphaBlending(PrimaryColor, value);
                SecondaryBorderColor = value;
            }
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--DISABLED ACCENT COLOR-->

        private const string Disabled1ColorKey = "D_Disabled1Color_";
        private const string Disabled2ColorKey = "D_Disabled2Color_";
        private const string Disabled3ColorKey = "D_Disabled3Color_";
        private const string Disabled4ColorKey = "D_Disabled4Color_";
        private const string Disabled5ColorKey = "D_Disabled5Color_";

        /// <summary>
        /// COMPUTED: D_DefaultDisabledAccentColor_(Alpha 10%)
        /// </summary>
        public Color Disabled1Color
        {
            get => GetValue<Color>(Disabled1ColorKey);
            private set => SetAndRaise(Disabled1ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDisabledAccentColor_(Alpha 25%)
        /// </summary>
        public Color Disabled2Color
        {
            get => GetValue<Color>(Disabled2ColorKey);
            private set => SetAndRaise(Disabled2ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDisabledAccentColor_(Alpha 40%)
        /// </summary>
        public Color Disabled3Color
        {
            get => GetValue<Color>(Disabled3ColorKey);
            private set
            {
                if (!SetAndRaise(Disabled3ColorKey, value))
                    return;

                DisabledBorderColor = ColorHelper.AlphaBlending(value, Dark4Color);
            }
        }

        /// <summary>
        /// COMPUTED: D_DefaultDisabledAccentColor_(Alpha 55%)
        /// </summary>
        public Color Disabled4Color
        {
            get => GetValue<Color>(Disabled4ColorKey);
            private set => SetAndRaise(Disabled4ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultDisabledAccentColor_(Alpha 70%)
        /// </summary>
        public Color Disabled5Color
        {
            get => GetValue<Color>(Disabled5ColorKey);
            private set => SetAndRaise(Disabled5ColorKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--STEP ACCENT COLOR-->

        private const string Step1ColorKey = "D_Step1Color_";
        private const string Step2ColorKey = "D_Step2Color_";
        private const string Step3ColorKey = "D_Step3Color_";
        private const string Step4ColorKey = "D_Step4Color_";
        private const string Step5ColorKey = "D_Step5Color_";

        /// <summary>
        /// COMPUTED: D_DefaultStepAccentColor_(Alpha 3%)
        /// </summary>
        public Color Step1Color
        {
            get => GetValue<Color>(Step1ColorKey);
            private set => SetAndRaise(Step1ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultStepAccentColor_(Alpha 6%)
        /// </summary>
        public Color Step2Color
        {
            get => GetValue<Color>(Step2ColorKey);
            private set => SetAndRaise(Step2ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultStepAccentColor_(Alpha 9%)
        /// </summary>
        public Color Step3Color
        {
            get => GetValue<Color>(Step3ColorKey);
            private set => SetAndRaise(Step3ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultStepAccentColor_(Alpha 12%)
        /// </summary>
        public Color Step4Color
        {
            get => GetValue<Color>(Step4ColorKey);
            private set => SetAndRaise(Step4ColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultStepAccentColor_(Alpha 15%)
        /// </summary>
        public Color Step5Color
        {
            get => GetValue<Color>(Step5ColorKey);
            private set => SetAndRaise(Step5ColorKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--SPECIAL COLORS-->

        private const string ScrollThumbBackgroundColorKey = "D_ScrollThumbBackgroundColor_";

        /// <summary>
        /// COMPUTED: D_Dark5Color_ + D_Dark5Color_
        /// </summary>
        public Color ScrollThumbBackgroundColor
        {
            get => GetValue<Color>(ScrollThumbBackgroundColorKey);
            private set => SetAndRaise(ScrollThumbBackgroundColorKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--BORDER COLORS-->

        private const string FocusBorderColorKey = "D_FocusBorderColor_";
        private const string PrimaryBorderColorKey = "D_PrimaryBorderColor_";
        private const string SecondaryBorderColorKey = "D_SecondaryBorderColor_";
        private const string DisabledBorderColorKey = "D_DisabledBorderColor_";
        private const string DisabledBorderNoOpacityColorKey = "D_DisabledBorderNoOpacityColor_";

        /// <summary>
        /// COMPUTED: D_DefaultPrimaryColor_ + D_Light5Color_
        /// </summary>
        public Color FocusBorderColor
        {
            get => GetValue<Color>(FocusBorderColorKey);
            private set => SetAndRaise(FocusBorderColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_DefaultPrimaryColor_ + D_Dark5Color_
        /// </summary>
        public Color PrimaryBorderColor
        {
            get => GetValue<Color>(PrimaryBorderColorKey);
            private set => SetAndRaise(PrimaryBorderColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_Dark5Color_
        /// </summary>
        public Color SecondaryBorderColor
        {
            get => GetValue<Color>(SecondaryBorderColorKey);
            private set => SetAndRaise(SecondaryBorderColorKey, value);
        }

        /// <summary>
        /// COMPUTED: D_Disabled3Color_ + D_Dark4Color_
        /// </summary>
        public Color DisabledBorderColor
        {
            get => GetValue<Color>(DisabledBorderColorKey);
            private set
            {
                if (!SetAndRaise(DisabledBorderColorKey, value))
                    return;

                DisabledBorderNoOpacityColor = ColorHelper.ChangeOpacity(value, 1.0);
            }
        }

        /// <summary>
        /// COMPUTED: D_PrimaryBorderColor_(Alpha 100%)
        /// </summary>
        public Color DisabledBorderNoOpacityColor
        {
            get => GetValue<Color>(DisabledBorderNoOpacityColorKey);
            private set => SetAndRaise(DisabledBorderNoOpacityColorKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--FONT COLORS-->

        private const string CommonFontColorKey = "D_CommonFontColor_";

        /// <summary>
        /// COMPUTED: D_DefaultSecondaryFontColor_ + D_Light3Color_
        /// </summary>
        public Color CommonFontColor
        {
            get => GetValue<Color>(CommonFontColorKey);
            private set => SetAndRaise(CommonFontColorKey, value);
        }
    }
}
