using System.Windows.Media;

namespace RedSharp.ClearDesign.Wpf.Controls.Interfaces
{
    /// <summary>
    /// Provides an interface to describe the parameters of a Shape.
    /// </summary>
    public interface IGeometrySourceParameters
    {
        Stretch Stretch { get; }

        Brush Stroke { get; }

        double StrokeThickness { get; }
    }
}
