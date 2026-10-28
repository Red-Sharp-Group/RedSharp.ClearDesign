using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Models
{
    public class ThicknessAnimationDescription
    {
        public string TargetName { get; set; }

        public PropertyPath TargetProperty { get; set; }

        public Thickness To { get; set; }

        public Thickness From { get; set; }

        public Duration Duration { get; set; }
    }
}
