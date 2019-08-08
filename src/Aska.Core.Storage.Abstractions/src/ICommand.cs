using System.Threading;
using System.Threading.Tasks;

namespace Aska.Core.Storage.Abstractions
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }

    public interface ICommand<in T>
    {
        Task ExecuteAsync(T context, CancellationToken ct);
    }

    public interface ICreateCommand<in T> : ICommand<T> {}
    public interface IUpdateCommand<in T> : ICommand<T> {}
    public interface IDeleteCommand<in T> : ICommand<T> {}
}