using System;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage
{
    public class QueryFactory : IQueryFactory
    {
        protected readonly IServiceProvider Provider;

        public QueryFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>() where TEntity : class
        {
            return Provider.GetRequiredService<IQuery<TEntity, IExpressionSpecification<TEntity>>>();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>() 
            where TEntity : class
            where TSpecification : ISpecification<TEntity>
        {
            return Provider.GetRequiredService<IQuery<TEntity, TSpecification>>();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>() 
            where TEntity : class
            where TSpecification : ISpecification<TEntity> where TQuery : IQuery<TEntity, TSpecification>
        {
            return Provider.GetRequiredService<TQuery>();
        }
    }
}