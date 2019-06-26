using System;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.Store
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync(CancellationToken ct);

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Delete<TEntity>(Guid id) where TEntity : class, IEntity;
        
        void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;
        
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;
    }
}