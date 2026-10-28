using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using RedSharp.ClearDesign.Wpf.Controls.Helpers;

namespace RedSharp.ClearDesign.Wpf.Controls.Specialized
{
    using Attached = RedSharp.ClearDesign.Wpf.Utils.Attached;

    [ContentProperty("Child")]
    public class AdornedPlaceholder : FrameworkElement, IAddChild
    {
        public static readonly DependencyProperty IsFocusTargetProperty;
        public static readonly DependencyProperty FocusTargetProperty;

        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty ActualCornerRadiusProperty;

        private UIElement _child;
        private Adorner _templatedAdorner;

        static AdornedPlaceholder()
        {
            IsFocusTargetProperty = DependencyProperty.RegisterAttached("IsFocusTarget", typeof(bool), typeof(AdornedPlaceholder), new PropertyMetadata(IsFocusTargetPropertyChangedCallback));
            FocusTargetProperty = DependencyProperty.RegisterAttached("FocusTarget", typeof(UIElement), typeof(AdornedPlaceholder));

            CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(AdornedPlaceholder),
                new FrameworkPropertyMetadata(new CornerRadius()), new ValidateValueCallback(IsCornerRadiusValid));

            ActualCornerRadiusProperty = DependencyProperty.Register(nameof(ActualCornerRadius), typeof(CornerRadius), typeof(AdornedPlaceholder));
        }

        public static void SetIsFocusTarget(DependencyObject element, bool value)
        {
            element.SetValue(IsFocusTargetProperty, value);
        }

        public static bool GetIsFocusTarget(DependencyObject element)
        {
            return (bool)element.GetValue(IsFocusTargetProperty);
        }

        public static void SetFocusTarget(DependencyObject element, UIElement value)
        {
            element.SetValue(FocusTargetProperty, value);
        }

        public static UIElement GetFocusTarget(DependencyObject element)
        {
            return (FrameworkElement)element.GetValue(FocusTargetProperty);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public CornerRadius ActualCornerRadius
        {
            get => (CornerRadius)GetValue(ActualCornerRadiusProperty);
            private set => SetValue(ActualCornerRadiusProperty, value);
        }

        public UIElement AdornedElement
        {
            get
            {
                if (Adorner != null)
                    return Adorner.AdornedElement;
                else
                    return null;
            }
        }

        public UIElement FocusTarget
        {
            get
            {
                if (AdornedElement != null)
                    return GetFocusTarget(AdornedElement);
                else
                    return null;
            }
        }

        [DefaultValue(null)]
        public virtual UIElement Child
        {
            get
            {
                return _child;
            }

            set
            {
                UIElement old = _child;

                if (old != value)
                {
                    RemoveVisualChild(old);
                    //need to remove old element from logical tree
                    RemoveLogicalChild(old);
                    _child = value;

                    AddVisualChild(_child);
                    AddLogicalChild(value);

                    InvalidateMeasure();
                }
            }
        }

        void IAddChild.AddChild(object value)
        {
            // keeping consistent with other elements:  adding null is a no-op.
            if (value == null)
                return;

            if (!(value is UIElement))
                throw new ArgumentException("Incompatible child type, has to be at least UIElement.");

            if (Child != null)
                throw new ArgumentException("Already has a child.");

            Child = (UIElement)value;
        }

        void IAddChild.AddText(string text)
        {
            throw new NotSupportedException();
        }

        private static bool IsCornerRadiusValid(object value)
        {
            var radius = (CornerRadius)value;

            return radius.IsValid(false, false, false, false);
        }

        private static void IsFocusTargetPropertyChangedCallback(DependencyObject item, DependencyPropertyChangedEventArgs arguments)
        {
            if (!(bool)arguments.NewValue)
                return;

            var element = (FrameworkElement)item;

            if (element == null)
                return;

            var templateParent = (FrameworkElement)element.TemplatedParent;

            if (templateParent == null)
                throw new InvalidOperationException($"{nameof(IsFocusTargetProperty)} has to be used only in templates.");

            SetFocusTarget(templateParent, element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetBinding(FrameworkElement owner, DependencyProperty ownerProperty, object target, DependencyProperty targetProperty)
        {
            var binding = new Binding();

            binding.Path = new PropertyPath(targetProperty);
            binding.Mode = BindingMode.OneWay;
            binding.Source = target;

            owner.SetBinding(ownerProperty, binding);
        }

        private void SetActualCornerRadius(FrameworkElement element)
        {
            if (element == null)
            {
                ActualCornerRadius = CornerRadius;
            }
            else if (element is Border border)
            {
                SetBinding(this, ActualCornerRadiusProperty, border, Border.CornerRadiusProperty);
            }
            else if (element is LayeredBorder layeredBorder)
            {
                SetBinding(this, ActualCornerRadiusProperty, layeredBorder, LayeredBorder.ActualCornerRadiusProperty);
            }
            else if(element.HasDependencyProperty(Attached.Control.CornerRadiusProperty))
            {
                SetBinding(this, ActualCornerRadiusProperty, element, Attached.Control.CornerRadiusProperty);
            }
            else
            {
                ActualCornerRadius = CornerRadius;
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _child == null ? 0 : 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_child == null || index != 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _child;
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (TemplatedParent == null)
                throw new InvalidOperationException("Adorned placeholder must be in template.");

            base.OnInitialized(e);

            if (Adorner != null)
            {
                //The most important action for this control
                Adorner.IsClipEnabled = false;

                var adornerElement = (FrameworkElement)AdornedElement;
                var focusTarget = (FrameworkElement)FocusTarget;

                if (focusTarget != null)
                {
                    SetBinding(this, WidthProperty, focusTarget, ActualWidthProperty);
                    SetBinding(this, HeightProperty, focusTarget, ActualHeightProperty);

                    HorizontalAlignment = HorizontalAlignment.Left;
                    VerticalAlignment = VerticalAlignment.Top;

                    var translatedPoint = focusTarget.TranslatePoint(new Point(), Adorner.AdornedElement);

                    Margin = new Thickness(translatedPoint.X, translatedPoint.Y, 0, 0);

                    SetActualCornerRadius(focusTarget);
                }
                else if (adornerElement != null)
                {
                    SetBinding(this, WidthProperty, adornerElement, ActualWidthProperty);
                    SetBinding(this, HeightProperty, adornerElement, ActualHeightProperty);

                    SetActualCornerRadius(adornerElement);
                }
                else
                {
                    SetActualCornerRadius(null);
                }
            }
            else
            {
                SetActualCornerRadius(null);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (TemplatedParent == null)
                throw new InvalidOperationException("Adorned placeholder must be in template.");

            var desiredSize = new Size(0, 0);

            if (AdornedElement == null)
                return desiredSize;

            if (FocusTarget != null)
                desiredSize = FocusTarget.RenderSize;
            else
                desiredSize = AdornedElement.RenderSize;

            if (_child != null)
                _child.Measure(desiredSize);

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (_child != null)
                _child.Arrange(new Rect(arrangeBounds));

            return arrangeBounds;
        }

        private Adorner Adorner
        {
            get
            {
                if (_templatedAdorner == null)
                {
                    // find the TemplatedAdorner
                    var templateParent = TemplatedParent as FrameworkElement;

                    if (templateParent != null)
                        _templatedAdorner = VisualTreeHelper.GetParent(templateParent) as Adorner;
                }

                return _templatedAdorner;
            }
        }
    }
}
