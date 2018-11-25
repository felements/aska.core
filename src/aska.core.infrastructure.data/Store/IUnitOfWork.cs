using System;
using System.Threading.Tasks;
using kd.domainmodel.Entity;

namespace kd.infrastructure.Store
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();

        void Save<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Delete<TEntity>(Guid id) where TEntity : class, IEntity;
    }
}