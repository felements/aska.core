using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.Storage.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _provider;

        public CommandFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TCommand GetCommand<TEntity, TCommand>() where TCommand : ICommand<TEntity>
        {
            return _provider.GetRequiredService<TCommand>();
        }

        public T GetCommand<T>() where T : ICommand
        {
            return _provider.GetRequiredService<T>();
        }

        public ICreateCommand<T> GetCreateCommand<T>() where T : class
        {
            return new CreateEntityCommand<T>(_provider);
        }

        public IDeleteCommand<T> GetDeleteCommand<T>() where T : class
        {
            return new DeleteEntityCommand<T>(_provider);
        }

        public IUpdateCommand<T> GetUpdateCommand<T>() where T : class
        {
            return new UpdateEntityCommand<T>(_provider);
        }
    }
}