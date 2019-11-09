using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IStorageContext
    {
    }
    
    public interface IEntityStorageContext : IEntityStorageInitialize, IEntityStorageReader, IEntityStorageWriter
    {
    }

    public interface IEntityStorageReader : IStorageContext
    {
        IQueryable<T> Get<T>() where T : class;
    }

    public interface IEntityStorageReader<out T> : IStorageContext 
        where T : class
    {
        IQueryable<T> Get();
    }

    public interface IEntityStorageWriter : IStorageContext
    {
        void Add<T>(T entity) where T : class;
        void Add<T>(IEnumerable<T> entity) where T : class;
        
        void Update<T>(T entity) where T : class;
        void Update<T>(IEnumerable<T> entity) where T : class;
        
        void Remove<T>(T entity) where T : class;
        void Remove<T>(IEnumerable<T> entity) where T : class;
        
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IEntityStorageWriter<in T> : IStorageContext where T: class
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        
        void Update(T entity);
        void Update(IEnumerable<T> entity);
        
        void Remove(T entity);
        void Remove(IEnumerable<T> entity);
        
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IEntityStorageInitialize : IStorageContext
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}