using System;
using System.Linq.Expressions;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface IExpressionSpecification<T> : ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, bool>> SpecificationExpression { get; }
    }
}