using System;
using System.Linq;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}")]
    public class KafkaClusterController : Controller
    {
        private readonly IConfiguration _configuration;
        public KafkaClusterController(IConfiguration configuration)
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
            var clusterConfig = _configuration.GetKafkaCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(5));
                return Ok(md);
            }
        }
        
        [HttpGet("topics")]
        [HttpGet("topics.{format}")]
        public IActionResult GetTopicList(string clusterId)
        {
            var clusterConfig = _configuration.GetKafkaCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(5));
                var resultObject = md.Topics.Select(t => new
                {
                    t.Topic,
                    PartitionCount = t.Partitions.Count
                });
                return Ok(resultObject);
            }
        }
        
        [HttpGet("groups")]
        [HttpGet("groups.{format}")]
        public IActionResult GetGroupList(string clusterId)
        {
            var clusterConfig = _configuration.GetKafkaCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                // Dummy calls to GetMetadata to avoid "Broker transport failure" issue
                producer.GetMetadata();
                producer.GetMetadata();
                
                var groups = producer.ListGroups(TimeSpan.FromSeconds(5));

                var resultObject = groups.Select(g => new
                {
                    g.Group,
                    g.State,
                    g.Protocol,
                    g.ProtocolType,
                    MemberCount = g.Members.Count
                });
                
                return Ok(resultObject);
            }
        }
    }
}