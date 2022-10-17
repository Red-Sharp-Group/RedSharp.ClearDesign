using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RedSharp.ClearDesign.Shared.Interfaces;
using RedSharp.ClearDesign.Shared.Utils;

namespace RedSharp.ClearDesign.Helpers
{
    public static class Modifiers
    {
        public static IConvertor ColorBlending { get; } = Create<IList<Object>, Object>(BlendColors);

        public static IConvertor ColorOpacity { get; } = Create<IList<Object>, Object>(ChangeOpacity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DelegateConverter<TInput, TTarget> Create<TInput, TTarget>(Func<TInput, TTarget> conversion)
        {
            return new DelegateConverter<TInput, TTarget>(conversion);
        }

        private static Object BlendColors(IList<Object> parameters)
        {
            var result = Color.FromArgb(0, 0, 0, 0);

            foreach (var item in parameters)
                result = ColorHelper.AlphaBlending(result, (Color)item);

            return result;
        }
        
        private static Object ChangeOpacity(IList<Object> parameters)
        {
            return ColorHelper.ChangeOpacity((Color)parameters[0], (double)parameters[1]);
        }
    }
}
