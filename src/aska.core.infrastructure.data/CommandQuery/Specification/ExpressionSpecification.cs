﻿using System;
using System.Linq.Expressions;
using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Interfaces;

namespace kd.infrastructure.CommandQuery.Specification
{
    public class ExpressionSpecification<T> : IExpressionSpecification<T>
        where T : class, IEntity
    {
        public Expression<Func<T, bool>> SpecificationExpression { get; protected set; }

        private Func<T, bool> Func
        {
            get
            {
                return this.AsFunc(SpecificationExpression);
            }
        }

        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            SpecificationExpression = expression;
        }

        public bool IsSatisfiedBy(T o)
        {
            return Func(o);
        }
    }
}