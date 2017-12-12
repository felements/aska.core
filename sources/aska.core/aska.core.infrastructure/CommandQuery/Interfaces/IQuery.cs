using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface IQuery<TEntity, in TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression);

        IQuery<TEntity, TSpecification> Where(TSpecification specification);

        IQuery<TEntity, TSpecification> Where(IExpressionSpecification<TEntity> specification);

        IQuery<TEntity, TSpecification> OrderBy<TProperty>(
           Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Ascending);

        IQuery<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        TEntity Single();

        TEntity SingleOrDefault();

        TEntity FirstOrDefault();

        TEntity LastOrDefault<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        IEnumerable<TEntity> All();

        IEnumerable<TEntity> Paged(int? page, int? size);

        IEnumerable<TEntity> Feed(Guid? lastId, int count);

        long Count();
    }

    
}