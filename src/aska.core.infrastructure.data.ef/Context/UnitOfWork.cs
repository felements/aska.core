using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;
using aska.core.infrastructure.data.ef.Query;
using aska.core.infrastructure.data.Store;
using Microsoft.EntityFrameworkCore;

namespace aska.core.infrastructure.data.ef.Context
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IQueryableEntityProvider _contextProvider;
        
        private IDbContext _ctx;

        private IDbContext GetOrResolveContext<TEntity>() where TEntity : class, IEntity 
            => _ctx ?? (_ctx = _contextProvider.GetContext<TEntity>());


        public UnitOfWork(IQueryableEntityProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            if (_ctx == null) throw new InvalidOperationException("Db context not initialized.");
            await _ctx.SaveChangesAsync(ct);
        }

        public void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _contextProvider.GetContext<TEntity>();
            SaveInternal(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var set = GetOrResolveContext<TEntity>().GetDbSet<TEntity>();
            set.Remove(entity);
        }

        public void Delete<TEntity>(Guid id) where TEntity : class, IEntity
        {
            var set = GetOrResolveContext<TEntity>().GetDbSet<TEntity>();
            var entity = set.SingleOrDefault(x => x.Id == id);
            if (entity != null) set.Remove(entity);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var set = GetOrResolveContext<TEntity>().GetDbSet<TEntity>();
            set.Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var set = GetOrResolveContext<TEntity>().GetDbSet<TEntity>();
            set.Update(entity);
        }

        public void Dispose()
        {
            _ctx = null;
        }

        private void SaveInternal<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            
            IQueryable<TEntity> set = _ctx.GetDbSet<TEntity>();

            var dbentity = set.FirstOrDefault(e => entity.Id.Equals(e.Id));
            if (dbentity != null)
            {
                if (dbentity == entity) return;

                _ctx.Entry(dbentity).CurrentValues.SetValues(entity);
            }
            else
            {
                _ctx.Entry(entity).State = EntityState.Added;
            }
        }
    }
}