using System.Collections.Generic;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        long TotalCount { get; }
    }
}