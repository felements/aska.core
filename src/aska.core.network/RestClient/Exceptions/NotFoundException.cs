using System;
using System.Net.Http;

namespace aska.core.network.RestClient.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(HttpMethod method, string url,  string content, Exception inner) 
            : base($"Not found at {method.Method} {url} - {content}", inner)
        {
            
        }
    }
}