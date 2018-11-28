using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka/cluster/{clusterId}/topic/{topicId}/partition/{partitionId}")]
    public class KafkaTopicPartitionController : Controller
    {
        [HttpGet("offsets")]
        [HttpGet("offsets.{format}")]
        public IActionResult GetTopicPartitionOffsets(string clusterId, string topicId, string partitionId)
        {
            return Ok("Not implemented yet.");
        }
    }
}