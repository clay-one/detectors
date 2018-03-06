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

        public long GetTotalCommitted()
        {
            var result = Tpos.Value.Select(tpo => tpo.Offset.Value).Sum();
            GetTotalCommittedRateCalculator().AddSample(result);

            return result;
        }
        
        public RateCalculator GetTotalCommittedRateCalculator()
        {
            return RateCalculatorCollection.GetCalculator(TotalCommittedRateCalculatorKey, true);
        }

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
        
        private string TotalCommittedRateCalculatorKey => 
            $"kafka/cluster/{Configuration.Id}topic/{TopicId}/consumer/{ConsumerId}/commit/total";

        private List<TopicPartitionOffsetError> LoadTpos()
        {
            return Consumer.Value.Committed(Metadata.Partitions.Select(
                    p => new TopicPartition(TopicId, p.PartitionId)), TimeSpan.FromSeconds(5));
        }
    }
}