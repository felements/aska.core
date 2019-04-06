using System.Linq;
using aska.core.common;
using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.ef.Query
{
    
    //TODO: support multiple dbContexts
    public interface IQueryableEntityProvider
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity: class, IEntity;
    }
}