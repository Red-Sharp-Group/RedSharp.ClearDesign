using System;
using System.Collections.Generic;
using RedSharp.ClearDesign.Shared.Abstracts;
using RedSharp.ClearDesign.Shared.Interfaces;

namespace RedSharp.ClearDesign.Shared.Utils
{
    public class ModificationRule : MultiInputModifierBase
    {
        public IConvertor Converter { get; set; }

        protected override object InternalModify(IList<object> values)
        {
            if (Converter == null)
                throw new Exception("Converter wasn't initialized.");

            return Converter.ForwardConversion(values);
        }
    }
}
