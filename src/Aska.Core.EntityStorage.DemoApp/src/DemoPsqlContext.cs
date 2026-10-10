using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aska.Core.EntityStorage.DemoApp;

public class DemoPsqlContext : AutoDiscoveryPostgresqlContext
{
    public DemoPsqlContext(
        IConnectionStringProvider<DemoPsqlContext> connectionStringProvider,
        ITypeDiscoveryProvider<DemoPsqlContext> typeDiscoveryProvider)
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
public class DemoPsqlContextDesignTimeFactory : IDesignTimeDbContextFactory<DemoPsqlContext>
{
    public DemoPsqlContext CreateDbContext(string[] args) => new(
        new StaticConnectionStringProvider<DemoPsqlContext>(DemoSettings.PostgreSqlConnection),
        new TypeDiscoveryProvider<DemoPsqlContext>(
            new TypeDiscoveryOptions(typeof(IPsqlEntity), DemoSettings.EntitiesAssemblyPrefix, forceLoadAssemblies: false),
            new TypeDiscoveryProvider()));
}
