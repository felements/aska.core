using System;
using System.Linq.Expressions;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface IFulltextMatchSpecification<T> : ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, string>>[] FieldSelectors { get; }
        string[] SearchQuery { get; }
    }
}