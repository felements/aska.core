using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public class TestMariaDbContext : AutoDiscoveryMariaDbContext, IEntityStorageWriter
    {
        public TestMariaDbContext() : base(
            new TestMariaDbConnectionStringProvider().GetConnectionString,
            new TestEntityTypeDiscovery<TestMariaDbContext>("Aska.Core.EntityStorage.Ef").Discover)
        {
        }

        public override async  Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var pendingMigrations = await Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any()) throw new Exception("There are some pending migrations. Apply them first before using the context."); 
                
            await base.InitializeAsync(cancellationToken);
        }
    }
    
    public class DesignTimeMariaDbCtxFactory : IDesignTimeDbContextFactory<TestMariaDbContext>
    {
        public DesignTimeMariaDbCtxFactory()
        {
        }
        
        public TestMariaDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time MariaDb context factory");
            Console.WriteLine(string.Join(", ", args));

            return new TestMariaDbContext();
        }
    }

    public class TestMariaDbConnectionStringProvider : StaticConnectionStringProvider<TestMariaDbContext>
    {
        public TestMariaDbConnectionStringProvider() : base(
            MariaDbConnectionString
                .Create()
                .WithServer("localhost")
                .WithDatabase("askaone")
                .WithUser("askaone")
                .WithPassword("askaone"))
        {
        }
    }

    public class TestEntityTypeDiscovery<TDbContext> : TypeDiscoveryProvider<TDbContext> where TDbContext : IStorageContext
    {
        public TestEntityTypeDiscovery(string assemblyNamePrefix) 
            : base(new TypeDiscoveryOptions(typeof(ITestEntity<Guid>), assemblyNamePrefix, false), new TypeDiscoveryProvider())
        {
        }
    }
}


