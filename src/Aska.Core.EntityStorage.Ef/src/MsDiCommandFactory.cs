using System;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage.Ef
{
    public class MsDiCommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _provider;

        public MsDiCommandFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public TCommand GetCommand<TEntity, TCommand, TResult>() 
            where TCommand : ICommand<TEntity, TResult>
        {
            return _provider.GetRequiredService<TCommand>();
        }

        public T GetCommand<T>() where T : ICommandBase
        {
            return _provider.GetRequiredService<T>();
        }

        public ICreateCommand<T> GetCreateCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public IDeleteCommand<T> GetDeleteCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public IUpdateCommand<T> GetUpdateCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public IBulkCreateCommand<T> GetBulkCreateCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public IBulkDeleteCommand<T> GetBulkDeleteCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public IBulkUpdateCommand<T> GetBulkUpdateCommand<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
        
        // todo: interceptors for fake-deleted entities
        // todo: interceptors for modification-time-tracked entities
    }
}