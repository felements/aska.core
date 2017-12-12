using System;

namespace aska.core.networking.CacheStore
{
    public class Cached<TItem> where TItem: class
    {
        private readonly int _cacheLifetimeInMinutes;
        private readonly object _lock = new object();
        private static DateTime _lastUpdateTime = DateTime.MinValue;
        private static TItem _item = null;
        private readonly Func<TItem> _createDelegate;


        public Cached(Func<TItem> createDelegate, int cacheLifetimeInMinutes)
        {
            _cacheLifetimeInMinutes = cacheLifetimeInMinutes;
            _createDelegate = createDelegate;
        }

        public TItem Get()
        {
            if ((DateTime.UtcNow - _lastUpdateTime).TotalMinutes > _cacheLifetimeInMinutes)
                lock (_lock)
                {
                    if ((DateTime.UtcNow - _lastUpdateTime).TotalMinutes > _cacheLifetimeInMinutes)
                    {
                        _item = _createDelegate();
                        _lastUpdateTime = DateTime.UtcNow;
                    }
                }

            return _item;
        }

    }
}