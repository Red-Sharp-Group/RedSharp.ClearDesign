using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.ClearDesign.Wpf.Controls.Enums;

namespace RedSharp.ClearDesign.Wpf.Controls.Interfaces
{
    public interface IArcGeometrySourceParameters : IGeometrySourceParameters
    {
        double StartAngle { get; }

        double EndAngle { get; }

        double ArcThickness { get; }

        UnitType ArcThicknessUnit { get; }
    }
}
