using System;
using System.Threading;
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
                var totalMaxOffets = topic.GetTotalHighOffsets();

                var result = totalMaxOffets - totalCommit;
                return Ok(result);
            }
        }
        
        [HttpGet("lag/total/seconds/{duration?}")]
        [HttpGet("lag/total/seconds/{duration}.{format}")]
        [HttpGet("lag/total/seconds.{format}")]
        public IActionResult GetConsumerTotalRelativeLag(string clusterId, string topicId, string consumerId, 
            string duration = "1m")
        {
            var durationTimeSpan = DurationStringParser.Parse(duration);
            if (durationTimeSpan <= TimeSpan.Zero)
                return BadRequest("Invalid time duration specified");
            
            using (var topic = _configuration.BuildTopicConsumerWrapper(clusterId, topicId, consumerId))
            {
                if (topic == null)
                    return NotFound();

                var rateCalculator = topic.GetTotalCommittedRateCalculator();
                if (rateCalculator.SampleCount <= 0)
                {
                    topic.GetTotalCommitted();
                    Thread.Sleep(8000);
                }
                
                var totalCommit = topic.GetTotalCommitted();
                var utcNow = DateTime.UtcNow;
                var rate = rateCalculator.CalculateRateAverage(utcNow - durationTimeSpan, utcNow);

                if (Math.Abs(rate) < 0.01)
                {
                    Thread.Sleep(8000);
                    totalCommit = topic.GetTotalCommitted();
                    utcNow = DateTime.UtcNow;
                    rate = rateCalculator.CalculateRateAverage(utcNow - durationTimeSpan, utcNow);
                }

                var totalMaxOffets = topic.GetTotalHighOffsets();
                var lag = totalMaxOffets - totalCommit;
                
                if (lag == 0)
                    return Ok((double)0);
                
                var result = (Math.Abs(rate) < 0.01 ? 1440 : lag / rate) * 60;
                
                return Ok(result);
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
                
                return Ok(topic.GetTotalCommitted());
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
            
                return Ok(rate);
            }
        }
        
    }
}