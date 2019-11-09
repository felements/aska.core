using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.Storage.Ef.Sqlite;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp
{
    public class DemoSqliteContext : AutoDiscoverySqliteContext
    {
        public DemoSqliteContext(
            IConnectionStringProvider<DemoSqliteContext> connectionStringProvider,
            ITypeDiscoveryProvider<DemoSqliteContext> typeProvider) 
            : base(connectionStringProvider.GetConnectionString, typeProvider.Discover)
        {
        }
    }
    
    public class DesignTimeSqliteCtxFactory : IDesignTimeDbContextFactory<DemoSqliteContext>
    {
        public DesignTimeSqliteCtxFactory()
        {
        }
        
        public DemoSqliteContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time Sqlite context factory");
            Console.WriteLine(string.Join(", ", args));

            return new DemoSqliteContext(
                new StaticConnectionStringProvider<DemoSqliteContext>(
                    SqliteConnectionString.Create().WithDataFile("demo.db")),
                new TypeDiscoveryProvider<DemoSqliteContext>(
                    new TypeDiscoveryOptions(typeof(ISqliteEntity), "Aska", true),
                    new TypeDiscoveryProvider()));
        }
    }
}