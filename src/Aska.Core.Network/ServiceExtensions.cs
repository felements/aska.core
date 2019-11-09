using Aska.Core.Network.RestClient;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Network
{
    public static class ServiceExtensions
    {
        public static void AddRestClient(this IServiceCollection services)
        {
            services.AddScoped<IRestApiClient, RestApiClient>();
        }
    }
}