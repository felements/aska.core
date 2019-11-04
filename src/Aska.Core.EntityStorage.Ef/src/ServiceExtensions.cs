using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage.Ef
{
    public static class ServiceExtensions
    {
        public static StorageConfigurationBuilder UseEf(this StorageConfigurationBuilder builder)
        {
            builder.Services
                .AddTransient(typeof(IQuery<,>), typeof(EfDatabaseNoTrackingQuery<,>));
            return builder;
        }
    }
}