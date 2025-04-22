using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            if (exactMatch)
            {
                Cache.Remove(key);
                return;
            }

            if (Cache is MemoryCache memoryCache)
            {
                List<string> entries = GetAllKeys();
                if (entries != null)
                {
                    string pattern = "^" + Regex.Escape(key).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                    Regex regex = new(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    foreach (string entry in entries.Where(entry => regex.IsMatch(entry)))
                        memoryCache.Remove(entry);
                }
            }
        }

        public Task RemoveAsync(string key, bool exactMatch = true)
        {
            Remove(key, exactMatch);
            return Task.CompletedTask;
        }

        private List<string> GetAllKeys()
        {
            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var coherentStateValue = coherentState.GetValue(Cache);

            var stringEntriesCollection = coherentStateValue.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            var stringEntriesCollectionValue = stringEntriesCollection.GetValue(coherentStateValue) as ICollection;

            var keys = new List<string>();

            if (stringEntriesCollectionValue != null)
            {
                foreach (var item in stringEntriesCollectionValue)
                {
                    var methodInfo = item.GetType().GetProperty("Key");

                    var val = methodInfo.GetValue(item);

                    keys.Add(val.ToString());
                }
            }

            return keys;
        }
    }
}