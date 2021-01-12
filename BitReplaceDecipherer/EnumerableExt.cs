using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BitReplaceDecipherer
{
    public static class EnumerableExt
    {
        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate) {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items) {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            T buff = list[index1];
            list[index1] = list[index2];
            list[index2] = buff;
        }

        public static void Swap<TKey, TValue>(this IDictionary<TKey,TValue> list, TKey index1, TKey index2)
        {
            TValue buff = list[index1];
            list[index1] = list[index2];
            list[index2] = buff;
        }
    }
}