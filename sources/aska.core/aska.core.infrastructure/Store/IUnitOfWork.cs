using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.Store
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();

        void Save<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Save<TEntity, TProperty_1>(TEntity entity, Expression<Func<TEntity, TProperty_1>> include_1) where TEntity : class, IEntity;

        void Save<TEntity, TProperty_1, TProperty_2>(TEntity entity, Expression<Func<TEntity, TProperty_1>> include_1, Expression<Func<TEntity, TProperty_2>> include_2) where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Delete<TEntity>(Guid id) where TEntity : class, IEntity;

        void Truncate<TEntity>() where TEntity : class, IEntity;

        void SetChangeTracking<TEntity>(bool enabled) where TEntity : class, IEntity;
    }
}