using System.Collections.Concurrent;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.EntityStorage.Ef.MariaDb;

public class AutoDiscoveryMariaDbContext : EntityStorageContext
{
    private static readonly ConcurrentDictionary<string, ServerVersion> DetectedServerVersions = new();

    private readonly Func<string> _connectionStringProvider;
    private readonly Func<Type[]> _entityTypesProvider;
    private readonly ServerVersion? _serverVersion;

    /// <param name="connectionStringProvider">Returns the connection string.</param>
    /// <param name="entityTypesProvider">Returns the entity types to map.</param>
    /// <param name="serverVersion">
    /// The server version to target. When null, it is detected from the server once per connection string.
    /// Pass a fixed version where no server is reachable, such as in a design-time factory.
    /// </param>
    protected AutoDiscoveryMariaDbContext(
        Func<string> connectionStringProvider,
        Func<Type[]> entityTypesProvider,
        ServerVersion? serverVersion = null)
    {
        _connectionStringProvider = connectionStringProvider;
        _entityTypesProvider = entityTypesProvider;
        _serverVersion = serverVersion;
    }

    public AutoDiscoveryMariaDbContext(
        IConnectionStringProvider connectionStringProvider,
        ITypeDiscoveryProvider typeDiscoveryProvider)
        : this(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        var connectionString = _connectionStringProvider();
        var serverVersion = _serverVersion ?? DetectedServerVersions.GetOrAdd(connectionString, ServerVersion.AutoDetect);
        options.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.RegisterTypes(_entityTypesProvider());
    }
}

internal static class ModelBuilderExtensions
{
    public static void RegisterTypes(this ModelBuilder builder, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            builder.Entity(type).ToTable(type.Name);
        }
    }
}
