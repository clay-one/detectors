using System;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicWrapper : IDisposable
    {
        protected readonly KafkaClusterConfiguration Configuration;
        protected readonly string TopicId;
        
        protected readonly Lazy<TopicMetadata> TopicMetadata;
        protected readonly Lazy<Producer> Producer;

        public KafkaTopicWrapper(KafkaClusterConfiguration configuration, string topicId)
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
            var result = TopicMetadata.Value.Partitions
                .AsParallel()
                .Select(p => GetMaxOffset(p.PartitionId))
                .Sum();
            
            GetTotalMaxOffsetsRateCalculator().AddSample(result);
            return result;
        }

        public RateCalculator GetTotalMaxOffsetsRateCalculator()
        {
            return RateCalculatorCollection.GetCalculator(TotalMaxOffsetsRateCalculatorKey, true);
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
        
        private string TotalMaxOffsetsRateCalculatorKey => 
            $"kafka/cluster/{Configuration.Id}topic/{TopicId}/offsets/total";

    }
}