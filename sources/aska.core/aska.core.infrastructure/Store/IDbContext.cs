using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace aska.core.infrastructure.Store
{
    public interface IDbContext
    {
        IDbSet<T> GetDbSet<T>() where T : class;

        DbSet GetDbSet(Type entityType);

        int SaveChanges();

        Task<int> SaveChangesAsync();

        DbEntityEntry Entry(object o);

        bool AutoDetectChangesEnabled { get; set; }

        bool LazyLoadingEnabled { get; set; }

        void DetectChanges();

        IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query, params Expression<Func<TEntity, TProperty>>[] properties);

        void Truncate<T>() where T : class;

        void ExecuteSqlCommand(string command);
    }
}