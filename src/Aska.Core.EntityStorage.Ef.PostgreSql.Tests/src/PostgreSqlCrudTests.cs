using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Tests;
using Testcontainers.PostgreSql;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.PostgreSql.Tests;

public sealed class PostgreSqlCrudTests(PostgreSqlFixture fixture)
    : EntityStorageCrudTests<PostgreSqlFixture, TestPostgreSqlContext>(fixture), IClassFixture<PostgreSqlFixture>;

public sealed class PostgreSqlFixture : EntityStorageFixture<TestPostgreSqlContext>
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:18").Build();

    protected override async Task<string> StartDatabaseAsync(CancellationToken cancellationToken)
    {
        await _container.StartAsync(cancellationToken);
        return _container.GetConnectionString();
    }

    protected override void ConfigureStorage(EfEntityStorageConfigurationBuilder storage, string connectionString) =>
        storage.UsePostgresql<TestPostgreSqlContext>()
            .WithEntitiesAutoDiscovery<ITestEntity>(TestEntitiesAssemblyPrefix)
            .WithConnectionString(connectionString);

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _container.DisposeAsync();
    }
}

public class TestPostgreSqlContext(
    IConnectionStringProvider<TestPostgreSqlContext> connectionString,
    ITypeDiscoveryProvider<TestPostgreSqlContext> entityTypes)
    : AutoDiscoveryPostgresqlContext(connectionString.GetConnectionString, entityTypes.Discover);
