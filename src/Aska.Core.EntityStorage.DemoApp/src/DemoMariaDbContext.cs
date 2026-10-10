using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp;

public class DemoMariaDbContext : AutoDiscoveryMariaDbContext
{
    public DemoMariaDbContext(
        IConnectionStringProvider<DemoMariaDbContext> connectionStringProvider,
        ITypeDiscoveryProvider<DemoMariaDbContext> typeDiscoveryProvider)
        : base(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover)
    {
    }

    // A fixed server version lets design-time tools run without a live server.
    internal DemoMariaDbContext(
        IConnectionStringProvider<DemoMariaDbContext> connectionStringProvider,
        ITypeDiscoveryProvider<DemoMariaDbContext> typeDiscoveryProvider,
        ServerVersion serverVersion)
        : base(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover, serverVersion)
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
public class DemoMariaDbContextDesignTimeFactory : IDesignTimeDbContextFactory<DemoMariaDbContext>
{
    public DemoMariaDbContext CreateDbContext(string[] args) => new(
        new StaticConnectionStringProvider<DemoMariaDbContext>(DemoSettings.MariaDbConnection),
        new TypeDiscoveryProvider<DemoMariaDbContext>(
            new TypeDiscoveryOptions(typeof(IMariaDbEntity), DemoSettings.EntitiesAssemblyPrefix, forceLoadAssemblies: false),
            new TypeDiscoveryProvider()),
        new MariaDbServerVersion(new Version(11, 8)));
}
