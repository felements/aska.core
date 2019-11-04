using System;
using System.Linq.Expressions;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IExpressionSpecification<T> : ISpecification<T>
        where T : class
    {
        Expression<Func<T, bool>> SpecificationExpression { get; }
    }
}