using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<T> GetAsync<T>(string key)
        {
            T value = Get<T>(key);
            return Task.FromResult(value);
        }

        public bool Contains(string key)
            => Cache.TryGetValue(key, out _);

        public Task<bool> ContainsAsync(string key)
            => Task.FromResult(Contains(key));

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            MemoryCacheEntryOptions cacheEntryOptions = new();
            if (expiry != null)
                cacheEntryOptions.SetSlidingExpiration(expiry.GetValueOrDefault());

            Cache.Set(key, value, cacheEntryOptions);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            Set(key, value, expiry);
            return Task.CompletedTask;
        }

        public void Remove(string key, bool exactMatch = true)
        {
            if (!exactMatch)
                throw new NotSupportedException("This action is not supported yet");

            Cache.Remove(key);
        }

        public Task RemoveAsync(string key, bool exactMatch = true)
        {
            Remove(key, exactMatch);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(IEnumerable<string> keys)
        {
            foreach (string key in keys)
                Remove(key, true);

            return Task.CompletedTask;
        }
    }
}