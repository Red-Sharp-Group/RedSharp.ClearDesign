using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RedSharp.ClearDesign.Wpf.Abstracts;

namespace RedSharp.ClearDesign.Utils
{
    public class DelegateModifier : MultiInputModifierBase
    {
        public Func<IList<Object>, Object> Delegate { get; set; }

        protected override object InternalModify(IList<Object> value)
        {
            if (Delegate == null)
                throw new Exception("Object is not fully initialized.");

            return Delegate.Invoke(value);
        }
    }
}
