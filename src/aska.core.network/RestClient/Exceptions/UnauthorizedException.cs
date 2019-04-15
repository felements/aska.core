using System;
using System.Net.Http;

namespace aska.core.network.RestClient.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(HttpMethod method, string url,  string content, Exception inner) 
            : base($"Unauthorized at {method.Method} {url} - {content}", inner) {}
    }
}