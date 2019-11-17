using System;
using System.Linq.Expressions;
using Aska.Core.EntityStorage.Abstractions.Extensions;

namespace Aska.Core.EntityStorage.Abstractions
{
    public class ExpressionSpecification<T> : IExpressionSpecification<T>
        where T : class
    {
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            SpecificationExpression = expression ?? (_ => true) ;
        }
        
        public Expression<Func<T, bool>> SpecificationExpression { get; protected set; }

        private Func<T, bool> Func => SpecificationExpression.AsFunc();

        public bool IsSatisfiedBy(T o) => Func(o);
    }
}