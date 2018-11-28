using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Detectors.Redis.Configuration
{
    public class RedisConnectionConfigCollection
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        
        public RedisConnectionConfigCollection(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public ConnectionMultiplexer BuildMultiplexer(string connectionId)
        {
            var config = GetRedisConnectionConfig(connectionId);
            return config?.BuildMultiplexer();
        }
        
        public List<RedisConnectionConfiguration> GetAllRedisConnectionConfigs()
        {
            return _configuration
                .GetSection("redis:connections")
                .GetChildren()
                .Select(c => c.Get<RedisConnectionConfiguration>())
                .ToList();
        }
        
        public RedisConnectionConfiguration GetRedisConnectionConfig(string connectionId)
        {
            return GetAllRedisConnectionConfigs().FirstOrDefault(c => c.Id == connectionId);
        }
    }
}