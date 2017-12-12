using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.CommandQuery.Specification;
using aska.core.infrastructure.Store;

namespace aska.core.infrastructure.CommandQuery.Query
{
    public class DbQueryDynamicType : IQuery<IEntity, ByIdExpressionSpecification<IEntity>>
    {
        public Type EntityType;

        private IQueryable<IEntity> _query;

        public DbQueryDynamicType(IIndex<DbContextKey, IDbContext> ctx, Type entityType) 
        {
            EntityType = entityType;

            _query =  typeof(IDbContext)
                .GetMethod(nameof(IDbContext.GetDbSet))
                .MakeGenericMethod(entityType)
                .Invoke(ctx.Choose(entityType), new object[] { }) as IQueryable<IEntity>;
        }

        #region Implementation of IQuery<IEntity,in ByIdExpressionSpecification<IEntity>>

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> Where(Expression<Func<IEntity, bool>> expression)
        {
            _query = _query.Where(expression);
            return this;
        }

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> Where(ByIdExpressionSpecification<IEntity> specification)
        {
            _query = _query.Where(specification.SpecificationExpression);
            return this;
        }

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> Where(IExpressionSpecification<IEntity> specification)
        {
            _query = _query.Where(specification.SpecificationExpression);
            return this;
        }

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> OrderBy<TProperty>(Expression<Func<IEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            var ordered = sortOrder == SortOrder.Ascending
                ? _query.OrderBy(expression)
                : _query.OrderByDescending(expression);
            _query = ordered;
            return this;
        }

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> Include<TProperty>(Expression<Func<IEntity, TProperty>> expression)
        {
            _query = _query.Include(expression);
            return this;
        }

        public IEntity Single()
        {
            return _query.Single();
        }

        public IEntity SingleOrDefault()
        {
            return _query.SingleOrDefault();
        }

        public IEntity FirstOrDefault()
        {
            return _query.FirstOrDefault();
        }

        public IEntity LastOrDefault<TProperty>(Expression<Func<IEntity, TProperty>> expression)
        {
            return _query.OrderByDescending(expression).FirstOrDefault();
        }

        public IEnumerable<IEntity> All()
        {
            return _query.AsEnumerable();
        }

        public IEnumerable<IEntity> Paged(int? page, int? size)
        {
            return size.HasValue
                ? _query.Skip((page ?? 0) * size.Value).Take(size.Value).ToList()
                : All();
        }

        public long Count()
        {
            return _query.Count();
        }

        public IEnumerable<IEntity> Feed(Guid? lastId, int count)
        {
            return !lastId.HasValue
                 ? _query.Take(count).AsEnumerable()
                 : _query.AsEnumerable().SkipWhile(x => x.Id == lastId.Value).Take(count);
        }

        #endregion
    }
}