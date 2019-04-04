using System.Linq;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.Model;

namespace aska.core.infrastructure.data.ef.Query
{
    
    //TODO: support multiple dbContexts
    public interface IQueryableEntityProvider
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity: class, IEntity;
    }
}