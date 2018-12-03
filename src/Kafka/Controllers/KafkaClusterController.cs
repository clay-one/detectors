using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}")]
    public class KafkaClusterController : Controller
    {
        private readonly NotSyncReplicaLogic _notSyncReplicaLogic;
        private readonly KafkaClusterConfigCollection _configuration;

        public KafkaClusterController(KafkaClusterConfigCollection configuration, NotSyncReplicaLogic notSyncReplicaLogic)
        {
            _configuration = configuration;
            _notSyncReplicaLogic = notSyncReplicaLogic;
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
            try
            {
                var brokerPartitionList = _notSyncReplicaLogic.GetBrokerPartitionList(clusterId);
                if (brokerPartitionList == null || brokerPartitionList.Count <= 1)
                    return Ok("No replicas founds");

                var notSyncReplicaList = _notSyncReplicaLogic.GetNotSyncReplicaList(brokerPartitionList);
                return notSyncReplicaList.Any() ? Ok(notSyncReplicaList) : Ok("All replicas are in sync");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}