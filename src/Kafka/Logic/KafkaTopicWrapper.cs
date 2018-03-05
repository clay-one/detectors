using System;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicWrapper : IDisposable
    {
        protected readonly ClusterConfiguration Configuration;
        protected readonly string TopicId;
        
        protected readonly Lazy<TopicMetadata> TopicMetadata;
        protected readonly Lazy<Producer> Producer;

        public KafkaTopicWrapper(ClusterConfiguration configuration, string topicId)
        {
            Configuration = configuration;
            TopicId = topicId;

            Producer = new Lazy<Producer>(LoadProducer);
            TopicMetadata = new Lazy<TopicMetadata>(LoadTopicMetadata);
        }
        
        public TopicMetadata Metadata => TopicMetadata.Value;
        
        public long GetMaxOffset(int partitionId)
        {
            return Producer.Value.QueryWatermarkOffsets(new TopicPartition(TopicId, partitionId)).High.Value;
        }

        public long GetTotalMaxOffsets()
        {
            return TopicMetadata.Value.Partitions
                .AsParallel()
                .Select(p => GetMaxOffset(p.PartitionId))
                .Sum();
        }

        public virtual void Dispose()
        {
            if (Producer.IsValueCreated)
                Producer.Value.Dispose();
        }

        private Producer LoadProducer()
        {
            return Configuration.BuildProducer();
        }

        private TopicMetadata LoadTopicMetadata()
        {
            return Producer
                .Value
                .GetMetadata(false, TopicId, TimeSpan.FromSeconds(5))
                .Topics
                .FirstOrDefault();
        }
    }
}