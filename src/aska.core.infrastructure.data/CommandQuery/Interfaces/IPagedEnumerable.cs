using System.Collections.Generic;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        long TotalCount { get; }
    }
}