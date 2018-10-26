using Autofac;
using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Command;
using kd.infrastructure.CommandQuery.Interfaces;

namespace kd.infrastructure.CommandQuery
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