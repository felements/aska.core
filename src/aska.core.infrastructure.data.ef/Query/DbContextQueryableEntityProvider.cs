using System.Linq;
using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.ef.Query
{
    public class DbContextQueryableEntityProvider<TContext> : IQueryableEntityProvider 
        where TContext: class, IDbContext
    {
        private readonly IDbContext _ctx;

        public DbContextQueryableEntityProvider(TContext ctx)
        {
            _ctx = ctx;
        }
        
        IQueryable<TEntity> IQueryableEntityProvider.Get<TEntity>() 
        {
            return _ctx.GetDbSet<TEntity>();
        }
    }
}