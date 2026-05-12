namespace Wasla_Backend.Helpers.Caching
{
    public interface ICacheManager
    {
        void Set<T>(string key, T value, TimeSpan? ttl = null);
        T? Get<T>(string key);
        void Remove(string key);
        bool Exists(string key);
    }
}
