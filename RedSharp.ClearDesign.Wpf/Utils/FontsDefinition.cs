using System;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Abstractions;

namespace RedSharp.ClearDesign.Utils
{
    public class FontsDefinition : DefinitionBase
    {
        public FontsDefinition(ResourceDictionary resources) : base(resources)
        { }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--TITLE1-->

        private const String Title1FontSizeKey = "D_DefaultTitle1FontSize_";
        private const String Title1FontWeightKey = "D_DefaultTitle1FontWeight_";
        private const String Title1FontFamilyKey = "D_DefaultTitle1FontFamily_";

        public double Title1FontSize
        {
            get => GetValue<double>(Title1FontSizeKey);
            set => SetAndRaise(Title1FontSizeKey, value);
        }

        public FontWeight Title1FontWeight
        {
            get => GetValue<FontWeight>(Title1FontWeightKey);
            set => SetAndRaise(Title1FontWeightKey, value);
        }

        public FontFamily Title1FontFamily
        {
            get => GetValue<FontFamily>(Title1FontFamilyKey);
            set => SetAndRaise(Title1FontFamilyKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--TITLE2-->

        private const String Title2FontSizeKey = "D_DefaultTitle2FontSize_";
        private const String Title2FontWeightKey = "D_DefaultTitle2FontWeight_";
        private const String Title2FontFamilyKey = "D_DefaultTitle2FontFamily_";

        public double Title2FontSize
        {
            get => GetValue<double>(Title2FontSizeKey);
            set => SetAndRaise(Title2FontSizeKey, value);
        }

        public FontWeight Title2FontWeight
        {
            get => GetValue<FontWeight>(Title2FontWeightKey);
            set => SetAndRaise(Title2FontWeightKey, value);
        }

        public FontFamily Title2FontFamily
        {
            get => GetValue<FontFamily>(Title2FontFamilyKey);
            set => SetAndRaise(Title2FontFamilyKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--TITLE3-->

        private const String Title3FontSizeKey = "D_DefaultTitle3FontSize_";
        private const String Title3FontWeightKey = "D_DefaultTitle3FontWeight_";
        private const String Title3FontFamilyKey = "D_DefaultTitle3FontFamily_";

        public double Title3FontSize
        {
            get => GetValue<double>(Title3FontSizeKey);
            set => SetAndRaise(Title3FontSizeKey, value);
        }

        public FontWeight Title3FontWeight
        {
            get => GetValue<FontWeight>(Title2FontWeightKey);
            set => SetAndRaise(Title2FontWeightKey, value);
        }

        public FontFamily Title3FontFamily
        {
            get => GetValue<FontFamily>(Title3FontFamilyKey);
            set => SetAndRaise(Title3FontFamilyKey, value);
        }

        //<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
        //<!--COMMON-->

        private const String CommonFontSizeKey = "D_DefaultCommonFontSize_";
        private const String CommonFontWeightKey = "D_DefaultCommonFontWeight_";
        private const String CommonFontFamilyKey = "D_DefaultCommonFontFamily_";

        public double CommonFontSize
        {
            get => GetValue<double>(CommonFontSizeKey);
            set => SetAndRaise(CommonFontSizeKey, value);
        }

        public FontWeight CommonFontWeight
        {
            get => GetValue<FontWeight>(Title2FontWeightKey);
            set => SetAndRaise(Title2FontWeightKey, value);
        }

        public FontFamily CommonFontFamily
        {
            get => GetValue<FontFamily>(Title3FontFamilyKey);
            set => SetAndRaise(Title3FontFamilyKey, value);
        }
    }
}
