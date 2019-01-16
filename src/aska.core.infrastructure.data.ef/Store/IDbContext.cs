using System.Linq;
using System.Threading.Tasks;
using aska.core.infrastructure.data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace aska.core.infrastructure.data.ef.Store
{
    public interface IDbContext
    {
        DbSet<T> GetDbSet<T>() where T : class;
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