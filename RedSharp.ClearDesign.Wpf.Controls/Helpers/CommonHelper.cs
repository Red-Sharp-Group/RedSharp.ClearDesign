using System;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RedSharp.ClearDesign.Wpf.Controls.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Ensures the count of a list to a given count. Creates with a given factory or removes items when necessary.
        /// If Input IList is a List, AddRange or RemoveRange is used when there's no factory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EnsureListCount<TItem>(this IList<TItem> list, int count, Func<TItem> factory = null)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (list.EnsureListCountAtLeast(count, factory))
                return true;

            if (list.Count <= count)
                return false;

            if (list is List<TItem> objList)
            {
                objList.RemoveRange(count, list.Count - count);
            }
            else
            {
                for (int index = list.Count - 1; index >= count; --index)
                    list.RemoveAt(index);
            }

            return true;
        }

        /// <summary>
        /// Ensures the count of a list is at least the given count. Creates with a given factory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EnsureListCountAtLeast<TItem>(this IList<TItem> list, int count, Func<TItem> factory = null)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (list.Count >= count)
                return false;

            if (list is List<TItem> objList && factory == null)
            {
                objList.AddRange(new TItem[count - list.Count]);
            }
            else
            {
                for (int count1 = list.Count; count1 < count; ++count1)
                    list.Add(factory == null ? default : factory());
            }

            return true;
        }

        /// <summary>
        /// Removes the last item from the given list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Sets the value if different. Avoids setting a local value if possible.
        /// Returns true when the value has been changed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySet(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
        {
            if (Equals(dependencyObject.GetValue(dependencyProperty), value))
                return false;

            dependencyObject.SetValue(dependencyProperty, value);

            return true;
        }
    }
}