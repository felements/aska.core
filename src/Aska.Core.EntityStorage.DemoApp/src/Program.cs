using Aska.Core.EntityStorage.DemoApp;
using Aska.Core.EntityStorage.Ef;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Aska.Core.EntityStorage.Ef.PostgreSql;
using Aska.Core.EntityStorage.Ef.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Start the databases first: `docker compose up -d` from the repository root.
var builder = Host.CreateApplicationBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services
    .AddHostedService<StorageDemoService>()
    .AddEfEntityStorage(storage =>
    {
        storage.UseMariaDb<DemoMariaDbContext>()
            .WithEntitiesAutoDiscovery<IMariaDbEntity>(DemoSettings.EntitiesAssemblyPrefix)
            .WithConnectionString(DemoSettings.MariaDbConnection);

        storage.UsePostgresql<DemoPsqlContext>()
            .WithEntitiesAutoDiscovery<IPsqlEntity>(DemoSettings.EntitiesAssemblyPrefix)
            .WithConnectionString(DemoSettings.PostgreSqlConnection);

        storage.UseSqlite<DemoSqliteContext>()
            .WithEntitiesAutoDiscovery<ISqliteEntity>(DemoSettings.EntitiesAssemblyPrefix)
            .WithConnectionString(DemoSettings.SqliteConnection);
    });

await builder.Build().RunAsync();
