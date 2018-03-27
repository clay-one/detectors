using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Detectors.Redis.Configuration
{
    public static class ConfigurationExtensions
    {
        public static List<RedisConnectionConfiguration> GetRedisConnections(this IConfiguration configuration)
        {
            return configuration.GetSection("redis:connections").GetChildren()
                .Select(c => c.Get<RedisConnectionConfiguration>()).ToList();
        }
        
        public static RedisConnectionConfiguration GetRedisConnection(this IConfiguration configuration, string id)
        {
            return configuration.GetRedisConnections().FirstOrDefault(c => c.Id == id);
        }
    }
}