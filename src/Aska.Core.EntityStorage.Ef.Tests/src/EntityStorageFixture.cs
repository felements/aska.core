using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.Tests;

/// <summary>
/// Starts a database, registers the entity storage for it through the public DI extensions,
/// and creates the schema for the test entities.
/// </summary>
public abstract class EntityStorageFixture<TContext> : IAsyncLifetime
    where TContext : DbContext
{
    /// <summary>
    /// Assembly name prefix that entity discovery scans for test entities.
    /// </summary>
    public const string TestEntitiesAssemblyPrefix = "Aska.Core.EntityStorage.Ef.Tests";

    private ServiceProvider? _services;

    protected IServiceProvider Services =>
        _services ?? throw new InvalidOperationException("The fixture has not been initialized.");

    public IQueryFactory QueryFactory => Services.GetRequiredService<IQueryFactory>();

    public ICommandFactory CommandFactory => Services.GetRequiredService<ICommandFactory>();

    /// <summary>
    /// Starts the database and returns a connection string for it.
    /// </summary>
    protected abstract Task<string> StartDatabaseAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Registers the provider-specific storage for <typeparamref name="TContext"/>.
    /// </summary>
    protected abstract void ConfigureStorage(EfEntityStorageConfigurationBuilder storage, string connectionString);

    public async ValueTask InitializeAsync()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var connectionString = await StartDatabaseAsync(cancellationToken);

        _services = new ServiceCollection()
            .AddEfEntityStorage(storage => ConfigureStorage(storage, connectionString))
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        await using var scope = _services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (_services is not null)
        {
            await _services.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
