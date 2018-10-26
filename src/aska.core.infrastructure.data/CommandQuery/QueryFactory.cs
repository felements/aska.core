using Autofac;
using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Interfaces;

namespace kd.infrastructure.CommandQuery
{
    public class QueryFactory : IQueryFactory
    {
        protected ILifetimeScope Scope;

        public QueryFactory(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>() where TEntity : class, IEntity
        {
            return Scope.Resolve<IQuery<TEntity, IExpressionSpecification<TEntity>>>();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity>
        {
            return Scope.Resolve<IQuery<TEntity, TSpecification>>();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity> where TQuery : IQuery<TEntity, TSpecification>
        {
            return Scope.Resolve<TQuery>();
        }
    }
}