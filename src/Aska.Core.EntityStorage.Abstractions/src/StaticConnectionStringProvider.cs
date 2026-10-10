namespace Aska.Core.EntityStorage.Abstractions;

public class StaticConnectionStringProvider<TContext>(string connectionString) : IConnectionStringProvider<TContext>
    where TContext : IStorageContext
{
    public string GetConnectionString() => connectionString;
}
