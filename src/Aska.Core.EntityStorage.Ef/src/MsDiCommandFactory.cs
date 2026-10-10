using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.EntityStorage.Ef;

public class MsDiCommandFactory(IServiceProvider provider) : ICommandFactory
{
    public TCommand GetCommand<TEntity, TCommand, TResult>()
        where TCommand : ICommand<TEntity, TResult> => provider.GetRequiredService<TCommand>();

    public TCommand GetCommand<TCommand>() where TCommand : ICommandBase => provider.GetRequiredService<TCommand>();

    public ICreateCommand<T> GetCreateCommand<T>() where T : class => provider.GetRequiredService<ICreateCommand<T>>();

    public IDeleteCommand<T> GetDeleteCommand<T>() where T : class => provider.GetRequiredService<IDeleteCommand<T>>();

    public IUpdateCommand<T> GetUpdateCommand<T>() where T : class => provider.GetRequiredService<IUpdateCommand<T>>();

    public IBulkCreateCommand<T> GetBulkCreateCommand<T>() where T : class => provider.GetRequiredService<IBulkCreateCommand<T>>();

    public IBulkDeleteCommand<T> GetBulkDeleteCommand<T>() where T : class => provider.GetRequiredService<IBulkDeleteCommand<T>>();

    public IBulkUpdateCommand<T> GetBulkUpdateCommand<T>() where T : class => provider.GetRequiredService<IBulkUpdateCommand<T>>();
}
