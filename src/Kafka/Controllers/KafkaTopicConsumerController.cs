using System;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}/topic/{topicId}/consumer/{consumerId}")]
    public class KafkaTopicConsumerController : Controller
    {
        private readonly KafkaClusterConfigCollection _configuration;
        public KafkaTopicConsumerController(KafkaClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("lag/details")]
        [HttpGet("lag/details.{format}")]
        public IActionResult GetConsumerLagDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("lag/total")]
        [HttpGet("lag/total.{format}")]
        public IActionResult GetConsumerTotalLag(string clusterId, string topicId, string consumerId)
        {
            using (var topic = _configuration.BuildTopicConsumerWrapper(clusterId, topicId, consumerId))
            {
                if (topic == null)
                    return NotFound();

                var totalCommit = topic.GetTotalCommitted();
                var totalMaxOffets = topic.GetTotalMaxOffsets();

                var result = totalMaxOffets - totalCommit;
                return Ok($"[{result}]");
            }
        }
        
        [HttpGet("commit/details")]
        [HttpGet("commit/details.{format}")]
        public IActionResult GetConsumerCommitDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("commit/total")]
        [HttpGet("commit/total.{format}")]
        public IActionResult GetConsumerTotalCommit(string clusterId, string topicId, string consumerId)
        {
            using (var topic = _configuration.BuildTopicConsumerWrapper(clusterId, topicId, consumerId))
            {
                if (topic == null)
                    return NotFound();
                
                return Ok($"[{topic.GetTotalCommitted()}]");
            }
        }

        [HttpGet("commit/total/rate/{duration?}")]
        [HttpGet("commit/total/rate/{duration}.{format}")]
        [HttpGet("commit/total/rate.{format}")]
        public IActionResult GetTopicTotalOffsetRate(string clusterId, string topicId, string consumerId, 
            string duration = "1m")
        {
            var durationTimeSpan = DurationStringParser.Parse(duration);
            if (durationTimeSpan <= TimeSpan.Zero)
                return BadRequest("Invalid time duration specified");
            
            using (var topic = _configuration.BuildTopicConsumerWrapper(clusterId, topicId, consumerId))
            {
                if (topic == null)
                    return NotFound();
                
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