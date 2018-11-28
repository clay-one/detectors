using System;
using System.Linq;
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
        [HttpGet("health.{format}")]
        public IActionResult GetTopicHealth(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("partitions")]
        [HttpGet("partitions.{format}")]
        public IActionResult GetTopicPartitionList(string clusterId, string topicId)
        {
            using (var topic = _configuration.BuildTopicWrapper(clusterId, topicId))
            {
                if (topic == null)
                    return NotFound();

                var result = topic.Metadata.Partitions.Select(p => new
                {
                    p.PartitionId,
                    p.Leader,
                    Replicas = string.Join(",", p.Replicas.Select(r => r.ToString())),
                    ISRs = string.Join(",", p.InSyncReplicas.Select(isr => isr.ToString())),
                }).ToList();
                
                return Ok(result);
            }
        }

        [HttpGet("consumers")]
        [HttpGet("consumers.{format}")]
        public IActionResult GetTopicConsumerList(string clusterId, string topicId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("offsets")]
        [HttpGet("offsets.{format}")]
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
                
                return Ok(topic.GetTotalHighOffsets());
            }
        }
        
        [HttpGet("offsets/total/rate/{duration?}")]
        [HttpGet("offsets/total/rate/{duration}.{format}")]
        [HttpGet("offsets/total/rate.{format}")]
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
                topic.GetTotalHighOffsets();

                var utcNow = DateTime.UtcNow;
                var rate = topic
                    .GetTotalMaxOffsetsRateCalculator()
                    .CalculateRateAverage(utcNow - durationTimeSpan, utcNow);
            
                return Ok(rate);
            }
        }
    }
}