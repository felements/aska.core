using System;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.Store
{
    public static class DbContextSelectorExtensions
    {
        public static IDbContext Choose(this IIndex<DbContextKey, IDbContext> index, Type type)
        {
            if (typeof(IRegularEntity).IsAssignableFrom(type)) return index[DbContextKey.Regular];
            if (typeof(IReadonlyEntity).IsAssignableFrom(type)) return index[DbContextKey.Readonly];

            return null;
        }
    }
}