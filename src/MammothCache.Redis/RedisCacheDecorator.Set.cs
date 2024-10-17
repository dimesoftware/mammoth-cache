using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MammothCache
{
    public partial class RedisCacheDecorator
    {
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

        public virtual void Set<T>(string key, T value, TimeSpan? expiry)
            => _cache.StringSet(GetKey(key), JsonSerializer.Serialize(value, SerializationOptions), expiry);

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
            => _cache.StringSetAsync(GetKey(key), JsonSerializer.Serialize(value, SerializationOptions), expiry);
    }
}