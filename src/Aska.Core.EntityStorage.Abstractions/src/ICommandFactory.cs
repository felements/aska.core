namespace Aska.Core.EntityStorage.Abstractions
{
    public interface ICommandFactory
    {
        TCommand GetCommand<TEntity, TCommand, TResult>()
            where TCommand : ICommand<TEntity, TResult>;

        T GetCommand<T>() where T : ICommandBase;

        ICreateCommand<T> GetCreateCommand<T>() where T : class;
        IDeleteCommand<T> GetDeleteCommand<T>() where T : class;
        IUpdateCommand<T> GetUpdateCommand<T>() where T : class;
        
        
        IBulkCreateCommand<T> GetBulkCreateCommand<T>() where T : class;
        IBulkDeleteCommand<T> GetBulkDeleteCommand<T>() where T : class;
        IBulkUpdateCommand<T> GetBulkUpdateCommand<T>() where T : class;
    }
}