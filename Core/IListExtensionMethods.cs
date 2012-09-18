using System;
using System.Collections.Generic;

namespace Core
{
    public static class IListExtensionMethods
    {
        public static void Sort<T>(this IList<T> list, Func<T, T, int> comparator)
        {
            for (var i = 0; i < list.Count - 1; i++)
            {
                for (var j = i + 1; j < list.Count; j++)
                {
                    if (comparator(list[i], list[j]) > 0)
                    {
                        var temp = list[j];
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
            }
        }
    }
}