namespace Aska.Core.Storage.Abstractions
{
    public interface IQueryFactory
    {
        IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class;

        IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class
            where TSpecification : ISpecification<TEntity>;

        TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class
            where TSpecification : ISpecification<TEntity>
            where TQuery : IQuery<TEntity, TSpecification>;
    }
}