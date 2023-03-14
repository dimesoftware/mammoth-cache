using System;
using Microsoft.Extensions.Caching.Memory;

namespace MammothCache
{
    public class InMemoryCacheDecorator : ICache
    {
        public InMemoryCacheDecorator(IMemoryCache cache)
        {
            Cache = cache;
        }

        private IMemoryCache Cache { get; set; }

        public T Get<T>(string key)
        {
            Cache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            MemoryCacheEntryOptions cacheEntryOptions = new();
            if (expiry != null)
                cacheEntryOptions.SetSlidingExpiration(expiry.GetValueOrDefault());

            Cache.Set(key, value, cacheEntryOptions);
        }

        public void Remove(string key, bool exactMatch = true)
        {
            if (!exactMatch)
                throw new NotSupportedException("This action is not supported yet");

            Cache.Remove(key);
        }
    }
}