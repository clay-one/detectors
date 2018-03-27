using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Configuration
{
    public static class ConfigurationExtensions
    {
        public static List<KafkaClusterConfiguration> GetKafkaClusters(this IConfiguration configuration)
        {
            return configuration.GetSection("kafka:clusters").GetChildren().Select(c => c.Get<KafkaClusterConfiguration>()).ToList();
        }
        
        public static KafkaClusterConfiguration GetKafkaCluster(this IConfiguration configuration, string id)
        {
            return configuration.GetKafkaClusters().FirstOrDefault(c => c.Id == id);
        }
    }
}