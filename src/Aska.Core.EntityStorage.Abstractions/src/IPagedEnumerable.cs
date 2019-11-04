using System.Collections.Generic;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        long TotalCount { get; }
    }
}