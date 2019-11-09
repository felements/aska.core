using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.Storage.Ef;
using Microsoft.Extensions.Hosting;

namespace Aska.Core.EntityStorage.DemoApp
{
    internal sealed class StorageDemoService: IHostedService
    {
        private readonly IEntityStorageContextInitializer _contextInitializer;
        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;

        public StorageDemoService(
            IEntityStorageContextInitializer contextInitializer, 
            IQueryFactory queryFactory,
            ICommandFactory commandFactory)
        {
            _contextInitializer = contextInitializer;
            _queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _contextInitializer.InitializeAsync(cancellationToken);
            
            var maria = await _queryFactory
                .GetQuery<MariaDbEntity>()
                .AllAsync(cancellationToken);
            
            var sqlite = await _queryFactory
                .GetQuery<SqliteEntity>()
                .AllAsync(cancellationToken);

            var psql = await _queryFactory
                .GetQuery<PsqlEntity>()
                .AllAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}