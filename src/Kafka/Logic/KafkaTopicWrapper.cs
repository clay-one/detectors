using System;
using System.Linq;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicWrapper : IDisposable
    {
        protected readonly KafkaClusterConfig Config;
        protected readonly string TopicId;
        
        protected readonly Lazy<TopicMetadata> TopicMetadata;
        protected readonly Lazy<Producer> Producer;

        public KafkaTopicWrapper(KafkaClusterConfig config, string topicId)
        {
            Config = config;
            TopicId = topicId;

            Producer = new Lazy<Producer>(LoadProducer);
            TopicMetadata = new Lazy<TopicMetadata>(LoadTopicMetadata);
        }
        
        public TopicMetadata Metadata => TopicMetadata.Value;
        
        public long GetLowOffset(int partitionId)
        {
            var offsets = Producer.Value.QueryWatermarkOffsets(new TopicPartition(TopicId, partitionId));

            if (offsets.Low.IsSpecial)
                throw new InvalidKafkaResponseException(
                    $"Returned high offset is special ({offsets.High.Value}) for partition '{partitionId}' on topic '{TopicId}'.");
            
            return offsets.Low.Value;
        }
        
        public long GetHighOffset(int partitionId)
        {
            var offsets = Producer.Value.QueryWatermarkOffsets(new TopicPartition(TopicId, partitionId));

            if (offsets.High.IsSpecial)
                throw new InvalidKafkaResponseException(
                    $"Returned high offset is special ({offsets.High.Value}) for partition '{partitionId}' on topic '{TopicId}'.");
            
            return offsets.High.Value;
        }

        public long GetTotalHighOffsets()
        {
            var result = TopicMetadata.Value.Partitions
                .AsParallel()
                .Select(p => GetHighOffset(p.PartitionId))
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
            return Config.BuildProducer();
        }

        private TopicMetadata LoadTopicMetadata()
        {
            var result = Producer.Value
                .GetMetadata(false, TopicId, TimeSpan.FromSeconds(5))
                .Topics.FirstOrDefault();
            
            if (result.Error.HasError)
                throw new InvalidKafkaResponseException($"");
            
            return result;
        }
        
        private string TotalMaxOffsetsRateCalculatorKey => 
            $"kafka/cluster/{Config.Id}/topic/{TopicId}/offsets/total";

    }
}