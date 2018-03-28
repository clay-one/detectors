using System;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}/topic/{topicId}")]
    public class KafkaTopicController : Controller
    {
        private readonly KafkaClusterConfigCollection _configuration;
        public KafkaTopicController(KafkaClusterConfigCollection configuration)
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
        [HttpGet("offsets/total.{format}")]
        public IActionResult GetTopicTotalOffset(string clusterId, string topicId)
        {
            using (var topic = _configuration.BuildTopicWrapper(clusterId, topicId))
            {
                if (topic == null)
                    return NotFound();
                
                return Ok($"[{topic.GetTotalMaxOffsets()}]");
            }
        }
        
        [HttpGet("offsets/total/rate/{duration?}")]
        public IActionResult GetTopicTotalOffsetRate(string clusterId, string topicId, string duration = "1m")
        {
            var durationTimeSpan = DurationStringParser.Parse(duration);
            if (durationTimeSpan <= TimeSpan.Zero)
                return BadRequest("Invalid time duration specified");
            
            using (var topic = _configuration.BuildTopicWrapper(clusterId, topicId))
            {
                if (topic == null)
                    return NotFound();
                
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