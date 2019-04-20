using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using aska.core.network.RestClient.Exceptions;
using Newtonsoft.Json;

namespace aska.core.network.RestClient
{
    public class RestApiClient : IRestApiClient
    {
        private readonly Lazy<HttpClient> _client;
        private string _authToken;

        public RestApiClient()
        {
            _client = new Lazy<HttpClient>(
                () => BaseUrl != null ? GetHttpClient(BaseUrl, _authToken) : throw new Exception("Client not initialized"),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Uri BaseUrl { get; private set; }

        public void Initialize(string baseUrl, string authToken = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));
            _authToken = authToken;
            
            BaseUrl = new Uri($"{baseUrl.TrimEnd('/')}/");
        }

        public Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default) where T : new()
        {
            return ExecuteAsync<T>(HttpMethod.Get, url, null, cancellationToken);
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(string url, TRequest obj = default,
            CancellationToken cancellationToken = default) where TResponse : new() where TRequest : class
        {
            return ExecuteAsync<TResponse>(HttpMethod.Get, url, obj, cancellationToken);
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest obj = default,
            CancellationToken cancellationToken = default) where TRequest : class where TResponse : new()
        {
            return ExecuteAsync<TResponse>(HttpMethod.Post, url, obj, cancellationToken);
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest obj = default,
            CancellationToken cancellationToken = default) where TRequest : class where TResponse : new()
        {
            return ExecuteAsync<TResponse>(HttpMethod.Put, url, obj, cancellationToken);
        }

        public Task<TResponse> DeleteAsync<TRequest, TResponse>(string url, TRequest obj = default,
            CancellationToken cancellationToken = default) where TRequest : class where TResponse : new()
        {
            return ExecuteAsync<TResponse>(HttpMethod.Delete, url, obj, cancellationToken);
        }

        private static HttpClient GetHttpClient(Uri baseUrl, string token = null)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                MaxConnectionsPerServer = 5 //todo: move to config
            };
            var client = new HttpClient(handler);
            client.BaseAddress = baseUrl;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("aska-core","0.1.0")); //todo: get product version
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task<TResponse> ExecuteAsync<TResponse>(HttpMethod method, string url, object obj = null,
            CancellationToken cancellationToken = default) where TResponse : new()
        {
            var client = _client.Value;
            var request = new HttpRequestMessage(method, url);

            if (obj != null)
            {
                var data = JsonConvert.SerializeObject(obj ?? new object());
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.SeeOther && request.Headers.Contains("Location"))
            {
                request = new HttpRequestMessage();
            }
            
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
                if (string.IsNullOrEmpty(content)) return default;
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            catch (Exception ex)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedException(method, url, content, ex);
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException(method, url,content, ex);
                    default:
                        throw new Exception(content, ex);
                }
            }
        }
    }
}