using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}/topic/{topicId}/consumer/{consumerId}")]
    public class TopicConsumerController : Controller
    {
        private readonly IConfiguration _configuration;
        public TopicConsumerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("lag/details")]
        public IActionResult GetConsumerLagDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("lag/total")]
        public async Task<IActionResult> GetConsumerTotalLag(string clusterId, string topicId, string consumerId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers", clusterConfig.BuildBrokersString()},
                {"group.id", consumerId}
            };

            using (var consumer = new Consumer(config))
            {
                using (var producer = new Producer(config))
                {
                    var md = producer.GetMetadata(false, topicId, TimeSpan.FromSeconds(5));
                    var tpos = consumer.Committed(md.Topics[0].Partitions.Select(p => new TopicPartition(topicId, p.PartitionId)),
                        TimeSpan.FromSeconds(5));

                    return Ok(tpos.AsParallel().Select(tpo =>
                    {
                        var wo = consumer.QueryWatermarkOffsets(tpo.TopicPartition);
                        return wo.High - tpo.Offset.Value;
                    }).Sum());
                }
            }
        }
        
        [HttpGet("commit/details")]
        public IActionResult GetConsumerCommitDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("commit/total")]
        public IActionResult GetConsumerTotalCommit(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}