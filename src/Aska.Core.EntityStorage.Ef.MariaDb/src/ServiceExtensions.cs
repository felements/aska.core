using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.Storage.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aska.Core.EntityStorage.Ef.MariaDb
{
    public static class ServiceExtensions
    {
        /// <summary>
        ///  Use specified MariaDb context
        /// </summary>
        /// <typeparam name="TContext">Type of the context</typeparam>
        /// <returns>MariaDb context configuration builder</returns>
        public static MariaDbEntityStorageConfigurationBuilder<TContext> UseMariaDb<TContext>(
            this EfEntityStorageConfigurationBuilder builder)
            where TContext : DbContext, IEntityStorageContext
        {
            return new MariaDbEntityStorageConfigurationBuilder<TContext>(builder.Services);
        }
        
        /// <summary>
        /// Use default MariaDb context with entity type auto-discovery
        /// </summary>
        /// <returns>MariaDb context configuration builder</returns>
        public static MariaDbEntityStorageConfigurationBuilder<AutoDiscoveryMariaDbContext> UseMariaDb(this EfEntityStorageConfigurationBuilder builder)
        {
            return new MariaDbEntityStorageConfigurationBuilder<AutoDiscoveryMariaDbContext>(builder.Services);
        }
        
        //todo: manually created context configuration methods
        
        public class MariaDbEntityStorageConfigurationBuilder<TContext>
            where TContext : DbContext, IEntityStorageContext
        {
            private readonly IServiceCollection _services;
            
            public MariaDbEntityStorageConfigurationBuilder(IServiceCollection services)
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
            /// <returns>MariaDb context configuration builder</returns>
            public MariaDbEntityStorageConfigurationBuilder<TContext> WithEntitiesAutoDiscovery<TBaseEntity>(string assemblyNamePrefix, bool forceLoadAssemblies = false) 
            {
                _services.AddSingleton<ITypeDiscoveryProvider<TContext>, TypeDiscoveryProvider<TContext>>(
                    pr => new TypeDiscoveryProvider<TContext>(new TypeDiscoveryOptions(
                            typeof(TBaseEntity),
                            assemblyNamePrefix,
                            forceLoadAssemblies),
                        pr.GetRequiredService<ITypeDiscoveryProvider>()));
                
                // register entities
                foreach (var entityType in new TypeDiscoveryProvider().Discover(typeof(TBaseEntity), assemblyNamePrefix, forceLoadAssemblies) )
                {
                    var readerInterfaceType = typeof(IEntityStorageReader<>).MakeGenericType(entityType);
                    var writerInterfaceType = typeof(IEntityStorageWriter<>).MakeGenericType(entityType);

                    var readerProxyType = typeof(EntityStorageReaderContextProxy<,>).MakeGenericType(entityType, typeof(TContext));
                    var writerProxyType = typeof(EntityStorageWriterContextProxy<,>).MakeGenericType(entityType, typeof(TContext));

                    _services.AddTransient(readerInterfaceType, readerProxyType);
                    _services.AddTransient(writerInterfaceType, writerProxyType);
                }
                
                return this;
            }
            
            /// <summary>
            /// Specify MariaDb connection string
            /// </summary>
            /// <param name="connectionString"></param>
            /// <returns></returns>
            public MariaDbEntityStorageConfigurationBuilder<TContext> WithConnectionString(string connectionString)
            {
                _services.AddSingleton<IConnectionStringProvider<TContext>, StaticConnectionStringProvider<TContext>>(
                        provider => new StaticConnectionStringProvider<TContext>(connectionString));

                return this;
            }
        }
    }
}