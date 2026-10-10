using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp;

public class DemoSqliteContext : AutoDiscoverySqliteContext
{
    public DemoSqliteContext(
        IConnectionStringProvider<DemoSqliteContext> connectionStringProvider,
        ITypeDiscoveryProvider<DemoSqliteContext> typeDiscoveryProvider)
        : base(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover)
    {
    }

    /// <summary>
    /// Applies pending migrations when the application starts.
    /// </summary>
    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }
}

/// <summary>
/// Creates the context for <c>dotnet ef</c> commands.
/// </summary>
public class DemoSqliteContextDesignTimeFactory : IDesignTimeDbContextFactory<DemoSqliteContext>
{
    public DemoSqliteContext CreateDbContext(string[] args) => new(
        new StaticConnectionStringProvider<DemoSqliteContext>(DemoSettings.SqliteConnection),
        new TypeDiscoveryProvider<DemoSqliteContext>(
            new TypeDiscoveryOptions(typeof(ISqliteEntity), DemoSettings.EntitiesAssemblyPrefix, forceLoadAssemblies: false),
            new TypeDiscoveryProvider()));
}
