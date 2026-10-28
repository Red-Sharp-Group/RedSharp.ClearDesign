using System;

namespace RedSharp.ClearDesign.Wpf.Controls.Utils
{
    public class DrawingChangedEventArgs : EventArgs
    {
        public DrawingMetadata Metadata { get; set; }

        public bool IsAnimated { get; set; }
    }
}
