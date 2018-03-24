using System;
using System.Linq;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}")]
    public class ClusterController : Controller
    {
        private readonly IConfiguration _configuration;
        public ClusterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetClusterInfo(string clusterId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(5));
                var result = JsonConvert.SerializeObject(md, Formatting.Indented);
                return Ok(result);
            }
        }
        
        [HttpGet("topics")]
        public IActionResult GetTopicList(string clusterId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                var md = producer.GetMetadata(true, null, TimeSpan.FromSeconds(5));
                var resultObject = new
                {
                    TopicCount = md.Topics.Count,
                    Topics = md.Topics.Select(t => new
                    {
                        t.Topic,
                        PartitionCount = t.Partitions.Count
                    }).ToList()
                };
                var result = JsonConvert.SerializeObject(resultObject, Formatting.Indented);
                return Ok(result);
            }
        }
        
        [HttpGet("groups")]
        public IActionResult GetGroupList(string clusterId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var producer = clusterConfig.BuildProducer())
            {
                // Dummy calls to GetMetadata to avoid "Broker transport failure" issue
                producer.GetMetadata();
                producer.GetMetadata();
                
                var groups = producer.ListGroups(TimeSpan.FromSeconds(5));

                var resultObject = new
                {
                    GroupCount = groups.Count,
                    Groups = groups.Select(g => new
                    {
                        g.Group,
                        g.State,
                        g.Protocol,
                        g.ProtocolType,
                        MemberCount = g.Members.Count
                    })
                };
                
                var result = JsonConvert.SerializeObject(resultObject, Formatting.Indented);
                return Ok(result);
            }
        }
    }
}