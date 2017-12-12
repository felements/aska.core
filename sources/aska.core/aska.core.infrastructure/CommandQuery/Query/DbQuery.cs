using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.Store;

namespace aska.core.infrastructure.CommandQuery.Query
{
    public class DbQuery<TEntity, TSpecification> : IQuery<TEntity, TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        private IQueryable<TEntity> _query;

        public DbQuery(IIndex<DbContextKey, IDbContext> ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            _query = ctx.Choose(typeof(TEntity)).GetDbSet<TEntity>().AsQueryable();
        }

        public TEntity LastOrDefault<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return _query.OrderByDescending(expression).FirstOrDefault();
        }
        
        public IEnumerable<TEntity> All()
        {
            return _query.AsEnumerable();
        }

        public long Count()
        {
            return _query.Count();
        }

        public TEntity FirstOrDefault()
        {
            return _query.FirstOrDefault();
        }

        public IQuery<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            _query = _query.Include(expression);
            return this;
        }

        public IQuery<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }

        public IEnumerable<TEntity> Paged(int? page, int? size)
        {
            return size.HasValue
                ? _query.Skip( (page ?? 0) * size.Value ).Take( size.Value ).AsEnumerable() 
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
            var expressionSpecification = specification as IExpressionSpecification<TEntity>;
            if (expressionSpecification != null)
            {
                _query = _query.Where(expressionSpecification.SpecificationExpression);
                return this;
            }
            throw new NotSupportedException("Only expression specifications are supported");
        }

        public IQuery<TEntity, TSpecification> Where(IExpressionSpecification<TEntity> expressionSpecification)
        {
            _query = _query.Where(expressionSpecification.SpecificationExpression);
            return this;
        }

        public IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
        {
            _query = _query.Where(expression);
            return this;
        }

        
        public IEnumerable<TEntity> Feed(Guid? lastId, int count)
        {
            return !lastId.HasValue
                ? _query.Take(count).AsEnumerable()
                : _query.AsEnumerable().SkipWhile(x => x.Id != lastId.Value).Skip(1).Take(count);
        }
    }
}