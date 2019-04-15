using System.Threading;
using System.Threading.Tasks;

namespace aska.core.network.RestClient
{
    public interface IRestApiClient
    {
        void Initialize(string baseUrl, string authToken = null);
        
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default) where TResponse : new();
        Task<TResponse> GetAsync<TRequest, TResponse>(string url, TRequest obj = null, CancellationToken cancellationToken = default) where TResponse : new() where TRequest: class;
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest obj = null, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : new() where TRequest: class;
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest obj = null, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : new() where TRequest : class;
        Task<TResponse> DeleteAsync<TRequest, TResponse>(string url, TRequest obj = null, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : new() where TRequest : class;
    }
}