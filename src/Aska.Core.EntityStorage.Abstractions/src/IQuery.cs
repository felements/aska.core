using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IQuery<TEntity, in TSpecification>
        where TEntity : class
        where TSpecification : ISpecification<TEntity>
    {
        IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression);

        IQuery<TEntity, TSpecification> Where(TSpecification specification);

        IQuery<TEntity, TSpecification> OrderBy<TProperty>(
           Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Ascending);

        Task<TEntity> SingleAsync(CancellationToken ct);

        Task<TEntity> SingleOrDefaultAsync(CancellationToken ct);

        Task<TEntity> FirstOrDefaultAsync(CancellationToken ct);

        Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct);

        Task<bool> AnyAsync(CancellationToken ct);

        Task<int> CountAsync(CancellationToken ct);
        
        //todo: paged
    }
}