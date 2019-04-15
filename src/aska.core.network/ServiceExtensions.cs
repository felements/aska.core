using aska.core.network.RestClient;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.network
{
    public static class ServiceExtensions
    {
        public static void AddNetworkServices(this IServiceCollection services)
        {
            services.AddScoped<IRestApiClient, RestApiClient>();
        }
    }
}