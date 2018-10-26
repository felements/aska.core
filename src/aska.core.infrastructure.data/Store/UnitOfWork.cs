using System;
using System.Linq;
using System.Threading.Tasks;
using kd.domainmodel.Entity;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace kd.infrastructure.Store
{
    internal class UnitOfWork : IUnitOfWork
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnitOfWork(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            Logger.Trace("Constructing UOW with context id: {0}", ctx.Id);
        }

        private IDbContext _ctx;

        Task IUnitOfWork.CommitAsync()
        {
            return CommitAsync();
        }

        void IUnitOfWork.Commit()
        {
            _ctx.SaveChanges();
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

        public int Commit()
        {
            return _ctx.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _ctx.SaveChangesAsync();
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