using System;
using Microsoft.Extensions.Caching.Memory;

namespace MammothCache.Web
{
    public class InMemoryCache : ICache
    {
        public InMemoryCache(IMemoryCache cache)
        {
            Cache = cache;
        }

        private IMemoryCache Cache { get; set; }

        public T Get<T>(string key)
        {
            Cache.TryGetValue<T>(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            MemoryCacheEntryOptions cacheEntryOptions = new();
            if (expiry != null)
                cacheEntryOptions.SetSlidingExpiration(expiry.GetValueOrDefault());

            Cache.Set(key, value, cacheEntryOptions);
        }

        public void Remove(string key)
            => Cache.Remove(key);
    }
}