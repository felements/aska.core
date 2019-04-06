using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface IQueryFactory
    {
        IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity;

        IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>;

        TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
            where TQuery : IQuery<TEntity, TSpecification>;
    }
}