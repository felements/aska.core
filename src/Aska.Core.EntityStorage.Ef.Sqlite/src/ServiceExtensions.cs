using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage.Ef.Sqlite
{
    public static class ServiceExtensions
    {
        /// <summary>
        ///  Use specified Sqlite context
        /// </summary>
        /// <typeparam name="TContext">Type of the context</typeparam>
        /// <returns>Sqlite context configuration builder</returns>
        public static SqliteEntityStorageConfigurationBuilder<TContext> UseSqlite<TContext>(
            this EfEntityStorageConfigurationBuilder builder)
            where TContext : DbContext, IEntityStorageContext
        {
            return new SqliteEntityStorageConfigurationBuilder<TContext>(builder.Services);
        }
        
        /// <summary>
        /// Use default Sqlite context with entity type auto-discovery
        /// </summary>
        /// <returns>Sqlite context configuration builder</returns>
        public static SqliteEntityStorageConfigurationBuilder<AutoDiscoverySqliteContext> UseSqlite(this EfEntityStorageConfigurationBuilder builder)
        {
            return new SqliteEntityStorageConfigurationBuilder<AutoDiscoverySqliteContext>(builder.Services);
        }
        
        //todo: manually created context configuration methods
        
        public class SqliteEntityStorageConfigurationBuilder<TContext>
            where TContext : DbContext, IEntityStorageContext
        {
            private readonly IServiceCollection _services;
            
            public SqliteEntityStorageConfigurationBuilder(IServiceCollection services)
            {
                _services = services;
                
                _services.AddTransient<TContext>();
                _services.AddTransient<IEntityStorageContext, TContext>();
                _services.AddTransient<IStorageContext, TContext>();
                _services.AddTransient<IEntityStorageReader, TContext>();
                _services.AddTransient<IEntityStorageWriter, TContext>();
                _services.AddTransient<IEntityStorageInitialize, TContext>();
                
                _services.AddDbContext<TContext>();
            }
            
            /// <summary>
            /// Discover entity types derived from the BaseType searching in the assemblies with names starting from 'assemblyNamePrefix'
            /// </summary>
            /// <param name="assemblyNamePrefix">Assemblies names prefix</param>
            /// <param name="forceLoadAssemblies">Force load assemblies if they still didn't load to the app domain.</param>
            /// <returns>Sqlite context configuration builder</returns>
            public SqliteEntityStorageConfigurationBuilder<TContext> WithEntitiesAutoDiscovery<TBaseEntity>(string assemblyNamePrefix, bool forceLoadAssemblies = false)
            {
                _services.AddSingleton<ITypeDiscoveryProvider<TContext>, TypeDiscoveryProvider<TContext>>(
                    pr => new TypeDiscoveryProvider<TContext>(new TypeDiscoveryOptions(
                            typeof(TBaseEntity),
                            assemblyNamePrefix,
                            forceLoadAssemblies),
                        pr.GetRequiredService<ITypeDiscoveryProvider>()));
                
                Aska.Core.Storage.Ef.ServiceExtensions.RegisterEntityStorageContextProxies<TBaseEntity, TContext>(
                    _services, assemblyNamePrefix, forceLoadAssemblies);
                return this;
            }
            
            /// <summary>
            /// Specify Sqlite connection string
            /// </summary>
            /// <param name="connectionString"></param>
            /// <returns></returns>
            public SqliteEntityStorageConfigurationBuilder<TContext> WithConnectionString(string connectionString)
            {
                _services
                    .AddSingleton<IConnectionStringProvider<TContext>, StaticConnectionStringProvider<TContext>>(
                        provider => new StaticConnectionStringProvider<TContext>(connectionString));

                return this;
            }
        }
    }
}