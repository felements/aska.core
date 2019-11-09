using System.Threading.Tasks;
using Aska.Core.EntityStorage.Ef.MariaDb;
using Aska.Core.Storage.Ef;
using Aska.Core.Storage.Ef.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        .AddHostedService<StorageService>()
                        .AddEntityStorage(opt =>
                        {
                            opt.UseMariaDb()
                                .WithEntitiesAutoDiscovery<IMariaDbEntity>("Aska.Core.EntityStorage.Demo", true)
                                .WithConnectionString(
                                    MariaDbConnectionString.Create()
                                        .WithServer("localhost")
                                        .WithDatabase("askaone")
                                        .WithUser("askaone")
                                        .WithPassword("askaone"));
                            
                            opt.UseSqlite()
                                .WithEntitiesAutoDiscovery<ISqliteEntity>("Aska.Core.EntityStorage.Demo", true)
                                .WithConnectionString("");//todo
                        });
                })
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    //todo: connection options
                });
    }
}