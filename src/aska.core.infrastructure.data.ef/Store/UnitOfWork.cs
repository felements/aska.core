using System;
using System.Linq;
using System.Threading.Tasks;
using aska.core.common.Data.Entity;
using aska.core.infrastructure.data.Store;
using Microsoft.EntityFrameworkCore;

namespace aska.core.infrastructure.data.ef.Store
{
    internal class UnitOfWork : IUnitOfWork
    {
        private IDbContext _ctx;

        public UnitOfWork(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task CommitAsync()
        {
            await _ctx.SaveChangesAsync();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            SaveInternal(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var set = _ctx.GetDbSet<TEntity>();
            var dbentity = (TEntity)set.FirstOrDefault(e => entity.Id.Equals(e.Id));
            if (dbentity != null) set.Remove(dbentity);
        }

        public void Delete<TEntity>(Guid id) where TEntity : class, IEntity
        {
            var dbSet = _ctx.GetDbSet<TEntity>();
            var entity = dbSet.SingleOrDefault(x => x.Id == id);
            if (entity != null) dbSet.Remove(entity);
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