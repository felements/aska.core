using System.Threading.Tasks;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Aska.Core.Storage.Ef;
using Aska.Core.Storage.Ef.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aska.Core.EntityStorage.DemoApp
{
    class Program
    {
        static Task Main(string[] args) => CreateConsoleHost(args).UseConsoleLifetime().Build().RunAsync();

        private static IHostBuilder CreateConsoleHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services
                        .AddHostedService<StorageDemoService>()
                        .AddEntityStorage(opt =>
                        {
                            opt.UseMariaDb<DemoMariaDbContext>()
                                .WithEntitiesAutoDiscovery<IMariaDbEntity>("Aska.Core.EntityStorage.Demo", true)
                                .WithConnectionString(
                                    MariaDbConnectionString.Create()
                                        .WithServer("localhost")
                                        .WithDatabase("askaone")
                                        .WithUser("askaone")
                                        .WithPassword("askaone"));
                            
                            opt.UseSqlite<DemoSqliteContext>()
                                .WithEntitiesAutoDiscovery<ISqliteEntity>("Aska.Core.EntityStorage.Demo", true)
                                .WithConnectionString(SqliteConnectionString.Create()
                                    .WithDataFile("./../../../demo.db"));
                        });
                })
                .ConfigureLogging(options => 
                    options.AddConsole());
    }
}