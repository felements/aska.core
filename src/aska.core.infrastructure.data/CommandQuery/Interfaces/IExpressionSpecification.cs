using System;
using System.Linq.Expressions;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface IExpressionSpecification<T> : ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, bool>> SpecificationExpression { get; }
    }
}