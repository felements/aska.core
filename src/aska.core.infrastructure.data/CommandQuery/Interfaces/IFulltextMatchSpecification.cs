using System;
using System.Linq.Expressions;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface IFulltextMatchSpecification<T> : ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, string>>[] FieldSelectors { get; }
        string[] SearchQuery { get; }
    }
}