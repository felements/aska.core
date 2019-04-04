using System;
using aska.core.infrastructure.data.Model;
using aska.core.infrastructure.data.CommandQuery.Command;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.CommandQuery
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _scope;

        public CommandFactory(IServiceProvider scope)
        {
            _scope = scope;
        }

        public TCommand GetCommand<TEntity, TCommand>() where TCommand : ICommand<TEntity>
        {
            return _scope.GetRequiredService<TCommand>();
        }

        public T GetCommand<T>() where T : ICommand
        {
            return _scope.GetRequiredService<T>();
        }

        public CreateEntityCommand<T> GetCreateCommand<T>() where T : class, IEntity
        {
            return new CreateEntityCommand<T>(_scope);
        }

        public DeleteEntityCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            return new DeleteEntityCommand<T>(_scope);
        }

        public UpdateEntityCommand<T> GetUpdateCommand<T>() where T : class, IEntity
        {
            return new UpdateEntityCommand<T>(_scope);
        }
    }
}