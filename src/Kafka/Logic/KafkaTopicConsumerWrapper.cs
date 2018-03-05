using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicConsumerWrapper : KafkaTopicWrapper
    {
        protected readonly string ConsumerId;
        protected readonly Lazy<Consumer> Consumer;
        protected readonly Lazy<List<TopicPartitionOffsetError>> Tpos;
        
        public KafkaTopicConsumerWrapper(ClusterConfiguration configuration, string topicId, string consumerId)
            : base(configuration, topicId)
        {
            ConsumerId = consumerId;
            Consumer = new Lazy<Consumer>(LoadConsumer);
            Tpos = new Lazy<List<TopicPartitionOffsetError>>(LoadTpos);
        }

        public long TotalCommitted => Tpos.Value.Select(tpo => tpo.Offset.Value).Sum();
        
        public override void Dispose()
        {
            base.Dispose();
            
            if (Consumer.IsValueCreated)
                Consumer.Value.Dispose();
        }

        private Consumer LoadConsumer()
        {
            return Configuration.BuildConsumer(ConsumerId);
        }
        
        private List<TopicPartitionOffsetError> LoadTpos()
        {
            return Consumer.Value.Committed(Metadata.Partitions.Select(
                    p => new TopicPartition(TopicId, p.PartitionId)), TimeSpan.FromSeconds(5));
        }
    }
}