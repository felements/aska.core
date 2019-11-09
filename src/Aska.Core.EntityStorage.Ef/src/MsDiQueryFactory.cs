using System;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage.Ef
{
    internal class MsDiQueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _provider;

        public MsDiQueryFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>() 
            where TEntity : class
        {
            return _provider.GetRequiredService<IQuery<TEntity, IExpressionSpecification<TEntity>>>();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>() 
            where TEntity : class 
            where TSpecification : ISpecification<TEntity>
        {
            return _provider.GetRequiredService<IQuery<TEntity, TSpecification>>();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>() 
            where TEntity : class 
            where TSpecification : ISpecification<TEntity> 
            where TQuery : IQuery<TEntity, TSpecification>
        {
            return _provider.GetRequiredService<TQuery>();
        }
    }
}