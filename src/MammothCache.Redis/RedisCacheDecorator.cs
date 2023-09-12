using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MammothCache
{
    public class RedisCacheDecorator : ICache
    {
        protected readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheDecorator(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
            _connectionMultiplexer = redis;
        }

        protected virtual string GetKey(string key) => key;

        public virtual T Get<T>(string key)
        {
            RedisValue value = _cache.StringGet(GetKey(key));
            return !value.IsNull ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            RedisValue value = await _cache.StringGetAsync(GetKey(key));
            return !value.IsNull ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public virtual void Set<T>(string key, T value, TimeSpan? expiry)
            => _cache.StringSet(
                GetKey(key),
                JsonSerializer.Serialize(value, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles }),
                expiry);

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
            => _cache.StringSetAsync(
                GetKey(key),
                JsonSerializer.Serialize(value, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles }),
                expiry);

        public virtual void Remove(string key, bool exactMatch = true)
        {
            if (exactMatch)
            {
                _cache.KeyDelete(GetKey(key));
                return;
            }

            foreach (EndPoint endpoint in _connectionMultiplexer.GetEndPoints())
            {
                IServer server = _connectionMultiplexer.GetServer(endpoint);
                IEnumerable<RedisKey> keys = server.Keys(_cache.Database, pattern: GetKey(key) + "*");
                foreach (RedisKey redisKey in keys)
                    _cache.KeyDelete(redisKey);
            }
        }

        public async Task RemoveAsync(string key, bool exactMatch = true)
        {
            if (exactMatch)
            {
                await _cache.KeyDeleteAsync(GetKey(key));
                return;
            }

            foreach (EndPoint endpoint in _connectionMultiplexer.GetEndPoints())
            {
                IServer server = _connectionMultiplexer.GetServer(endpoint);
                IEnumerable<RedisKey> keys = server.Keys(_cache.Database, pattern: GetKey(key) + "*");
                foreach (RedisKey redisKey in keys)
                    await _cache.KeyDeleteAsync(redisKey);
            }
        }
    }
}