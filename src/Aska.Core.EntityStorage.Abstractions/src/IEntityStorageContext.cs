using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IEntityStorageContext : IEntityStorageInitialize, IEntityStorageReader, IEntityStorageWriter
    {
    }

    public interface IEntityStorageReader
    {
        IQueryable<T> Get<T>() where T : class;
    }

    public interface IEntityStorageWriter
    {
        void Add<T>(T entity) where T : class;
        void Add<T>(IEnumerable<T> entity) where T : class;
        
        void Update<T>(T entity) where T : class;
        void Update<T>(IEnumerable<T> entity) where T : class;
        
        void Remove<T>(T entity) where T : class;
        void Remove<T>(IEnumerable<T> entity) where T : class;
        
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
    
    public interface IEntityStorageInitialize
    {
        void Initialize();
    }
}