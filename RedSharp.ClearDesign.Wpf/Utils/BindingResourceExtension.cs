using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RedSharp.ClearDesign.Utils
{
    public class BindingResourceExtension : StaticResourceExtension
    {
        public BindingResourceExtension() : base() 
        { }

        public BindingResourceExtension(object resourceKey) : base(resourceKey) 
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var resource = base.ProvideValue(serviceProvider);

            if (resource == null)
                throw new Exception($"The needed resource: {ResourceKey} - is null.");

            if (!(resource is BindingBase))
                throw new Exception($"The needed resource: {ResourceKey} - is not a binding.");

            return ((BindingBase)resource).ProvideValue(serviceProvider);
        }
    }
}
