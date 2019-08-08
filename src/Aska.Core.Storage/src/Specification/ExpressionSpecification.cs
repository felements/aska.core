using System;
using System.Linq.Expressions;
using Aska.Core.Storage.Abstractions;

namespace Aska.Core.Storage.Specification
{
    public class ExpressionSpecification<T> : IExpressionSpecification<T>
        where T : class
    {
        public Expression<Func<T, bool>> SpecificationExpression { get; protected set; }

        private Func<T, bool> Func => this.AsFunc(SpecificationExpression);

        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            SpecificationExpression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public bool IsSatisfiedBy(T o)
        {
            return Func(o);
        }
    }
}