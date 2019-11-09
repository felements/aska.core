using System;
using System.Linq.Expressions;

namespace Aska.Core.EntityStorage.Abstractions
{
    public class ExpressionSpecification<T> : IExpressionSpecification<T>
        where T : class
    {
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            SpecificationExpression = expression ?? (_ => true) ;
        }
        
        public Expression<Func<T, bool>> SpecificationExpression { get; }

        //private Func<T, bool> Func => this.AsFunc(SpecificationExpression);
        //todo

        public bool IsSatisfiedBy(T o)
        {
            return true;
            //todo
            //return Func(o);
        }
    }
}