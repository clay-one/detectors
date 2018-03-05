using System;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;

namespace Detectors.Kafka.Logic
{
    public class KafkaTopicWrapper : IDisposable
    {
        protected readonly ClusterConfiguration _configuration;
        protected readonly string _topicId;
        
        protected readonly Lazy<TopicMetadata> _metadata;
        protected readonly Lazy<Producer> _producer;

        public KafkaTopicWrapper(ClusterConfiguration configuration, string topicId)
        {
            _configuration = configuration;
            _topicId = topicId;

            _producer = new Lazy<Producer>(() => _configuration
                .BuildProducer());

            _metadata = new Lazy<TopicMetadata>(() => _producer
                .Value
                .GetMetadata(false, _topicId, TimeSpan.FromSeconds(3))
                .Topics
                .FirstOrDefault());
        }
        
        public TopicMetadata Metadata => _metadata.Value;
        
        public long GetMaxOffset(int partitionId)
        {
            var result = _producer.Value.QueryWatermarkOffsets(new TopicPartition(_topicId, partitionId)).High.Value;
            Console.WriteLine($"Max offset of {partitionId} is {result}");
            return result;
        }

        public long GetTotalMaxOffsets()
        {
            return _metadata.Value.Partitions
                .AsParallel()
                .Select(p => GetMaxOffset(p.PartitionId))
                .Sum();
        }

        public virtual void Dispose()
        {
            if (_producer.IsValueCreated)
                _producer.Value.Dispose();
        }
    }
}