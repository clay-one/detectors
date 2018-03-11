using System;
using System.Linq;
using System.Threading.Tasks;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}/topic/{topicId}/consumer/{consumerId}")]
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
        public IActionResult GetConsumerTotalLag(string clusterId, string topicId, string consumerId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var topic = new KafkaTopicConsumerWrapper(clusterConfig, topicId, consumerId))
            {
                var result = Task.WhenAll(
                        Task.Run(() => topic.GetTotalMaxOffsets()),
                        Task.Run(() => -topic.GetTotalCommitted())
                    )
                    .GetAwaiter().GetResult().Sum();

                return Ok($"[{result}]");
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
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var topic = new KafkaTopicConsumerWrapper(clusterConfig, topicId, consumerId))
            {
                return Ok($"[{topic.GetTotalCommitted()}]");
            }
        }

        [HttpGet("commit/total/rate/{duration?}")]
        public IActionResult GetTopicTotalOffsetRate(string clusterId, string topicId, string consumerId, 
            string duration = "1m")
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            var durationTimeSpan = DurationStringParser.Parse(duration);
            if (durationTimeSpan <= TimeSpan.Zero)
                return BadRequest("Invalid time duration specified");
            
            using (var topic = new KafkaTopicConsumerWrapper(clusterConfig, topicId, consumerId))
            {
                // Calculate the committed value to add a sample
                topic.GetTotalCommitted();

                var utcNow = DateTime.UtcNow;
                var rate = topic
                    .GetTotalCommittedRateCalculator()
                    .CalculateRateAverage(utcNow - durationTimeSpan, utcNow);
            
                return Ok($"[{rate}]");
            }
        }
        
    }
}