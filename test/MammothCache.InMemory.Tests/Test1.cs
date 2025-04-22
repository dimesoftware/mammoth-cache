using Microsoft.Extensions.Caching.Memory;

namespace MammothCache.InMemory.Tests
{
    [TestClass]
    public sealed class InMemoryCacheDecoratorTests
    {
        [TestMethod]
        public void InMemoryCacheDecorator_Remove_Wildcard_ShouldDeleteAll()
        {
            InMemoryCacheDecorator cache = new(new MemoryCache(new MemoryCacheOptions()));
            cache.Set("key1:suffix1", true);
            cache.Set("key1:suffix2", true);
            cache.Set("prefix1:key1:suffix2", true);
            cache.Set("key2", true);

            cache.Remove("key1:*", false);

            Assert.IsTrue(cache.Get<bool?>("key1:suffix1") == null);
            Assert.IsTrue(cache.Get<bool?>("key1:suffix2") == null);
            Assert.IsTrue(cache.Get<bool?>("key2") == true);
            Assert.IsTrue(cache.Get<bool?>("prefix1:key1:suffix2") == true);
        }
    }
}
