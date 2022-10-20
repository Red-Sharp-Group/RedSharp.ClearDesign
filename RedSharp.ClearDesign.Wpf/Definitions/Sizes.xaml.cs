using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSharp.ClearDesign.Definitions
{
    public partial class Sizes : ResourceDictionary
    {
        public const string HeightKey = "D_DefaultHeight_";
        public const string WidthKey = "D_DefaultWidth_";

        public const string ToggleHeightKey = "D_DefaultToggleHeight_";

        public const string MarginKey = "D_DefaultMargin_";
        public const string PaddingKey = "D_DefaultPadding_";

        public const string BorderKey = "D_DefaultBorder_";
        public const string FocusKey = "D_DefaultFocus_";

        public static Func<IList<Object>, Object> RoundToggleHeight => RoundToggleHeightModifier;

        public static Func<IList<Object>, Object> SwitchToggleWidth => SwitchToggleWidthModifier;

        public static Func<IList<Object>, Object> OutstandingFocus => OutstandingFocusModifier;

        private static Object RoundToggleHeightModifier(IList<Object> values)
        {
            if (values[0] == null || values[1] == null)
                return 0.0;

            var toggleHeight = (double)values[0];
            var border = (double)values[1];

            return toggleHeight + 2 * border;
        }

        private static Object SwitchToggleWidthModifier(IList<Object> values)
        {
            if (values[0] == null)
                return 0.0;

            var toggleHeight = (double)values[0];

            return toggleHeight * 2;
        }

        private static Object OutstandingFocusModifier(IList<Object> values)
        {
            if (values[0] == null || values[1] == null)
                return 0.0;

            var border = (double)values[0];
            var focus = (double)values[1];

            return border - focus;
        }
    }
}
