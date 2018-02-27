using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}/topic/{topicId}/partition/{partitionId}")]
    public class TopicPartitionController : Controller
    {
        [HttpGet("offsets")]
        public IActionResult GetTopicPartitionOffsets(string clusterId, string topicId, string partitionId)
        {
            return Ok("Not implemented yet.");
        }
    }
}