using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MammothCache
{
    public partial class RedisCacheDecorator : ICache
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

        public async Task<string> GetRawAsync(string key)
        {
            RedisValue value = await _cache.StringGetAsync(GetKey(key));
            return value.ToString();
        }
    }
}