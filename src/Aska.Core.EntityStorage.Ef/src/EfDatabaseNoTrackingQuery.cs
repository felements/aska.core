using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.Storage.Ef
{
    public class EfDatabaseNoTrackingQuery<TEntity, TSpecification>: IQuery<TEntity, TSpecification>
        where TEntity : class
        where TSpecification : IExpressionSpecification<TEntity>
    {
        private IQueryable<TEntity> _query;

        public EfDatabaseNoTrackingQuery(IEntityStorageReader<TEntity> reader)
        {
            _query = reader.Get().AsNoTracking();
        }
        
        public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct) => await _query.ToArrayAsync(ct);

        public Task<bool> AnyAsync(CancellationToken ct = default) => _query.AnyAsync(ct);

        public Task<int> CountAsync(CancellationToken ct = default) => _query.CountAsync(ct);

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken ct = default) => _query.FirstOrDefaultAsync(ct);

        public Task<TEntity> SingleAsync(CancellationToken ct = default) => _query.SingleAsync(ct);

        public Task<TEntity> SingleOrDefaultAsync(CancellationToken ct = default) => _query.SingleOrDefaultAsync(ct);
    
        public IQuery<TEntity, TSpecification> OrderBy<TProperty>(
            Expression<Func<TEntity, TProperty>> expression, 
            SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }
        
        public IQuery<TEntity, TSpecification> Where(TSpecification specification)
        {
            // if (!(specification is IExpressionSpecification<TEntity> expressionSpecification))
            //     throw new NotSupportedException("Only expression specifications are supported");
            _query = _query.Where(specification.SpecificationExpression);
            return this;
        }
        
        public IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
        {
            _query = _query.Where(expression);
            return this;
        }
    }
    
    public class EfDatabaseNoTrackingQuery<TEntity> : IQuery<TEntity, IExpressionSpecification<TEntity>> where TEntity : class
    {
        private IQueryable<TEntity> _query;

        public EfDatabaseNoTrackingQuery(IEntityStorageReader<TEntity> reader)
        {
            _query = reader.Get().AsNoTracking();
        }
        
        public IQuery<TEntity, IExpressionSpecification<TEntity>> Where(Expression<Func<TEntity, bool>> expression)
        {
            _query = _query.Where(expression);
            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Where(IExpressionSpecification<TEntity> specification)
        {
            _query = _query.Where(specification.SpecificationExpression);
            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> OrderBy<TProperty>(
            Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }

        public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct) => await _query.ToArrayAsync(ct);

        public Task<bool> AnyAsync(CancellationToken ct = default) => _query.AnyAsync(ct);
        
        public Task<int> CountAsync(CancellationToken ct = default) => _query.CountAsync(ct);

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken ct = default) => _query.FirstOrDefaultAsync(ct);
        
        public Task<TEntity> SingleAsync(CancellationToken ct = default) => _query.SingleAsync(ct);
        
        public Task<TEntity> SingleOrDefaultAsync(CancellationToken ct = default) => _query.SingleOrDefaultAsync(ct);
    }
}