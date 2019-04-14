using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using Microsoft.EntityFrameworkCore;

namespace aska.core.infrastructure.data.ef.Query
{
    public class EfDatabaseQuery<TEntity, TSpecification> : IQuery<TEntity, TSpecification>
        where TEntity : class, IEntity
        where TSpecification : IExpressionSpecification<TEntity>
    {
        private IQueryable<TEntity> _query;
        

        public EfDatabaseQuery(IQueryableEntityProvider queryable)
        {
            _query = queryable.Get<TEntity>();
        }

        public IEnumerable<TEntity> All() => _query.ToArray();
        public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct) => await _query.ToArrayAsync(ct);

        public bool Any() => _query.Any();

        public Task<bool> AnyAsync(CancellationToken ct) => _query.AnyAsync(ct);
        
        public int Count() => _query.Count();

        public Task<int> CountAsync(CancellationToken ct) => _query.CountAsync(ct);

        public TEntity FirstOrDefault() => _query.FirstOrDefault();

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken ct) => _query.FirstOrDefaultAsync(ct);
        
        public TEntity Single() => _query.Single(); 
        
        public Task<TEntity> SingleAsync(CancellationToken ct) => _query.SingleAsync(ct);
        
        public TEntity SingleOrDefault() => _query.SingleOrDefault();

        public Task<TEntity> SingleOrDefaultAsync(CancellationToken ct) => _query.SingleOrDefaultAsync(ct);
        
        
        public IEnumerable<TEntity> Paged(int? pageNumber, int? take)
        {
            return take.HasValue
                ? _query.Skip( (pageNumber ?? 0) * take.Value ).Take( take.Value ).ToList() 
                : All();
        }

        public IQuery<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }

        public IQuery<TEntity, TSpecification> Where(TSpecification specification)
        {
            if (!(specification is IExpressionSpecification<TEntity> expressionSpecification))
                throw new NotSupportedException("Only expression specifications are supported");
            _query = _query.Where(expressionSpecification.SpecificationExpression);
            return this;
        }

        public IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
        {
            _query = _query.Where(expression);
            return this;
        }
    }
}