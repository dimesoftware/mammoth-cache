using Microsoft.Extensions.Caching.Memory;

namespace MammothCache.InMemory.Tests
{
    public class InMemoryCacheDecoratorTests
    {
        [Fact]
        public async Task GetAsync_Hit_ShouldReturnData()
        {
            MemoryCache memCache = new(new MemoryCacheOptions());
            InMemoryCacheDecorator inMemoryCache = new(memCache);
            inMemoryCache.Set("customers", new List<Customer> { new() { Name = "Customer 1" } });

            IEnumerable<Customer> customers = await inMemoryCache.GetAsync<IEnumerable<Customer>>("customers");
            Assert.True(customers.Count() == 1);
        }

        [Fact]
        public async Task SetRaw_ArrayAsString_ShouldReturnArrayAsObjects()
        {
            MemoryCache memCache = new(new MemoryCacheOptions());
            InMemoryCacheDecorator inMemoryCache = new(memCache);

            inMemoryCache.SetRaw("customers", "[{\"Name\":\"Customer 1\"}]");

            IEnumerable<Customer> customers = await inMemoryCache.GetAsync<IEnumerable<Customer>>("customers");
            Assert.True(customers.Count() == 1);
        }
    }
}