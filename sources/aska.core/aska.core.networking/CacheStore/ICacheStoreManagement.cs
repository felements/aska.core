namespace aska.core.networking.CacheStore
{
    public interface ICacheStoreManagement
    {
        CacheStoreStats GetStats();

        void ClearExpired();
        void Clear();
        void Clear(string key);
    }

    public class CacheStoreStats
    {
        public int Count { get; set; }
        public long Size { get; set; }
        //public long FetchTimeMilliseconds { get; set; }
    }
}