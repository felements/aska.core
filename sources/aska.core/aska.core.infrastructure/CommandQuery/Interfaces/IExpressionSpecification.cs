using System;
using System.Linq.Expressions;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface IExpressionSpecification<T> : ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, bool>> SpecificationExpression { get; }
    }
}