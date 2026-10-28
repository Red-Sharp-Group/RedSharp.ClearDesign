using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Utils.Models
{
    public class DoubleAnimationDescription
    {
        public string TargetName { get; set; }

        public PropertyPath TargetProperty { get; set; }

        public double To { get; set; }

        public double From { get; set; }

        public Duration Duration { get; set; }
    }
}
