using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.EntityStorage.Abstractions
{
    public interface ICommandBase
    {
    }

    public interface ICommand<in TContext, TResult> : ICommandBase
    {
        Task<TResult> ExecuteAsync(TContext context, CancellationToken ct = default);
    }

    public interface IBulkCommand<in TContext, TResult> : ICommandBase
    {
        Task<TResult> ExecuteAsync(IEnumerable<TContext> context, CancellationToken ct = default);
    }

    public interface ICreateCommand<in TIn> : ICommand<TIn, bool> {}
    public interface IUpdateCommand<in TIn> : ICommand<TIn, bool> {}
    public interface IDeleteCommand<in TIn> : ICommand<TIn, bool> {}
    
    public interface IBulkCreateCommand<in TIn> : IBulkCommand<TIn, int> {}
    public interface IBulkUpdateCommand<in TIn> : IBulkCommand<TIn, int> {}
    public interface IBulkDeleteCommand<in TIn> : IBulkCommand<TIn, int> {}
}