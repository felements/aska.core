using System.Linq;
using System.Threading.Tasks;
using kd.domainmodel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace kd.infrastructure.Store
{
    public interface IDbContext
    {
        string Id { get; }
        DbSet<T> GetDbSet<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        EntityEntry Entry(object o);
        void Dispose();

        bool AutoDetectChangesEnabled { get; set; }
        void DetectChanges();

        //IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query, params Expression<Func<TEntity, TProperty>>[] properties);

        string GetTableName<T>() where T : class, IEntity;
        void TruncateTable<T>() where T : class, IEntity;
        void ExecuteRawSqlCommand(string command);
        IQueryable<T> ExecuteRawSqlQuery<T>(string query, params object[] parameters) where T : class;

        string[] Migrate();
    }
}