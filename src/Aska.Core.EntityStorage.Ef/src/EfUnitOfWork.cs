using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.Storage.Ef
{
    internal sealed class EfUnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        private readonly TContext _context;

        public EfUnitOfWork(TContext context)
        {
            _context = context;
        }

        IQueryable<T> IDataReader.Set<T>() => _context.Set<T>();

        void IDataAccessor.Add<T>(T entity) => _context.Add(entity);// attach?

        void IDataAccessor.AddRange<T>(params T[] entities) => _context.AddRange(entities);

        void IDataAccessor.Update<T>(T entity) => _context.Update(entity);

        void IDataAccessor.UpdateRange<T>(params T[] entities) => _context.UpdateRange(entities);

        void IDataAccessor.Remove<T>(T entity) => _context.Remove(entity);

        void IDataAccessor.RemoveRange<T>(params T[] entities) => _context.RemoveRange(entities);

        int IDataAccessor.SaveChanges() => _context.SaveChanges();

        async Task<int> IDataAccessor.SaveChangesAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);

        void IDisposable.Dispose()
        {
        }
    }
}