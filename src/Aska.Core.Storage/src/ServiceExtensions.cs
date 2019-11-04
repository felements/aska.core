using System;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddStorage(
            this IServiceCollection services,
            Action<StorageConfigurationBuilder> configuration)
        {
            services
                .AddTransient<IQueryFactory, QueryFactory>()
                .AddTransient<ICommandFactory, CommandFactory>();
            configuration(new StorageConfigurationBuilder(services));
            
            return services;
        }
    }

    public class StorageConfigurationBuilder
    {
        public IServiceCollection Services;
        
        public StorageConfigurationBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}