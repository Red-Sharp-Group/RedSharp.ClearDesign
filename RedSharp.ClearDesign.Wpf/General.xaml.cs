using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RedSharp.ClearDesign.Utils;

namespace RedSharp.ClearDesign
{
    public partial class General : ResourceDictionary
    {
        public General()
        {
            ResourceSchema.InitializeApplicationResources();
        }
    }
}
