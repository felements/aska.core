using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.CommandQuery.Specification;

namespace aska.core.infrastructure.CommandQuery
{
    public static class Extensions
    {
        private static readonly ConcurrentDictionary<Expression, object> CachedFunctions
            = new ConcurrentDictionary<Expression, object>();

        public static Func<TEntity, bool> AsFunc<TEntity>(this object entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IEntity
        {
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html

           
            return (Func<TEntity, bool>)   CachedFunctions.GetOrAdd(expr, id => new Lazy<object>(() => CachedFunctions.GetOrAdd(id, (object)expr.Compile())).Value );
        }

        public static bool Is<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IEntity
        {
            return AsFunc(entity, expr).Invoke(entity);
        }

        public static IQuery<TEntity, IExpressionSpecification<TEntity>> Where<TEntity>(
            this IQuery<TEntity, IExpressionSpecification<TEntity>> query,
            Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity
        {
            return query
                .Where(new ExpressionSpecification<TEntity>(expression));
        }
    }
}