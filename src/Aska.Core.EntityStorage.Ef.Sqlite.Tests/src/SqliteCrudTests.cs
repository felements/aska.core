using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Tests;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.Sqlite.Tests;

public sealed class SqliteCrudTests(SqliteFixture fixture)
    : EntityStorageCrudTests<SqliteFixture, TestSqliteContext>(fixture), IClassFixture<SqliteFixture>;

public sealed class SqliteFixture : EntityStorageFixture<TestSqliteContext>
{
    private readonly string _databaseFile = Path.Combine(Path.GetTempPath(), $"aska-entitystorage-{Guid.NewGuid():N}.db");

    protected override Task<string> StartDatabaseAsync(CancellationToken cancellationToken) =>
        Task.FromResult<string>(SqliteConnectionString.Create().WithDataFile(_databaseFile));

    protected override void ConfigureStorage(EfEntityStorageConfigurationBuilder storage, string connectionString) =>
        storage.UseSqlite<TestSqliteContext>()
            .WithEntitiesAutoDiscovery<ITestEntity>(TestEntitiesAssemblyPrefix)
            .WithConnectionString(connectionString);

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        SqliteConnection.ClearAllPools();
        File.Delete(_databaseFile);
    }
}

public class TestSqliteContext(
    IConnectionStringProvider<TestSqliteContext> connectionString,
    ITypeDiscoveryProvider<TestSqliteContext> entityTypes)
    : AutoDiscoverySqliteContext(connectionString.GetConnectionString, entityTypes.Discover);
