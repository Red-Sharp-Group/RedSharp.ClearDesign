using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using RedSharp.ClearDesign.Wpf.Utils.Models;

namespace RedSharp.ClearDesign.Wpf.Utils.Extensions
{
    [ContentProperty(nameof(Descriptions))]
    public class DoubleAnimationGeneratorExtension : MarkupExtension
    {
        private List<DoubleAnimationDescription> _descriptions;

        public List<DoubleAnimationDescription> Descriptions
        {
            get
            {
                if(_descriptions == null)
                    _descriptions = new List<DoubleAnimationDescription>();

                return _descriptions;
            }
        }

        public bool IsEnter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Descriptions == null || Descriptions.Count == 0)
                return null;

            var beginStoryboard = new BeginStoryboard();
            var storyboard = new Storyboard();

            foreach (var description in Descriptions)
            {
                var animation = new DoubleAnimation()
                {
                    AutoReverse = false,
                    Duration = description.Duration,
                };

                if (!string.IsNullOrEmpty(description.TargetName))
                    Storyboard.SetTargetName(animation, description.TargetName);

                Storyboard.SetTargetProperty(animation, description.TargetProperty);

                if (IsEnter)
                {
                    animation.To = description.To;
                    animation.FillBehavior = FillBehavior.HoldEnd;
                }
                else
                {
                    animation.To = description.From;
                    animation.FillBehavior = FillBehavior.Stop;
                }

                storyboard.Children.Add(animation);
            }

            beginStoryboard.Storyboard = storyboard;

            return beginStoryboard;
        }
    }
}
