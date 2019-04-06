using aska.core.common;
using aska.core.infrastructure.data.CommandQuery.Command;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
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