using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Utilities;

namespace RedSharp.ClearDesign.Helpers
{
    public static class RenderHelper
    {
        public static void AffectsRender<T>(params AvaloniaProperty[] properties) where T : Visual
        {
            static void Invalidate(AvaloniaPropertyChangedEventArgs e)
            {
                if (e.Sender is T sender)
                {
                    sender.InvalidateVisual();
                }
            }

            static void InvalidateAndSubscribe(AvaloniaPropertyChangedEventArgs e)
            {
                if (e.Sender is T sender)
                {
                    var affectsRenderInvalidatedDelegate = Delegate.CreateDelegate(typeof(EventHandler<EventArgs>), sender, "AffectsRenderInvalidated") as EventHandler<EventArgs>;

                    if (e.OldValue is IAffectsRender oldValue)
                    {
                        WeakEventHandlerManager.Unsubscribe<EventArgs, T>(oldValue, nameof(oldValue.Invalidated), affectsRenderInvalidatedDelegate);
                    }

                    if (e.NewValue is IAffectsRender newValue)
                    {
                        WeakEventHandlerManager.Subscribe<IAffectsRender, EventArgs, T>(newValue, nameof(newValue.Invalidated), affectsRenderInvalidatedDelegate);
                    }

                    sender.InvalidateVisual();
                }
            }

            foreach (var property in properties)
            {
                if (property.CanValueAffectRender())
                {
                    property.Changed.Subscribe(e => InvalidateAndSubscribe(e));
                }
                else
                {
                    property.Changed.Subscribe(e => Invalidate(e));
                }
            }
        }
    }

    public static class TogglesHelper
    {
        public static readonly AttachedProperty<IBrush> FalseIconColorProperty;
        public static readonly AttachedProperty<IBrush> NullIconColorProperty;
        public static readonly AttachedProperty<IBrush> TrueIconColorProperty;

        public static readonly AttachedProperty<bool> FalseIconVisibilityProperty;
        public static readonly AttachedProperty<bool> NullIconVisibilityProperty;
        public static readonly AttachedProperty<bool> TrueIconVisibilityProperty;

        static TogglesHelper()
        {
            FalseIconColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(FalseIconColorProperty)), typeof(TogglesHelper));
            NullIconColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(NullIconColorProperty)), typeof(TogglesHelper));
            TrueIconColorProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, IBrush>(ContentHelper.MakePropertyName(nameof(TrueIconColorProperty)), typeof(TogglesHelper));

            FalseIconVisibilityProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>(ContentHelper.MakePropertyName(nameof(FalseIconVisibilityProperty)), typeof(TogglesHelper));
            NullIconVisibilityProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>(ContentHelper.MakePropertyName(nameof(NullIconVisibilityProperty)), typeof(TogglesHelper));
            TrueIconVisibilityProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>(ContentHelper.MakePropertyName(nameof(TrueIconVisibilityProperty)), typeof(TogglesHelper));

            //RenderHelper.AffectsRender<Visual>(FalseIconColorProperty, NullIconColorProperty, TrueIconColorProperty);
        }

        static void Invalidate(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is Visual sender)
            {
                sender.InvalidateVisual();
            }
        }

        public static void SetFalseIconColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(FalseIconColorProperty, value);
        }

        public static IBrush GetFalseIconColor(AvaloniaObject element)
        {
            return element.GetValue(FalseIconColorProperty);
        }

        public static void SetNullIconColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(NullIconColorProperty, value);
        }

        public static IBrush GetNullIconColor(AvaloniaObject element)
        {
            return element.GetValue(NullIconColorProperty);
        }

        public static void SetTrueIconColor(AvaloniaObject element, IBrush value)
        {
            element.SetValue(TrueIconColorProperty, value);
        }

        public static IBrush GetTrueIconColor(AvaloniaObject element)
        {
            return element.GetValue(TrueIconColorProperty);
        }

        public static void SetFalseIconVisibility(AvaloniaObject element, bool value)
        {
            element.SetValue(FalseIconVisibilityProperty, value);
        }

        public static bool GetFalseIconVisibility(AvaloniaObject element)
        {
            return element.GetValue(FalseIconVisibilityProperty);
        }

        public static void SetNullIconVisibility(AvaloniaObject element, bool value)
        {
            element.SetValue(NullIconVisibilityProperty, value);
        }

        public static bool GetNullIconVisibility(AvaloniaObject element)
        {
            return element.GetValue(NullIconVisibilityProperty);
        }

        public static void SetTrueIconVisibility(AvaloniaObject element, bool value)
        {
            element.SetValue(TrueIconVisibilityProperty, value);
        }

        public static bool GetTrueIconVisibility(AvaloniaObject element)
        {
            return element.GetValue(TrueIconVisibilityProperty);
        }
    }
}
