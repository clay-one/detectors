using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}/topic/{topicId}")]
    public class TopicController : Controller
    {
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
            return Ok("Not implemented yet.");
        }
    }
}