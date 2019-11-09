using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.PostgreSql;
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
    }
    
    public class DesignTimePsqlCtxFactory : IDesignTimeDbContextFactory<DemoPsqlContext>
    {
        public DesignTimePsqlCtxFactory()
        {
        }
        
        public DemoPsqlContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time Sqlite context factory");
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