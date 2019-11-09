namespace Aska.Core.EntityStorage.Abstractions
{
    public class StaticConnectionStringProvider<TContext> : IConnectionStringProvider<TContext>
        where TContext : IStorageContext
    {
        private readonly string _connectionString;

        public StaticConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString() => _connectionString;
    }
}