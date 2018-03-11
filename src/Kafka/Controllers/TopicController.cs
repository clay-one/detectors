using System;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}/topic/{topicId}")]
    public class TopicController : Controller
    {
        private readonly IConfiguration _configuration;
        public TopicController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetTopicInfo(string clusterId, string topicId)
        {
            return Ok($"Topic {topicId} info in cluster {clusterId}");
        }

        [HttpGet("health")]
        public IActionResult GetTopicHealth(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("partitions")]
        public IActionResult GetTopicPartitionList(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("consumers")]
        public IActionResult GetTopicConsumerList(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("offsets")]
        public IActionResult GetTopicOffsets(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("offsets/total")]
        public IActionResult GetTopicTotalOffset(string clusterId, string topicId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var topic = new KafkaTopicWrapper(clusterConfig, topicId))
            {
                return Ok($"[{topic.GetTotalMaxOffsets()}]");
            }
        }
        
        [HttpGet("offsets/total/rate/{duration?}")]
        public IActionResult GetTopicTotalOffsetRate(string clusterId, string topicId, string duration = "1m")
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            var durationTimeSpan = DurationStringParser.Parse(duration);
            if (durationTimeSpan <= TimeSpan.Zero)
                return BadRequest("Invalid time duration specified");
            
            using (var topic = new KafkaTopicWrapper(clusterConfig, topicId))
            {
                // Calculate the max offsets to add a sample
                topic.GetTotalMaxOffsets();

                var utcNow = DateTime.UtcNow;
                var rate = topic
                    .GetTotalMaxOffsetsRateCalculator()
                    .CalculateRateAverage(utcNow - durationTimeSpan, utcNow);
            
                return Ok($"[{rate}]");
            }
        }
    }
}