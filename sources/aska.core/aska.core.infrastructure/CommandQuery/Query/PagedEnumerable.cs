using System.Collections.Generic;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Query
{
    public class PagedEnumerable<T> : List<T>, IPagedEnumerable<T>
    {
        public PagedEnumerable(long totalCount)
        {
            TotalCount = totalCount;
        }

        public long TotalCount { get; }
    }
}