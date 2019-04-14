using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<IEnumerable<TEntity>> AllAsync() => await _query.ToArrayAsync();

        public bool Any() => _query.Any();

        public Task<bool> AnyAsync() => _query.AnyAsync();
        
        public int Count() => _query.Count();

        public Task<int> CountAsync() => _query.CountAsync();

        public TEntity FirstOrDefault() => _query.FirstOrDefault();

        public Task<TEntity> FirstOrDefaultAsync() => _query.FirstOrDefaultAsync();
        
        public TEntity Single() => _query.Single(); 
        
        public Task<TEntity> SingleAsync() => _query.SingleAsync();
        
        public TEntity SingleOrDefault() => _query.SingleOrDefault();

        public Task<TEntity> SingleOrDefaultAsync() => _query.SingleOrDefaultAsync();
        
        
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