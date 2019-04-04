using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.Model;

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

        public IEnumerable<TEntity> All()
        {
            return _query.ToList();
        }

        public bool Any()
        {
            return _query.Any();
        }

        public long Count()
        {
            return _query.Count();
        }

        public TEntity FirstOrDefault()
        {
            return _query.FirstOrDefault();
        }

        public IQuery<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }
       
        public IEnumerable<TEntity> Paged(int? pageNumber, int? take)
        {
            return take.HasValue
                ? _query.Skip( (pageNumber ?? 0) * take.Value ).Take( take.Value ).ToList() 
                : All();
        }

        public TEntity Single()
        {
            return _query.Single();
        }

        public TEntity SingleOrDefault()
        {
            return _query.SingleOrDefault();
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