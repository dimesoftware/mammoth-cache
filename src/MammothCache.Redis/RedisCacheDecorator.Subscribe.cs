using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MammothCache
{
    public partial class RedisCacheDecorator
    {
        public void Subscribe(string channel, Action<string> onInvalidated)
        {
            ISubscriber subscriber = _connectionMultiplexer.GetSubscriber();
            RedisChannel channelWithPattern = new(channel, RedisChannel.PatternMode.Literal);
            subscriber.Subscribe(channelWithPattern, (channel, key) => onInvalidated(key));
        }

        public Task PublishAsync(string channel, string key)
        {
            ISubscriber subscriber = _connectionMultiplexer.GetSubscriber();
            RedisChannel channelWithPattern = new(channel, RedisChannel.PatternMode.Literal);
            return subscriber.PublishAsync(channelWithPattern, key);
        }
    }
}