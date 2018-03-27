using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Detectors.Redis.Configuration
{
    public class RedisConnectionConfiguration
    {
        public string Id { get; set; }
        public List<string> EndPoints { get; set; }
        public int? ConnectRetry { get; set; }
        public int? ConnectTimeout { get; set; }
        public int? DefaultDatabase { get; set; }

        public ConnectionMultiplexer BuildMultiplexer()
        {
            return ConnectionMultiplexer.Connect(BuildConfigurationOptions());
        }
        
        private ConfigurationOptions BuildConfigurationOptions()
        {
            var result = new ConfigurationOptions();

            if (EndPoints != null)
                foreach (var ep in EndPoints.DefaultIfEmpty())
                    result.EndPoints.Add(ep);

            if (ConnectRetry.HasValue)
                result.ConnectRetry = ConnectRetry.Value;

            if (ConnectTimeout.HasValue)
                result.ConnectTimeout = ConnectTimeout.Value;

            if (DefaultDatabase.HasValue)
                result.DefaultDatabase = DefaultDatabase.Value;

            return result;
        }
    }
}