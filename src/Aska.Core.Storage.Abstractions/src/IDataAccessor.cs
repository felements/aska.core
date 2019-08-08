using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.Storage.Abstractions
{
    public interface IDataAccessor : IDataReader
    {
        void Add<T>(T entity);
        void AddRange<T>(params T[] entities);

        void Update<T>(T entity);
        void UpdateRange<T>(params T[] entities);

        void Remove<T>(T entity);
        void RemoveRange<T>(params T[] entities);

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}