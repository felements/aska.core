using System.Collections.Generic;

namespace Aska.Core.Storage.Abstractions
{
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        long TotalCount { get; }
    }
}