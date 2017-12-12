using aska.core.infrastructure.CommandQuery.Command;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface ICommandFactory
    {
        TCommand GetCommand<TEntity, TCommand>()
            where TCommand : ICommand<TEntity>;

        T GetCommand<T>()
            where T : ICommand;

        CreateEntityCommand<TEntity> GetCreateCommand<TEntity>()
            where TEntity : class, IEntity;

        DeleteEntityCommand<TEntity> GetDeleteCommand<TEntity>(bool forceDelete = false)
            where TEntity : class, IEntity;

        UpdateEntityCommand<TEntity> GetUpdateCommand<TEntity>()
            where TEntity : class, IEntity;
    }
}