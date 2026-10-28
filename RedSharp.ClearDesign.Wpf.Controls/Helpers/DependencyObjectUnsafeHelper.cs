using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    public static class DependencyObjectUnsafeHelper
    {
        private static readonly MethodInfo _lookupEntryMethod;
        private static readonly PropertyInfo _entryIndexFound;

        static DependencyObjectUnsafeHelper()
        {
            var objectTypeinfo = typeof(DependencyObject).GetTypeInfo();

            _lookupEntryMethod = objectTypeinfo.DeclaredMethods.First(item => item.Name.StartsWith("LookupEntry"));

            var entryIndexTypeInfo = objectTypeinfo.Assembly.DefinedTypes.Where(item => item.Name.StartsWith("EntryIndex"))
                                                                         .First(item => item.FullName.StartsWith("System.Windows.EntryIndex"));

            _entryIndexFound = entryIndexTypeInfo.DeclaredProperties.First(item => item.Name.StartsWith("Found"));
        }

        public static bool HasDependencyProperty(this DependencyObject dependencyObject, DependencyProperty property)
        {
            var entry = _lookupEntryMethod.Invoke(dependencyObject, new object[] { property.GlobalIndex });

            var result = _entryIndexFound.GetValue(entry);

            return (bool)result;
        }
    }
}
