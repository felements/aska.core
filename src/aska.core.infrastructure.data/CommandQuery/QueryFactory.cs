using System;
using aska.core.infrastructure.data.Model;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.CommandQuery
{
    public class QueryFactory : IQueryFactory
    {
        protected readonly IServiceProvider Scope;

        public QueryFactory(IServiceProvider scope)
        {
            Scope = scope;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>() where TEntity : class, IEntity
        {
            return Scope.GetRequiredService<IQuery<TEntity, IExpressionSpecification<TEntity>>>();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity>
        {
            return Scope.GetRequiredService<IQuery<TEntity, TSpecification>>();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity> where TQuery : IQuery<TEntity, TSpecification>
        {
            return Scope.GetRequiredService<TQuery>();
        }
    }
}