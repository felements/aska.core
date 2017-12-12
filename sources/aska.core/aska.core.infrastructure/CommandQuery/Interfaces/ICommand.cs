using System.Collections.Generic;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in T>
    {
        void Execute(T context);
        void Execute(IEnumerable<T> context);
    }
}