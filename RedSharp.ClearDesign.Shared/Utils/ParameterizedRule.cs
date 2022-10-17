using System.Collections.Generic;

namespace RedSharp.ClearDesign.Shared.Utils
{
    public class ParameterizedRule : ModificationRule
    {
        public ParameterizedRule()
        {
            Parameters = new List<object>();
        }

        public List<object> Parameters { get; set; }

        protected override object InternalModify(IList<object> values)
        {
            if (Parameters != null && Parameters.Count > 0)
            {
                foreach (var item in Parameters)
                    values.Add(item);
            }

            return Converter.ForwardConversion(values);
        }
    }
}
