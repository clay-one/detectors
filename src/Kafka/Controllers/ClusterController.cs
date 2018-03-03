using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}")]
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

            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers", clusterConfig.BuildBrokersString()},
            };

            using (var producer = new Producer(config))
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