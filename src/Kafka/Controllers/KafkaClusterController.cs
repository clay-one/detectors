using System;
using System.Linq;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}")]
    public class KafkaClusterController : Controller
    {
        private readonly KafkaClusterConfigCollection _configuration;
        public KafkaClusterController(KafkaClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetClusterInfo(string clusterId)
        {
            return Redirect($"{clusterId}/metadata");
        }

        [HttpGet("metadata")]
        [HttpGet("metadata.{format}")]
        public IActionResult GetClusterMetadata(string clusterId)
        {
            using (var producer = _configuration.BuildProducer(clusterId))
            {
                if (producer == null)
                    return NotFound();

                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(10));
                return Ok(md);
            }
        }

        [HttpGet("brokers")]
        [HttpGet("brokers.{format}")]
        public IActionResult GetBrokers(string clusterId)
        {
            using (var producer = _configuration.BuildProducer(clusterId))
            {
                if (producer == null)
                    return NotFound();

                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(10));
                var result = md.Brokers.Select(b => new
                {
                    b.BrokerId,
                    b.Host,
                    b.Port
                }).ToList();
                return Ok(result);
            }
        }
        
        [HttpGet("topics")]
        [HttpGet("topics.{format}")]
        public IActionResult GetTopicList(string clusterId)
        {
            using (var producer = _configuration.BuildProducer(clusterId))
            {
                if (producer == null)
                    return NotFound();
                
                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(10));
                var resultObject = md.Topics.Select(t => new
                {
                    t.Topic,
                    PartitionCount = t.Partitions.Count
                }).ToList();
                return Ok(resultObject);
            }
        }
        
        [HttpGet("topic-partitions")]
        [HttpGet("topic-partitions.{format}")]
        public IActionResult GetTopicPartitionList(string clusterId)
        {
            using (var producer = _configuration.BuildProducer(clusterId))
            {
                if (producer == null)
                    return NotFound();

                var metadata = producer.GetMetadata();
                var result = metadata.Topics.SelectMany(t => t.Partitions.Select(p => new
                {
                    t.Topic,
                    p.PartitionId,
                    p.Leader,
                    Replicas = string.Join(",", p.Replicas.Select(r => r.ToString())),
                    ISRs = string.Join(",", p.InSyncReplicas.Select(isr => isr.ToString())),
                })).ToList();

                return Ok(result);
            }
        }

        [HttpGet("groups")]
        [HttpGet("groups.{format}")]
        public IActionResult GetGroupList(string clusterId)
        {
            using (var producer = _configuration.BuildProducer(clusterId))
            {
                if (producer == null)
                    return NotFound();

                // Dummy calls to GetMetadata to avoid "Broker transport failure" issue
                producer.GetMetadata();
                producer.GetMetadata();
                
                var groups = producer.ListGroups(TimeSpan.FromSeconds(10));
                var resultObject = groups.Select(g => new
                {
                    g.Group,
                    g.State,
                    g.Protocol,
                    g.ProtocolType,
                    MemberCount = g.Members.Count
                }).ToList();

                return Ok(resultObject);
            }
        }

        [HttpGet("not-sync-replicas")]
        [HttpGet("not-sync-replicas.{format}")]
        public IActionResult GetNotSyncReplicaList(string clusterId)
        {
            var nodeIds = clusterId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var partitions = Enumerable.Empty<object>().Select(q => new
            {
                Topic = string.Empty,
                PartitionId = 0,
                Leader = 0,
                Replicas = new int[0],
                ISRs = new int[0],
            }).ToList();

            var nodePartitions = Enumerable.Empty<object>().Select(p => new
            {
                NodeId = string.Empty,
                Partitions = partitions
            }).ToList();

            foreach (var nodeId in nodeIds)
            {
                using (var producer = _configuration.BuildProducer(nodeId))
                {
                    if (producer == null)
                        continue;

                    var metadata = producer.GetMetadata();

                    partitions.Clear();
                    foreach (var topic in metadata.Topics)
                    {
                        foreach (var partition in topic.Partitions)
                        {
                            var item = new
                            {
                                topic.Topic,
                                partition.PartitionId,
                                partition.Leader,
                                Replicas = partition.Replicas.OrderBy(r => r).ToArray(),
                                ISRs = partition.InSyncReplicas.OrderBy(i => i).ToArray()
                            };

                            partitions.Add(item);
                        }
                    }

                    nodePartitions.Add(new { NodeId = nodeId, Partitions = partitions });
                }
            }

            if (nodePartitions.Count <= 1)
                return Ok("No replicas found");

            var topicPartitionList = nodePartitions
                .SelectMany(c => c.Partitions.Select(p => new { p.Topic, p.PartitionId }))
                .Distinct()
                .ToList();

            var notSyncReplicaList = Enumerable.Empty<object>()
                .Select(n => new
                {
                    NodeId1 = string.Empty,
                    Leader1 = 0,
                    Replicas1 = string.Empty,
                    ISRs1 = string.Empty,

                    Topic = string.Empty,
                    PartitionId = 0,

                    NodeId2 = string.Empty,
                    Leader2 = 0,
                    Replicas2 = string.Empty,
                    ISRs2 = string.Empty,
                }).ToList();

            var baseNodeId = nodePartitions[0].NodeId;

            foreach (var topicPartition in topicPartitionList)
            {
                var basePartition = nodePartitions[0].Partitions
                    .FirstOrDefault(p => p.Topic == topicPartition.Topic
                                         && p.PartitionId == topicPartition.PartitionId);

                if (basePartition == null)
                {
                    notSyncReplicaList.Add(new
                    {
                        NodeId1 = baseNodeId,
                        Leader1 = -1,
                        Replicas1 = string.Empty,
                        ISRs1 = string.Empty,

                        topicPartition.Topic,
                        topicPartition.PartitionId,

                        NodeId2 = string.Empty,
                        Leader2 = -1,
                        Replicas2 = string.Empty,
                        ISRs2 = string.Empty,
                    });

                    continue;
                }

                for (var i = 1; i < nodePartitions.Count; i++)
                {
                    var matchItem = nodePartitions[i].Partitions
                        .FirstOrDefault(p => p.Topic == topicPartition.Topic
                                             && p.PartitionId == topicPartition.PartitionId);

                    if (matchItem == null)
                    {
                        notSyncReplicaList.Add(new
                        {
                            NodeId1 = baseNodeId,
                            Leader1 = basePartition.Leader,
                            Replicas1 = string.Join(",", basePartition.Replicas.Select(r => r.ToString())),
                            ISRs1 = string.Join(",", basePartition.ISRs.Select(r => r.ToString())),

                            topicPartition.Topic,
                            topicPartition.PartitionId,

                            NodeId2 = nodePartitions[i].NodeId,
                            Leader2 = -1,
                            Replicas2 = string.Empty,
                            ISRs2 = string.Empty,
                        });

                        continue;
                    }

                    if (matchItem.Leader != basePartition.Leader
                        || matchItem.Replicas.SequenceEqual(basePartition.Replicas) == false
                        || matchItem.ISRs.SequenceEqual(basePartition.ISRs) == false)
                    {
                        notSyncReplicaList.Add(new
                        {
                            NodeId1 = baseNodeId,
                            Leader1 = basePartition.Leader,
                            Replicas1 = string.Join(",", basePartition.Replicas.Select(r => r.ToString())),
                            ISRs1 = string.Join(",", basePartition.ISRs.Select(r => r.ToString())),

                            topicPartition.Topic,
                            topicPartition.PartitionId,

                            NodeId2 = nodePartitions[i].NodeId,
                            Leader2 = matchItem.Leader,
                            Replicas2 = string.Join(",", matchItem.Replicas.Select(r => r.ToString())),
                            ISRs2 = string.Join(",", matchItem.ISRs.Select(r => r.ToString())),
                        });
                    }
                }
            }

            if (notSyncReplicaList.Any() == false)
                return Ok("All replicas are in sync");

            notSyncReplicaList = notSyncReplicaList
                .OrderBy(n => n.NodeId2)
                .ThenBy(n => n.Topic)
                .ThenBy(n => n.PartitionId)
                .ToList();

            return Ok(notSyncReplicaList);
        }
    }
}