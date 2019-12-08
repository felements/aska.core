using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.Ef.PostgreSql.Tests
{
    public class TestPostgresqlContext : AutoDiscoveryPostgresqlContext, IEntityStorageWriter
    {
        public TestPostgresqlContext() : base(
            new TestMariaDbConnectionStringProvider().GetConnectionString,
            new TestEntityTypeDiscovery<TestPostgresqlContext>("Aska.Core.EntityStorage.Ef").Discover)
        {
        }

        public override async  Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var pendingMigrations = await Database.GetAppliedMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any()) throw new Exception("There are some pending migrations. Apply them first before using the context."); 
                
            await base.InitializeAsync(cancellationToken);
        }
    }
    
    public class DesignTimeMariaDbCtxFactory : IDesignTimeDbContextFactory<TestPostgresqlContext>
    {
        public DesignTimeMariaDbCtxFactory()
        {
        }
        
        public TestPostgresqlContext CreateDbContext(string[] args)
        {
            Console.WriteLine(">>> Design time MariaDb context factory");
            Console.WriteLine(string.Join(", ", args));

            return new TestPostgresqlContext();
        }
    }

    public class TestMariaDbConnectionStringProvider : StaticConnectionStringProvider<TestPostgresqlContext>
    {
        public TestMariaDbConnectionStringProvider() : base(
            PsqlConnectionString
                .Create()
                .WithServer(Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
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


