using System.Linq;
using aska.core.common;
using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.ef.Query
{
    public interface IQueryableEntityProvider
    {
        IQueryable<TEntity> GetEntity<TEntity>() where TEntity: class, IEntity;

        IDbContext GetContext<TEntity>() where TEntity : class, IEntity;
    }
}