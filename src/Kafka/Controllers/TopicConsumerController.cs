using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}/topic/{topicId}/consumer/{consumerId}")]
    public class TopicConsumerController : Controller
    {
        [HttpGet("lag/details")]
        public IActionResult GetConsumerLagDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("lag/total")]
        public IActionResult GetConsumerTotalLag(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
    }
}