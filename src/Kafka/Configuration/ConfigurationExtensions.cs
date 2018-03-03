using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Configuration
{
    public static class ConfigurationExtensions
    {
        public static List<ClusterConfiguration> GetClusters(this IConfiguration configuration)
        {
            return configuration.GetSection("clusters").GetChildren().Select(c => c.Get<ClusterConfiguration>()).ToList();
        }
        
        public static ClusterConfiguration GetCluster(this IConfiguration configuration, string id)
        {
            return configuration.GetClusters().FirstOrDefault(c => c.Id == id);
        }
    }
}