using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;

namespace Detectors.Kafka.Configuration
{
    public class KafkaClusterConfig
    {
        public string Id { get; set; }
        public List<KafkaClusterBrokerConfig> Brokers { get; set; }
        public bool DebugEnabled { get; set; }

        public Consumer BuildConsumer(string consumerId)
        {
            var consumer = new Consumer(BuildConfigDictionary(consumerId));
            
            if (DebugEnabled)
                consumer.OnLog += LogMessage;

            return consumer;
        }

        public Producer BuildProducer()
        {
            var producer = new Producer(BuildConfigDictionary());

            if (DebugEnabled)
                producer.OnLog += LogMessage;
            
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
                {"client.id", "detectors"},
                {"enable.auto.commit", "false"},
                {"offset.store.method", "none"}
//                {"auto.offset.reset", "smallest"}
            };

            if (DebugEnabled)
                result["debug"] = "all";
            
            if (consumerId != null)
                result["group.id"] = consumerId;

            return result;
        }

        private static void LogMessage(object sender, LogMessage message)
        {
            Console.WriteLine($"{DateTime.Now:O} - {message.Level} / {message.Facility,-12} : {message.Message}");
        }
    }
}