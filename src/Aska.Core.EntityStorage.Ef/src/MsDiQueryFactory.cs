using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.EntityStorage.Ef;

internal sealed class MsDiQueryFactory(IServiceProvider provider) : IQueryFactory
{
    public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
        where TEntity : class => provider.GetRequiredService<IQuery<TEntity, IExpressionSpecification<TEntity>>>();

    public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
        where TEntity : class
        where TSpecification : ISpecification<TEntity> => provider.GetRequiredService<IQuery<TEntity, TSpecification>>();

    public TQuery GetQuery<TEntity, TSpecification, TQuery>()
        where TEntity : class
        where TSpecification : ISpecification<TEntity>
        where TQuery : IQuery<TEntity, TSpecification> => provider.GetRequiredService<TQuery>();
}
