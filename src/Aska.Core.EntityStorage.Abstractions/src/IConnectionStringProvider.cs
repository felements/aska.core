namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IConnectionStringProvider<TContext> where TContext: IStorageContext
    {
        string GetConnectionString();
    }
}