namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in T>
    {
        void Execute(T context);
    }
}