using System;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
                return Ok(md);
            }
        }
        
        [HttpGet("topics")]
        public IActionResult GetTopicList(string clusterId)
        {
            return Ok($"List of topics in cluster {clusterId}");
        }
    }
}