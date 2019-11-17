using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.Storage.Ef.Sqlite;
using Microsoft.EntityFrameworkCore;
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
        
        public override async  Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var pendingMigrations = await Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any()) throw new Exception("There are some pending migrations. Apply them first before using the context."); 
                
            await base.InitializeAsync(cancellationToken);
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