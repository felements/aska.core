using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.DemoApp.Specification;
using Aska.Core.EntityStorage.Ef;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aska.Core.EntityStorage.DemoApp;

/// <summary>
/// Runs create, query and delete operations against every configured database, then stops the application.
/// </summary>
internal sealed class StorageDemoService(
    IEntityStorageContextInitializer contextInitializer,
    IQueryFactory queryFactory,
    ICommandFactory commandFactory,
    IHostApplicationLifetime lifetime,
    ILogger<StorageDemoService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Initializing contexts...");
        await contextInitializer.InitializeAsync(stoppingToken);

        await EntityCrudAsync(new MariaDbEntity("mariadb"), stoppingToken);
        await EntityCrudAsync(new PsqlEntity("postgresql"), stoppingToken);
        await EntityCrudAsync(new SqliteEntity("sqlite"), stoppingToken);

        logger.LogInformation("Demo finished.");
        lifetime.StopApplication();
    }

    private async Task EntityCrudAsync<T>(T entity, CancellationToken ct) where T : class, IEntity<Guid>
    {
        var entityName = typeof(T).Name;

        await commandFactory.GetCreateCommand<T>().ExecuteAsync(entity, ct);
        logger.LogInformation("{Entity}: created {Id}", entityName, entity.Id);

        var count = await queryFactory.GetQuery<T>().CountAsync(ct);
        logger.LogInformation("{Entity}: {Count} rows stored", entityName, count);

        var loaded = await queryFactory.GetQuery<T, ByIdExpressionSpecification<T, Guid>>()
            .Where(new ByIdExpressionSpecification<T, Guid>(entity.Id))
            .SingleAsync(ct);
        logger.LogInformation("{Entity}: loaded {Id} by id", entityName, loaded.Id);

        await commandFactory.GetDeleteCommand<T>().ExecuteAsync(entity, ct);
        logger.LogInformation("{Entity}: deleted {Id}", entityName, entity.Id);
    }
}
