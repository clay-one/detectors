using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Configuration
{
    public class KafkaClusterConfigCollection
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        
        public KafkaClusterConfigCollection(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public KafkaTopicWrapper BuildTopicWrapper(string clusterId, string topicId)
        {
            var config = GetKafkaClusterConfig(clusterId);
            return config == null ? null : new KafkaTopicWrapper(config, topicId);
        }

        public KafkaTopicConsumerWrapper BuildTopicConsumerWrapper(string clusterId, string topicId, string consumerId)
        {
            var config = GetKafkaClusterConfig(clusterId);
            return config == null ? null : new KafkaTopicConsumerWrapper(config, topicId, consumerId);
        }
        
        public Producer BuildProducer(string clusterId)
        {
            var config = GetKafkaClusterConfig(clusterId);
            return config?.BuildProducer();
        }

        public Consumer BuildConsumer(string clusterId, string consumerId)
        {
            var config = GetKafkaClusterConfig(clusterId);
            return config?.BuildConsumer(consumerId);
        }

        public List<KafkaClusterConfig> GetAllKafkaClusterConfigs()
        {
            return _configuration
                .GetSection("kafka:clusters")
                .GetChildren()
                .Select(c => c.Get<KafkaClusterConfig>())
                .ToList();
        }
        
        public KafkaClusterConfig GetKafkaClusterConfig(string clusterId)
        {
            var result = GetAllKafkaClusterConfigs().FirstOrDefault(c => c.Id == clusterId);
            if (result == null)
                return null;
            
            result.DebugEnabled = _environment.IsDevelopment();
            return result;
        }
    }
}