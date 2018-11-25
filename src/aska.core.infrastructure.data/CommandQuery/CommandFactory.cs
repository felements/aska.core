using aska.core.common.Data.Entity;
using aska.core.infrastructure.data.CommandQuery.Command;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using Autofac;

namespace aska.core.infrastructure.data.CommandQuery
{
    public class CommandFactory : ICommandFactory
    {
        protected ILifetimeScope Scope;

        public CommandFactory(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public TCommand GetCommand<TEntity, TCommand>() where TCommand : ICommand<TEntity>
        {
            return Scope.Resolve<TCommand>();
        }

        public T GetCommand<T>() where T : ICommand
        {
            return Scope.Resolve<T>();
        }

        public CreateEntityCommand<T> GetCreateCommand<T>() where T : class, IEntity
        {
            return new CreateEntityCommand<T>(Scope);
        }

        public DeleteEntityCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            return new DeleteEntityCommand<T>(Scope);
        }

        public UpdateEntityCommand<T> GetUpdateCommand<T>() where T : class, IEntity
        {
            return new UpdateEntityCommand<T>(Scope);
        }
    }
}