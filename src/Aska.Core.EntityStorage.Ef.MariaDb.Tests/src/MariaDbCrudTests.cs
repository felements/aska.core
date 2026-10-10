using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Tests;
using Testcontainers.MariaDb;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests;

public sealed class MariaDbCrudTests(MariaDbFixture fixture)
    : EntityStorageCrudTests<MariaDbFixture, TestMariaDbContext>(fixture), IClassFixture<MariaDbFixture>;

public sealed class MariaDbFixture : EntityStorageFixture<TestMariaDbContext>
{
    private readonly MariaDbContainer _container = new MariaDbBuilder("mariadb:11.8").Build();

    protected override async Task<string> StartDatabaseAsync(CancellationToken cancellationToken)
    {
        await _container.StartAsync(cancellationToken);
        return _container.GetConnectionString();
    }

    protected override void ConfigureStorage(EfEntityStorageConfigurationBuilder storage, string connectionString) =>
        storage.UseMariaDb<TestMariaDbContext>()
            .WithEntitiesAutoDiscovery<ITestEntity>(TestEntitiesAssemblyPrefix)
            .WithConnectionString(connectionString);

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _container.DisposeAsync();
    }
}

public class TestMariaDbContext(
    IConnectionStringProvider<TestMariaDbContext> connectionString,
    ITypeDiscoveryProvider<TestMariaDbContext> entityTypes)
    : AutoDiscoveryMariaDbContext(connectionString.GetConnectionString, entityTypes.Discover);
