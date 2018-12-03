using Detectors.Kafka.Configuration;
using Detectors.Kafka.Model;
using System.Collections.Generic;
using System.Linq;

namespace Detectors.Kafka.Logic
{
    public class NotSyncReplicaLogic
    {
        private readonly KafkaClusterConfigCollection _configuration;

        public NotSyncReplicaLogic(KafkaClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        public BrokerPartition GetBrokerPartition(KafkaClusterConfig kafkaClusterConfig, string bootstrapServers)
        {
            BrokerPartition result;

            using (var producer = kafkaClusterConfig.BuildProducer(bootstrapServers))
            {
                if (producer == null)
                    return null;

                var metadata = producer.GetMetadata();
                var partitionInfoList = new List<PartitionInfo>();
                foreach (var topic in metadata.Topics)
                {
                    foreach (var partition in topic.Partitions)
                    {
                        var item = new PartitionInfo
                        {
                            Topic = topic.Topic,
                            PartitionId = partition.PartitionId,
                            Leader = partition.Leader,
                            Replicas = partition.Replicas.OrderBy(r => r).ToArray(),
                            ISRs = partition.InSyncReplicas.OrderBy(isr => isr).ToArray()
                        };

                        partitionInfoList.Add(item);
                    }
                }

                result = new BrokerPartition { BrokerId = metadata.OriginatingBrokerId, PartitionInfoList = partitionInfoList };
            }

            return result;
        }

        public List<BrokerPartition> GetBrokerPartitionList(string clusterId)
        {
            var result = new List<BrokerPartition>();

            var clusterConfig = _configuration.GetKafkaClusterConfig(clusterId);

            Enumerable
                .Range(0, clusterConfig.Brokers.Count)
                .AsParallel()
                .ForAll(i =>
                {
                    var bootstrapServers = clusterConfig.BuildBrokersString(
                        new List<KafkaClusterBrokerConfig> { clusterConfig.Brokers[i] });

                    var brokerPartition = GetBrokerPartition(clusterConfig, bootstrapServers);
                    if (brokerPartition != null)
                        result.Add(brokerPartition);
                });

            return result;
        }

        public List<NotSyncReplica> GetNotSyncReplicaList(List<BrokerPartition> brokerPartitionList)
        {
            var notSyncReplicaList = new List<NotSyncReplica>();

            var topicPartitionList = brokerPartitionList
                .SelectMany(c => c.PartitionInfoList.Select(p => new { p.Topic, p.PartitionId }))
                .Distinct()
                .ToList();

            var baseBrokerPartition = brokerPartitionList.First();
            var baseBrokerId = baseBrokerPartition.BrokerId;

            foreach (var topicPartition in topicPartitionList)
            {
                var basePartitionInfo = baseBrokerPartition
                    .GetPartitionInfo(topicPartition.Topic, topicPartition.PartitionId);

                if (basePartitionInfo == null)
                {
                    notSyncReplicaList.Add(new NotSyncReplica
                    {
                        BrokerId1 = baseBrokerId,
                        Leader1 = -1,
                        Replicas1 = string.Empty,
                        ISRs1 = string.Empty,

                        Topic = topicPartition.Topic,
                        PartitionId = topicPartition.PartitionId,

                        BrokerId2 = -1,
                        Leader2 = -1,
                        Replicas2 = string.Empty,
                        ISRs2 = string.Empty,
                    });

                    continue;
                }

                for (var i = 1; i < brokerPartitionList.Count; i++)
                {
                    var brokerPartition = brokerPartitionList[i];
                    var matchPartitionInfo = brokerPartition
                        .GetPartitionInfo(topicPartition.Topic, topicPartition.PartitionId);

                    if (matchPartitionInfo == null)
                    {
                        notSyncReplicaList.Add(new NotSyncReplica
                        {
                            BrokerId1 = baseBrokerId,
                            Leader1 = basePartitionInfo.Leader,
                            Replicas1 = string.Join(",", basePartitionInfo.Replicas.Select(r => r.ToString())),
                            ISRs1 = string.Join(",", basePartitionInfo.ISRs.Select(r => r.ToString())),

                            Topic = topicPartition.Topic,
                            PartitionId = topicPartition.PartitionId,

                            BrokerId2 = brokerPartition.BrokerId,
                            Leader2 = -1,
                            Replicas2 = string.Empty,
                            ISRs2 = string.Empty,
                        });

                        continue;
                    }

                    if (matchPartitionInfo.Leader != basePartitionInfo.Leader
                        || matchPartitionInfo.Replicas.SequenceEqual(matchPartitionInfo.Replicas) == false
                        || matchPartitionInfo.ISRs.SequenceEqual(matchPartitionInfo.ISRs) == false)
                    {
                        notSyncReplicaList.Add(new NotSyncReplica
                        {
                            BrokerId1 = baseBrokerId,
                            Leader1 = basePartitionInfo.Leader,
                            Replicas1 = string.Join(",", basePartitionInfo.Replicas.Select(r => r.ToString())),
                            ISRs1 = string.Join(",", basePartitionInfo.ISRs.Select(r => r.ToString())),

                            Topic = topicPartition.Topic,
                            PartitionId = topicPartition.PartitionId,

                            BrokerId2 = brokerPartition.BrokerId,
                            Leader2 = matchPartitionInfo.Leader,
                            Replicas2 = string.Join(",", matchPartitionInfo.Replicas.Select(r => r.ToString())),
                            ISRs2 = string.Join(",", matchPartitionInfo.ISRs.Select(r => r.ToString())),
                        });
                    }
                }
            }

            notSyncReplicaList = notSyncReplicaList
                .OrderBy(n => n.BrokerId2)
                .ThenBy(n => n.Topic)
                .ThenBy(n => n.PartitionId)
                .ToList();

            return notSyncReplicaList;
        }
    }
}