using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Command;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface ICommandFactory
    {
        TCommand GetCommand<TEntity, TCommand>()
            where TCommand : ICommand<TEntity>;

        T GetCommand<T>()
            where T : ICommand;

        CreateEntityCommand<T> GetCreateCommand<T>()
            where T : class, IEntity;

        DeleteEntityCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;

        UpdateEntityCommand<T> GetUpdateCommand<T>()
            where T : class, IEntity;
    }
}