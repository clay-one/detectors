using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}")]
    public class ClusterController : Controller
    {
        [HttpGet("")]
        public IActionResult GetClusterInfo(string clusterId)
        {
            return Ok($"Info for {clusterId}");
        }
        
        [HttpGet("topics")]
        public IActionResult GetTopicList(string clusterId)
        {
            return Ok($"List of topics in cluster {clusterId}");
        }
    }
}