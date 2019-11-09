using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Aska.Core.EntityStorage.Abstractions.Extensions
{
    public static class ExpressionExtensions
    {
        private class Extensions<TEntity>
        {
            private static readonly ConcurrentDictionary<Expression<Func<TEntity, bool>>, Func<TEntity, bool>> Cached =
                new ConcurrentDictionary<Expression<Func<TEntity, bool>>, Func<TEntity, bool>>();

            public static Func<TEntity, bool> AsFuncCached(Expression<Func<TEntity, bool>> expression) =>
                Cached.GetOrAdd(expression, e => e.Compile());
        }
        
        public static Func<TEntity, bool> AsFunc<TEntity>(Expression<Func<TEntity, bool>> expr)
            where TEntity : class
        {
            return Extensions<TEntity>.AsFuncCached(expr);
        }

        public static bool Is<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class
        {
            return AsFunc(expr).Invoke(entity);
        }
    }
}