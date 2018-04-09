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
        
        public KafkaTopicConsumerWrapper(KafkaClusterConfig config, string topicId, string consumerId)
            : base(config, topicId)
        {
            ConsumerId = consumerId;
            Consumer = new Lazy<Consumer>(LoadConsumer);
            Tpos = new Lazy<List<TopicPartitionOffsetError>>(LoadTpos);
        }

        public long GetTotalCommitted()
        {
            var result = Tpos.Value.Select(GetOffsetValue).Sum();
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
            return Config.BuildConsumer(ConsumerId);
        }
        
        private string TotalCommittedRateCalculatorKey => 
            $"kafka/cluster/{Config.Id}topic/{TopicId}/consumer/{ConsumerId}/commit/total";

        private List<TopicPartitionOffsetError> LoadTpos()
        {
            var result = Consumer.Value.Committed(Metadata.Partitions.Select(
                p => new TopicPartition(TopicId, p.PartitionId)), TimeSpan.FromSeconds(5));
            
            if (result.Any(tpo => tpo.Error.HasError))
            {
                var errors = result.Where(tpo => tpo.Error.HasError).Select(tpo => tpo.Error.Code.ToString());
                throw new InvalidKafkaResponseException($"Errors when fetching committed offsets: " +
                                                        string.Join(",", errors));
            }

            return result;
        }

        private long GetOffsetValue(TopicPartitionOffsetError tpo)
        {
            if (!tpo.Offset.IsSpecial)
                return tpo.Offset.Value;

            var offset = tpo.Offset;
            if (offset == Offset.Invalid)
            {
                // TODO: Check configuration
                offset = Offset.End;
            }
            
            if (offset == Offset.Beginning)
            {
                return GetLowOffset(tpo.Partition);
            }

            if (offset == Offset.End)
            {
                return GetLowOffset(tpo.Partition);
            }

            throw new InvalidKafkaResponseException($"Cannot understand special offset value of {tpo.ToString()} for ");
        }
    }
}