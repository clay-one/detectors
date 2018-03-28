using System;
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
            var consumer = new Consumer(BuildConfigDictionary(consumerId));
            consumer.OnLog += (sender, message) =>
            {
                Console.WriteLine($"{DateTime.Now:O} - {message.Level} / {message.Facility,-12} : {message.Message}");
            };

            return consumer;
        }

        public Producer BuildProducer()
        {
            var producer = new Producer(BuildConfigDictionary());
            producer.OnLog += (sender, message) =>
            {
                Console.WriteLine($"{DateTime.Now:O} - {message.Level} / {message.Facility,-12} : {message.Message}");
            };
            
            return producer;
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
                {"debug", "all"}
            };

            if (consumerId != null)
                result["group.id"] = consumerId;

            return result;
        }

    }
}