namespace Aska.Core.Storage.Abstractions
{
    public interface ICommandFactory
    {
        TCommand GetCommand<TEntity, TCommand>()
            where TCommand : ICommand<TEntity>;

        T GetCommand<T>()
            where T : ICommand;

        ICreateCommand<T> GetCreateCommand<T>()
            where T : class;

        IDeleteCommand<T> GetDeleteCommand<T>()
            where T : class;

        IUpdateCommand<T> GetUpdateCommand<T>()
            where T : class;
    }
}