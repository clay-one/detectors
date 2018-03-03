using System.Collections.Generic;
using System.Linq;

namespace Detectors.Kafka.Configuration
{
    public class ClusterConfiguration
    {
        public class BrokerConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; }
        }
        
        public string Id { get; set; }
        public List<BrokerConfiguration> Brokers { get; set; }

        public string BuildBrokersString()
        {
            return string.Join(',', Brokers.Select(b => $"{b.Host}:{b.Port}"));
        }
    }
}