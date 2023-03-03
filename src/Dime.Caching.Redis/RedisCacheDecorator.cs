using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Dime.Caching.Redis
{
    public class RedisCacheDecorator : ICache
    {
        protected readonly IDatabase _cache;

        public RedisCacheDecorator(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        protected virtual string GetKey(string key) => key;

        public virtual T Get<T>(string key)
        {
            RedisValue value = _cache.StringGet(GetKey(key));
            return !value.IsNull ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public virtual void Remove(string key)
            => _cache.KeyDelete(GetKey(key));

        public virtual void Set<T>(string key, T value, TimeSpan? expiry)
            => _cache.StringSetAsync(
                GetKey(key),
                JsonSerializer.Serialize(value, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles }),
                expiry ?? TimeSpan.FromHours(1));
    }
}