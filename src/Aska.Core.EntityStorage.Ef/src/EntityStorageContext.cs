using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.Storage.Ef
{
    public class EntityStorageContext: DbContext, IEntityStorageContext
    {
        protected EntityStorageContext(): base()
        {
        }
        protected EntityStorageContext(DbContextOptions options) : base (options)
        {
        }

        public virtual IQueryable<T> Get<T>() where T : class => base.Set<T>();

        public new virtual void Add<T>(T entity) where T : class => base.Add(entity);

        public virtual void Add<T>(IEnumerable<T> entity) where T : class => base.AddRange(entity);

        public new virtual void Update<T>(T entity) where T : class => base.Update(entity);

        public virtual void Update<T>(IEnumerable<T> entity) where T : class => base.UpdateRange(entity);

        public new virtual void Remove<T>(T entity) where T : class => base.Remove(entity);

        public virtual void Remove<T>(IEnumerable<T> entity) where T : class => base.RemoveRange(entity);

        public virtual Task<int> SaveAsync(CancellationToken cancellationToken) => base.SaveChangesAsync();
        
        public virtual Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class EntityStorageReaderContextProxy<TEntity, TContext> : IEntityStorageReader<TEntity>
        where TEntity : class
        where TContext : IEntityStorageReader
    {
        private readonly TContext _context;

        public EntityStorageReaderContextProxy(TContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Get() => _context.Get<TEntity>();
    }

    public class EntityStorageWriterContextProxy<TEntity, TContext> : IEntityStorageWriter<TEntity>
        where TEntity : class
        where TContext : IEntityStorageWriter
    {
        private readonly TContext _context;

        public EntityStorageWriterContextProxy(TContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity) => _context.Add(entity);

        public void Add(IEnumerable<TEntity> entities) => _context.Add(entities);

        public void Update(TEntity entity) => _context.Update(entity);

        public void Update(IEnumerable<TEntity> entities) => _context.Update(entities);

        public void Remove(TEntity entity) => _context.Remove(entity);

        public void Remove(IEnumerable<TEntity> entities) => _context.Remove(entities);

        public Task<int> SaveAsync(CancellationToken cancellationToken = default) =>
            _context.SaveAsync(cancellationToken);
    }
}