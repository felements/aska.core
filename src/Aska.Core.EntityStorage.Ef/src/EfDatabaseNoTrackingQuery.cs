using System.Linq.Expressions;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.EntityStorage.Ef;

public class EfDatabaseNoTrackingQuery<TEntity, TSpecification>(IEntityStorageReader<TEntity> reader) : IQuery<TEntity, TSpecification>
    where TEntity : class
    where TSpecification : IExpressionSpecification<TEntity>
{
    private IQueryable<TEntity> _query = reader.Get().AsNoTracking();

    public IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
    {
        _query = _query.Where(expression);
        return this;
    }

    public IQuery<TEntity, TSpecification> Where(TSpecification specification)
    {
        _query = _query.Where(specification.SpecificationExpression);
        return this;
    }

    public IQuery<TEntity, TSpecification> OrderBy<TProperty>(
        Expression<Func<TEntity, TProperty>> expression,
        SortOrder sortOrder = SortOrder.Ascending)
    {
        _query = sortOrder == SortOrder.Ascending
            ? _query.OrderBy(expression)
            : _query.OrderByDescending(expression);
        return this;
    }

    public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct = default) => await _query.ToArrayAsync(ct);

    public Task<bool> AnyAsync(CancellationToken ct = default) => _query.AnyAsync(ct);

    public Task<int> CountAsync(CancellationToken ct = default) => _query.CountAsync(ct);

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken ct = default) => _query.FirstOrDefaultAsync(ct);

    public Task<TEntity> SingleAsync(CancellationToken ct = default) => _query.SingleAsync(ct);

    public Task<TEntity?> SingleOrDefaultAsync(CancellationToken ct = default) => _query.SingleOrDefaultAsync(ct);
}

public class EfDatabaseNoTrackingQuery<TEntity>(IEntityStorageReader<TEntity> reader)
    : EfDatabaseNoTrackingQuery<TEntity, IExpressionSpecification<TEntity>>(reader), IQuery<TEntity>
    where TEntity : class;
