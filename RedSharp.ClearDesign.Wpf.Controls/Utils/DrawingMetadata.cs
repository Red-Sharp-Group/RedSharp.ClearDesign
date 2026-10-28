using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RedSharp.ClearDesign.Wpf.Controls.Enums;
using RedSharp.ClearDesign.Wpf.Controls.Interfaces;

namespace RedSharp.ClearDesign.Wpf.Controls.Utils
{
    /// <summary>
    /// Provides the necessary notification about render, arrange, or measure.
    /// </summary>
    public class DrawingMetadata : FrameworkPropertyMetadata
    {
        private MetadataOptions _options;
        private PropertyChangedCallback _propertyChangedCallback;

        public static event EventHandler<DrawingChangedEventArgs> DrawingPropertyChanged;

        static DrawingMetadata()
        {
            DrawingPropertyChanged += (sender, args) =>
            {
                if (!(sender is IPrimitiveShape shape2) || !args.Metadata.AffectsRender)
                    return;

                InvalidateReasons reasons = InvalidateReasons.PropertyChanged;

                if (args.IsAnimated)
                    reasons |= InvalidateReasons.IsAnimated;

                shape2.InvalidateGeometry(reasons);
            };
        }

        public DrawingMetadata(object defaultValue)
            : this(defaultValue, MetadataOptions.None, null)
        { }

        public DrawingMetadata(PropertyChangedCallback propertyChangedCallback)
            : this(DependencyProperty.UnsetValue, MetadataOptions.None, propertyChangedCallback)
        { }

        public DrawingMetadata(object defaultValue, MetadataOptions options)
            : this(defaultValue, options, null)
        { }

        public DrawingMetadata(object defaultValue, MetadataOptions options, PropertyChangedCallback propertyChangedCallback)
          : base(defaultValue, (FrameworkPropertyMetadataOptions)options, AttachCallback(defaultValue, options, propertyChangedCallback))
        { }

        /// <summary>
        /// This private Ctor should only be used by AttachCallback.
        /// </summary>
        private DrawingMetadata(MetadataOptions options, object defaultValue)
          : base(defaultValue, (FrameworkPropertyMetadataOptions)options)
        { }

        /// <summary>
        /// Chain InternalCallback() to attach the instance of DrawingPropertyMetadata on property callback.
        /// </summary>
        private static PropertyChangedCallback AttachCallback(object defaultValue, MetadataOptions options, PropertyChangedCallback propertyChangedCallback)
        {
            return new PropertyChangedCallback(new DrawingMetadata(options, defaultValue)
            {
                _options = options,
                _propertyChangedCallback = propertyChangedCallback
            }.InternalCallback);
        }

        /// <summary>
        /// Before chaining the original callback, trigger DrawingPropertyChangedEvent.
        /// </summary>
        private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DrawingPropertyChanged != null)
            {
                DrawingPropertyChanged(sender, new DrawingChangedEventArgs()
                {
                    Metadata = this,
                    IsAnimated = DependencyPropertyHelper.GetValueSource(sender, e.Property).IsAnimated
                });
            }

            if (_propertyChangedCallback == null)
                return;

            _propertyChangedCallback(sender, e);
        }
    }
}
