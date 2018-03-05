using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicConsumerWrapper : KafkaTopicWrapper
    {
        protected readonly string _consumerId;
        protected readonly Lazy<Consumer> _consumer;
        protected readonly Lazy<List<TopicPartitionOffsetError>> _tpos;
        
        public KafkaTopicConsumerWrapper(ClusterConfiguration configuration, string topicId, string consumerId)
            : base(configuration, topicId)
        {
            _consumerId = consumerId;
            _consumer = new Lazy<Consumer>(LoadConsumer);
            _tpos = new Lazy<List<TopicPartitionOffsetError>>(LoadTpos);
        }

        public long TotalCommitted => _tpos.Value.Select(tpo => tpo.Offset.Value).Sum();
        
        private Consumer LoadConsumer()
        {
            return _configuration.BuildConsumer(_consumerId);
        }
        
        private List<TopicPartitionOffsetError> LoadTpos()
        {
            return _consumer.Value.Committed(Metadata.Partitions.Select(
                    p => new TopicPartition(_topicId, p.PartitionId)), TimeSpan.FromSeconds(5));
        }

        public override void Dispose()
        {
            base.Dispose();
            
            if (_consumer.IsValueCreated)
                _consumer.Value.Dispose();
        }
    }
}