using System;
using System.Linq;
using System.Collections.Generic;
using aska.core.common;
using aska.core.infrastructure.data.ef.Context;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.ef.Query
{
    public class DbContextQueryableEntityProvider : IQueryableEntityProvider 
    {
        private readonly IServiceProvider _serviceProvider;
        
        // TEntity => TContext map
        //todo: how to handle case where one type exists in many contexts?
        private readonly Dictionary<Type, Type> _entityTypeMap;

        public DbContextQueryableEntityProvider(IEnumerable<IDbContextMetadata> contexts, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _entityTypeMap = contexts
                .SelectMany(c => c.GetEntityTypes(), (ctx, entity) => new { Context = ctx.GetType(), Entity = entity })
                .ToDictionary(k => k.Entity, v => v.Context);
        }
        
        IQueryable<TEntity> IQueryableEntityProvider.GetEntity<TEntity>() 
        {
            var dbContextType = _entityTypeMap[typeof(TEntity)];
            return ((IDbContext)_serviceProvider.GetRequiredService(dbContextType)).GetDbSet<TEntity>();
        }

        public IDbContext GetContext<TEntity>() where TEntity : class, IEntity
        {
            var dbContextType = _entityTypeMap[typeof(TEntity)];
            return ((IDbContext) _serviceProvider.GetRequiredService(dbContextType));
        }
    }
}