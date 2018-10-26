using System.Collections.Generic;
using kd.infrastructure.CommandQuery.Interfaces;

namespace kd.infrastructure.CommandQuery.Query
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