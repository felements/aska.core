using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<AutoDiscoveryMariaDbContext>
    {
        public AutoDiscoveryMariaDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time context factory");
            Console.WriteLine(string.Join(", ", args));

            return new AutoDiscoveryMariaDbContext(
                new StaticConnectionStringProvider<AutoDiscoveryMariaDbContext>(MariaDbConnectionString.Create()
                    .WithServer("localhost")
                    .WithDatabase("askaone")
                    .WithUser("askaone")
                    .WithPassword("askaone")),
                new TypeDiscoveryProvider<AutoDiscoveryMariaDbContext>(
                    new TypeDiscoveryOptions(typeof(IMariaDbEntity), "Aska", true),
                    new TypeDiscoveryProvider()));
        }
    }
}