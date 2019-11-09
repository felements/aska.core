using System;
using System.Collections.Generic;
using System.Linq;

namespace Aska.Core.Network.RestClient
{
    public static class UriExtensions
    {
        public static string ToQuery(params (string Key, object Value)[] pairs)
        {
            var array = pairs
                .Where(keyValue => keyValue.Value != null)
                .Select(keyValue => new KeyValuePair<string, string>(keyValue.Key, keyValue.Value?.ToString()))
                .Select(keyValue => $"{Uri.EscapeDataString(keyValue.Key)}={Uri.EscapeDataString(keyValue.Value)}")
                .ToArray();
            return array.Any() ? $"?{string.Join("&", array)}" : string.Empty;
        }
    }
}