using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.DemoApp.Specification;
using Aska.Core.EntityStorage.Ef;
using Aska.Core.Storage.Ef;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aska.Core.EntityStorage.DemoApp
{
    internal sealed class StorageDemoService: IHostedService
    {
        private readonly IEntityStorageContextInitializer _contextInitializer;
        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<StorageDemoService> _logger;

        private CancellationTokenSource _workloadCancellation;

        public StorageDemoService(
            IEntityStorageContextInitializer contextInitializer, 
            IQueryFactory queryFactory,
            ICommandFactory commandFactory,
            IHostApplicationLifetime lifetime,//todo: remove workaround
            ILogger<StorageDemoService> logger)
        {
            _contextInitializer = contextInitializer;
            _queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _lifetime = lifetime;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Initializing contexts...");
            await _contextInitializer.InitializeAsync(cancellationToken);
            
#pragma warning disable 4014
            _logger.LogDebug("Starting workload in background...");
            
            _workloadCancellation = new CancellationTokenSource();
            Task.Factory.StartNew(() => Workload(_workloadCancellation.Token), _workloadCancellation.Token);
#pragma warning restore 4014
            
            _logger.LogInformation("Storage service started.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping workload...");
            _workloadCancellation?.Cancel();
            
            _logger.LogInformation("Storage service stopped.");
            return Task.CompletedTask;
        }

        private async Task Workload(CancellationToken ct = default)
        {
            await EntityCrud(_queryFactory, _commandFactory, () => new MariaDbEntity("mariaDb"), _logger, ct);
            await EntityCrud(_queryFactory, _commandFactory, () => new SqliteEntity("sqlite"), _logger, ct);
            await EntityCrud(_queryFactory, _commandFactory, () => new PsqlEntity("psql"), _logger, ct);
            
            _lifetime.StopApplication();
        }

        private static async Task EntityCrud<T>(
            IQueryFactory queryFactory,
            ICommandFactory commandFactory,
            Func<T> entityFactory,
            ILogger logger,
            CancellationToken ct = default) where T : class, IEntity<Guid>
        {
            logger.LogDebug($"{typeof(T).Name} - CRUD operations");

            var entity = entityFactory();
            
            logger.LogDebug("Creating entity...");
            var succeed = await commandFactory.GetCreateCommand<T>().ExecuteAsync(entity, ct);

            logger.LogDebug("Querying all entities...");
            var all = await queryFactory.GetQuery<T>().AllAsync(ct);

            logger.LogDebug("Querying entity by id ...");
            var loadedById = await queryFactory.GetQuery<T, ByIdExpressionSpecification<T, Guid>>()
                .Where(new ByIdExpressionSpecification<T, Guid>(entity.Id))
                .SingleAsync(ct);

            logger.LogDebug("Deleting entity...");
            await commandFactory.GetDeleteCommand<T>().ExecuteAsync(entity, ct);
        }
    }
}