using System;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.CommandQuery.Specification;

namespace aska.core.infrastructure.CommandQuery
{
    public class QueryFactory : IQueryFactory
    {
        protected ILifetimeScope Scope;

        public QueryFactory(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public IQuery<IEntity, ByIdExpressionSpecification<IEntity>> GetQuery(Type entityType)
        {
            return Scope.Resolve<IQuery<IEntity, ByIdExpressionSpecification<IEntity>>>(new TypedParameter(typeof(Type), entityType));
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