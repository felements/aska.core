using System;
using System.Collections.Concurrent;

namespace aska.core.networking.CacheStore
{
    //TODO: Cache cleanup
    public class MemoryCacheStore : ICacheStore, ICacheStoreManagement
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<string, CacheRecord<SerializableResponse>> _cache;
        public MemoryCacheStore()
        {
            _cache = new ConcurrentDictionary<string, CacheRecord<SerializableResponse>>();
        }

        public CachedResponse Get(string keyRaw)
        {
            var key = NormalizeKey(keyRaw);

            CacheRecord<SerializableResponse> record;
            if (!_cache.TryGetValue(key, out record)) return null;

            if (record.ExpirationTime < DateTime.Now)
            {
                Remove(key);
            }
            return new CachedResponse(record.Data);
        }

        public void Set(string keyRaw, NancyContext context, DateTime absoluteExpiration)
        {
            var key = NormalizeKey(keyRaw);

            _cache.AddOrUpdate(key, 
                s => new CacheRecord<SerializableResponse>( new SerializableResponse(context.Response, absoluteExpiration), absoluteExpiration),
                (s, record) => new CacheRecord<SerializableResponse>(new SerializableResponse(context.Response, absoluteExpiration), absoluteExpiration));
        }

        public void Remove(string keyRaw)
        {
            var key = NormalizeKey(keyRaw);

            CacheRecord<SerializableResponse> record;
            _cache.TryRemove(key, out record);
        }

        public CacheStoreStats GetStats()
        {
            return new CacheStoreStats
            {
                Count = _cache.Count,
                Size = GetObjectSize(_cache)
            };
        }

        public void ClearExpired()
        {
            foreach (var key in _cache.Keys)
            {
                CacheRecord<SerializableResponse> record;
                if (!_cache.TryGetValue(key, out record)) continue;

                if (record.IsExpired) _cache.TryRemove(key, out record);
            }
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void Clear(string keyPartRaw)
        {
            var keyPart = NormalizeKey(keyPartRaw);

            foreach (var key in _cache.Keys)
            {
                CacheRecord<SerializableResponse> record;
                if (!_cache.TryGetValue(key, out record)) continue;

                if (record.IsExpired) _cache.TryRemove(key, out record);
                if ( key.Contains(keyPart) ) _cache.TryRemove(key, out record);
            }
        }

        private int GetObjectSize(object testObject)
        {
            return JsonConvert.SerializeObject(testObject).Length;

            //BinaryFormatter bf = new BinaryFormatter();
            //MemoryStream ms = new MemoryStream();
            //bf.Serialize(ms, testObject);
            //var array = ms.ToArray();
            //return array.Length;
        }

        private string NormalizeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return String.Empty;
            return key.Trim().ToLowerInvariant();
        }
    }

    public struct CacheRecord<TData>
    {
        public CacheRecord(TData data, DateTime expiration)
        {
            ExpirationTime = expiration;
            Data = data;
        }

        public DateTime ExpirationTime { get; private set; }
        public TData Data { get; private set; }

        public bool IsExpired
        {
            get { return ExpirationTime < DateTime.Now; }
        }
    }
}