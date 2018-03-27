using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;

namespace Detectors.Kafka.Configuration
{
    public class KafkaClusterConfiguration
    {
        public class BrokerConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; }
        }
        
        public string Id { get; set; }
        public List<BrokerConfiguration> Brokers { get; set; }

        public Consumer BuildConsumer(string consumerId)
        {
            return new Consumer(BuildConfigDictionary(consumerId));
        }

        public Producer BuildProducer()
        {
            return new Producer(BuildConfigDictionary());
        }
        
        private string BuildBrokersString()
        {
            return string.Join(",", Brokers.Select(b => $"{b.Host}:{b.Port}"));
        }

        private Dictionary<string, object> BuildConfigDictionary(string consumerId = null)
        {
            var result = new Dictionary<string, object>
            {
                {"bootstrap.servers", BuildBrokersString()},
            };

            if (consumerId != null)
                result["group.id"] = consumerId;

            return result;
        }

    }
}