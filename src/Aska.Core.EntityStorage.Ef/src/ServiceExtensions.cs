using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.EntityStorage.Ef;

public static class ServiceExtensions
{
    public static IServiceCollection AddEfEntityStorage(this IServiceCollection services, Action<EfEntityStorageConfigurationBuilder> configure)
    {
        configure(new EfEntityStorageConfigurationBuilder(services));

        services.AddTransient<IEntityStorageContextInitializer, EntityStorageContextInitializer>();
        services.AddTransient<ITypeDiscoveryParameterizedProvider, TypeDiscoveryProvider>();

        services.AddTransient<IQueryFactory, MsDiQueryFactory>();
        services.AddTransient<ICommandFactory, MsDiCommandFactory>();
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

    /// <summary>
    /// Registers an <see cref="IEntityStorageReader{T}"/> and <see cref="IEntityStorageWriter{T}"/> proxy, bound to
    /// <typeparamref name="TContext"/>, for every exported type implementing <typeparamref name="TBaseEntity"/>
    /// in the assemblies whose name starts with <paramref name="assemblyNamePrefix"/>.
    /// </summary>
    public static void RegisterEntityStorageContextProxies<TBaseEntity, TContext>(
        IServiceCollection services,
        string assemblyNamePrefix,
        bool forceLoadAssemblies)
    {
        var entityTypes = new TypeDiscoveryProvider().Discover(typeof(TBaseEntity), assemblyNamePrefix, forceLoadAssemblies);
        foreach (var entityType in entityTypes)
        {
            services.AddTransient(
                typeof(IEntityStorageReader<>).MakeGenericType(entityType),
                typeof(EntityStorageReaderContextProxy<,>).MakeGenericType(entityType, typeof(TContext)));
            services.AddTransient(
                typeof(IEntityStorageWriter<>).MakeGenericType(entityType),
                typeof(EntityStorageWriterContextProxy<,>).MakeGenericType(entityType, typeof(TContext)));
        }
    }
}

public class EfEntityStorageConfigurationBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
}
