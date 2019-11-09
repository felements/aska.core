namespace Aska.Core.EntityStorage.Abstractions
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }
    
    public interface IConnectionStringProvider<TContext>: IConnectionStringProvider 
        where TContext: IStorageContext
    {
    }
}