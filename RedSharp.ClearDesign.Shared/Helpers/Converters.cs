using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.ClearDesign.Shared.Interfaces;
using RedSharp.ClearDesign.Shared.Utils;

namespace RedSharp.ClearDesign.Shared.Helpers
{
    public static class Converters
    {
        public static IConvertor ObjectIsNullOrEmpty { get; } = Create<Object, bool>(IsObjectIsNullOrEmpty);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DelegateConverter<TInput, TTarget> Create<TInput, TTarget>(Func<TInput, TTarget> conversion)
        {
            return new DelegateConverter<TInput, TTarget>(conversion);
        }

        private static bool IsObjectIsNullOrEmpty(Object input)
        {
            if (input == null)
                return true;

            if (input is String castedString)
                return String.IsNullOrEmpty(castedString);

            if (input is Array castedArray)
                return castedArray.Length == 0;

            if (input is ICollection castedCollection)
                return castedCollection.Count == 0;

            if (input is IEnumerable castedEnumerable)
                return !castedEnumerable.GetEnumerator().MoveNext();

            return false;
        }
    }
}
