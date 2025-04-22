using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public IMemoryCache Cache { get; set; }

        public static JsonSerializerOptions SerializationOptions
        {
            get
            {
                JsonSerializerOptions opts = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = false
                };

                return opts;
            }
        }

        public T Get<T>(string key)
        {
            Cache.TryGetValue(key, out string value);
            return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value) : default;
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

            Cache.Set(key, JsonSerializer.Serialize(value, SerializationOptions), cacheEntryOptions);
        }

        public void SetRaw(string key, string value, TimeSpan? expiry = null)
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
            FieldInfo coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            object coherentStateValue = coherentState.GetValue(Cache);
            PropertyInfo stringEntriesCollection = coherentStateValue.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            List<string> keys = [];

            if (stringEntriesCollection.GetValue(coherentStateValue) is ICollection stringEntriesCollectionValue)
            {
                foreach (object item in stringEntriesCollectionValue)
                {
                    PropertyInfo methodInfo = item.GetType().GetProperty("Key");
                    object val = methodInfo.GetValue(item);
                    keys.Add(val.ToString());
                }
            }

            return keys;
        }

        public Task RemoveAsync(IEnumerable<string> keys)
        {
            foreach (string key in keys)
                Remove(key, true);

            return Task.CompletedTask;
        }
    }
}