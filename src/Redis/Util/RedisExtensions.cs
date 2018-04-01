using System.Linq;
using StackExchange.Redis;

namespace Detectors.Redis.Util
{
    public static class RedisExtensions
    {
        public static IServer GetFirstServer(this ConnectionMultiplexer redis)
        {
            var endpoint = redis?.GetEndPoints()?.FirstOrDefault();
            return endpoint == null ? null : redis.GetServer(endpoint);
        }
    }
}