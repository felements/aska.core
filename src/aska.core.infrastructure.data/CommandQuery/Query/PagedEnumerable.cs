using System.Collections.Generic;
using aska.core.infrastructure.data.CommandQuery.Interfaces;

namespace aska.core.infrastructure.data.CommandQuery.Query
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