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

        Task<TEntity> SingleAsync(CancellationToken ct = default);

        Task<TEntity> SingleOrDefaultAsync(CancellationToken ct = default);

        Task<TEntity> FirstOrDefaultAsync(CancellationToken ct = default);

        Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct = default);

        Task<bool> AnyAsync(CancellationToken ct = default);

        Task<int> CountAsync(CancellationToken ct = default);
        
        //todo: paged
    }
    
    public interface IQuery<TEntity> : IQuery<TEntity, IExpressionSpecification<TEntity>> where TEntity : class
    {
        
    }
}