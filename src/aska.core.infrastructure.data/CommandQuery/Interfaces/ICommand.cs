using System.Threading.Tasks;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }

    public interface ICommand<in T>
    {
        Task ExecuteAsync(T context);
    }
}