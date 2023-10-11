using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MammothCache
{
    public partial class RedisCacheDecorator
    {    
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
    }
}