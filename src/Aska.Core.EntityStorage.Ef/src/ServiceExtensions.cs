using System;
using System.Runtime.CompilerServices;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Aska.Core.EntityStorage.Ef.MariaDb.Tests")]
[assembly: InternalsVisibleTo("Aska.Core.EntityStorage.Ef.Tests")]

namespace Aska.Core.Storage.Ef
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddEfEntityStorage(this IServiceCollection services, Action<EfEntityStorageConfigurationBuilder> configure)
        {
            configure(new EfEntityStorageConfigurationBuilder(services));

            services.AddTransient<IEntityStorageContextInitializer, EntityStorageContextInitializer>();
            services.AddTransient<ITypeDiscoveryProvider, TypeDiscoveryProvider>();

            services.AddTransient<IQueryFactory, MsDiQueryFactory>(provider => new MsDiQueryFactory(provider));
            services.AddTransient<ICommandFactory, MsDiCommandFactory>(provider => new MsDiCommandFactory(provider));
            services.AddTransient(typeof(IQuery<,>), typeof(EfDatabaseNoTrackingQuery<,>));
            services.AddTransient(typeof(IQuery<>), typeof(EfDatabaseNoTrackingQuery<>));
            
            services.AddTransient(typeof(ICreateCommand<>), typeof(EfDatabaseCreateCommand<>));
            services.AddTransient(typeof(IUpdateCommand<>), typeof(EfDatabaseUpdateCommand<>));
            services.AddTransient(typeof(IDeleteCommand<>), typeof(EfDatabaseDeleteCommand<>));
            
            services.AddTransient(typeof(IBulkCreateCommand<>), typeof(EfDatabaseCreateCommand<>));
            services.AddTransient(typeof(IBulkUpdateCommand<>), typeof(EfDatabaseUpdateCommand<>));
            services.AddTransient(typeof(IBulkDeleteCommand<>), typeof(EfDatabaseDeleteCommand<>));
            
            return services;
        }
        
        public static void RegisterEntityStorageContextProxies<TBaseEntity, TContext>(
            IServiceCollection services,
            string assemblyNamePrefix,
            bool forceLoadAssemblies)
        {
            // register entities
            foreach (var entityType in new TypeDiscoveryProvider().Discover(typeof(TBaseEntity), assemblyNamePrefix,
                forceLoadAssemblies))
            {
                var readerInterfaceType = typeof(IEntityStorageReader<>).MakeGenericType(entityType);
                var writerInterfaceType = typeof(IEntityStorageWriter<>).MakeGenericType(entityType);

                var readerProxyType = typeof(EntityStorageReaderContextProxy<,>).MakeGenericType(entityType, typeof(TContext));
                var writerProxyType = typeof(EntityStorageWriterContextProxy<,>).MakeGenericType(entityType, typeof(TContext));

                services.AddTransient(readerInterfaceType, readerProxyType);
                services.AddTransient(writerInterfaceType, writerProxyType);
            }
        }
    }

    public class EfEntityStorageConfigurationBuilder
    {
        public readonly IServiceCollection Services;

        public EfEntityStorageConfigurationBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}