using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MammothCache
{
    public partial class RedisCacheDecorator
    {
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