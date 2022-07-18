using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace RedSharp.ClearDesign.Helpers
{
    public static class ContentLayout
    {
        public static readonly AttachedProperty<bool> ContentLayoutRoundingProperty;

        static ContentLayout()
        {
            ContentLayoutRoundingProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("ContentLayoutRounding", typeof(ContentLayout));
        }

        public static void SetContentLayoutRounding(AvaloniaObject element, bool value)
        {
            element.SetValue(ContentLayoutRoundingProperty, value);
        }

        public static bool GetContentLayoutRounding(AvaloniaObject element)
        {
            return element.GetValue(ContentLayoutRoundingProperty);
        }
    }
}
