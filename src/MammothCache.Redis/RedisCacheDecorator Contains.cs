using System.Threading.Tasks;

namespace MammothCache
{
    public partial class RedisCacheDecorator
    {
        public virtual bool Contains(string key)
            => _cache.KeyExists(GetKey(key));

        public Task<bool> ContainsAsync(string key)
            => _cache.KeyExistsAsync(GetKey(key));
    }
}