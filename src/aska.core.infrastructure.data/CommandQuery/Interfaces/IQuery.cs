using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface IQuery<TEntity, in TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression);

        IQuery<TEntity, TSpecification> Where(TSpecification specification);

        IQuery<TEntity, TSpecification> OrderBy<TProperty>(
           Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Ascending);

        //IQuery<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        TEntity Single();
        Task<TEntity> SingleAsync(CancellationToken ct);

        TEntity SingleOrDefault();
        Task<TEntity> SingleOrDefaultAsync(CancellationToken ct);

        TEntity FirstOrDefault();
        Task<TEntity> FirstOrDefaultAsync(CancellationToken ct);

        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct);

        bool Any();
        Task<bool> AnyAsync(CancellationToken ct);

        IEnumerable<TEntity> Paged(int? pageNumber, int? take);

        int Count();
        Task<int> CountAsync(CancellationToken ct);
    }
}