using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.EntityStorage.Ef;

public class EntityStorageContext : DbContext, IEntityStorageContext
{
    protected EntityStorageContext()
    {
    }

    protected EntityStorageContext(DbContextOptions options) : base(options)
    {
    }

    public virtual IQueryable<T> Get<T>() where T : class => Set<T>();

    public new virtual void Add<T>(T entity) where T : class => base.Add(entity);

    public virtual void Add<T>(IEnumerable<T> entity) where T : class => AddRange(entity);

    public new virtual void Update<T>(T entity) where T : class => base.Update(entity);

    public virtual void Update<T>(IEnumerable<T> entity) where T : class => UpdateRange(entity);

    public new virtual void Remove<T>(T entity) where T : class => base.Remove(entity);

    public virtual void Remove<T>(IEnumerable<T> entity) where T : class => RemoveRange(entity);

    public virtual Task<int> SaveAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);

    public virtual Task InitializeAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}

public class EntityStorageReaderContextProxy<TEntity, TContext>(TContext context) : IEntityStorageReader<TEntity>
    where TEntity : class
    where TContext : IEntityStorageReader
{
    public IQueryable<TEntity> Get() => context.Get<TEntity>();
}

public class EntityStorageWriterContextProxy<TEntity, TContext>(TContext context) : IEntityStorageWriter<TEntity>
    where TEntity : class
    where TContext : IEntityStorageWriter
{
    public void Add(TEntity entity) => context.Add(entity);

    public void Add(IEnumerable<TEntity> entities) => context.Add(entities);

    public void Update(TEntity entity) => context.Update(entity);

    public void Update(IEnumerable<TEntity> entities) => context.Update(entities);

    public void Remove(TEntity entity) => context.Remove(entity);

    public void Remove(IEnumerable<TEntity> entities) => context.Remove(entities);

    public Task<int> SaveAsync(CancellationToken cancellationToken = default) => context.SaveAsync(cancellationToken);
}
