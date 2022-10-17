using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.ClearDesign.Shared.Interfaces
{
    public interface IResourceModifier
    {
        public Object ResultKey { get; }

        public IEnumerable<Object> InputKeys { get; }

        public Object Modify(IList<Object> values);
    }
}
