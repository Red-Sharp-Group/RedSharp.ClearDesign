using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RedSharp.ClearDesign.Helpers;
using RedSharp.ClearDesign.Utils;

namespace RedSharp.ClearDesign.Definitions
{
    public partial class Colors : ResourceDictionary
    {
        public Colors()
        {
            if (!ResourceSchema.ApplicationResources.IsFilledFrom(this))
            {
                InitializeComponent();

                ResourceSchema.ApplicationResources.FillFrom(this);
            }
        }
    }
}
