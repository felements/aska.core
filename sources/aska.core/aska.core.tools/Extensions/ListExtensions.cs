using System;
using System.Collections.Generic;

namespace aska.core.tools.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<List<T>> Split<T>(this List<T> locations, int nSize = 30)
        {
            for (var i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}