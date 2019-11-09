using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp
{
    public class DemoMariaDbContext : AutoDiscoveryMariaDbContext
    {
        public DemoMariaDbContext(
            IConnectionStringProvider<DemoMariaDbContext> connectionStringProvider,
            ITypeDiscoveryProvider<DemoMariaDbContext> typeDiscoveryProvider)
            : base(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover)
        {
        }
    }
    
    public class DesignTimeMariaDbCtxFactory : IDesignTimeDbContextFactory<DemoMariaDbContext>
    {
        public DesignTimeMariaDbCtxFactory()
        {
        }
        
        public DemoMariaDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time MariaDb context factory");
            Console.WriteLine(string.Join(", ", args));

            return new DemoMariaDbContext(
                new StaticConnectionStringProvider<DemoMariaDbContext>(MariaDbConnectionString
                    .Create()
                    .WithServer("localhost")
                    .WithDatabase("askaone")
                    .WithUser("askaone")
                    .WithPassword("askaone")),
                new TypeDiscoveryProvider<DemoMariaDbContext>(
                    new TypeDiscoveryOptions(typeof(IMariaDbEntity), "Aska", true),
                    new TypeDiscoveryProvider()));
        }
    }
}


