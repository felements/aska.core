using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp
{
    public class DemoPsqlContext : AutoDiscoveryPostgresqlContext
    {
        public DemoPsqlContext(
            IConnectionStringProvider<DemoPsqlContext> connectionStringProvider,
            ITypeDiscoveryProvider<DemoPsqlContext> typeProvider) 
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
    
    public class DesignTimePsqlCtxFactory : IDesignTimeDbContextFactory<DemoPsqlContext>
    {
        public DesignTimePsqlCtxFactory()
        {
        }
        
        public DemoPsqlContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time Postgresql context factory");
            Console.WriteLine(string.Join(", ", args));

            return new DemoPsqlContext(
                new StaticConnectionStringProvider<DemoPsqlContext>(
                    PsqlConnectionString.Create()
                        .WithServer("localhost")
                        .WithDatabase("askaone")
                        .WithUser("askaone")
                        .WithPassword("askaone")),
                new TypeDiscoveryProvider<DemoPsqlContext>(
                    new TypeDiscoveryOptions(typeof(IPsqlEntity), "Aska", true),
                    new TypeDiscoveryProvider()));
        }
    }
}