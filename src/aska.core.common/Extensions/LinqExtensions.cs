using System;
using System.Collections.Generic;

namespace kd.misc
{
    public static  class LinqExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
                yield return cur;
            }
        }
    }
}