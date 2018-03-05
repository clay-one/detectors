using System.Linq;
using System.Threading.Tasks;
using Detectors.Kafka.Configuration;
using Detectors.Kafka.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("cluster/{clusterId}/topic/{topicId}/consumer/{consumerId}")]
    public class TopicConsumerController : Controller
    {
        private readonly IConfiguration _configuration;
        public TopicConsumerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("lag/details")]
        public IActionResult GetConsumerLagDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("lag/total")]
        public IActionResult GetConsumerTotalLag(string clusterId, string topicId, string consumerId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var topic = new KafkaTopicConsumerWrapper(clusterConfig, topicId, consumerId))
            {
                var result = Task.WhenAll(
                        Task.Run(() => topic.GetTotalMaxOffsets()),
                        Task.Run(() => -topic.TotalCommitted)
                    )
                    .GetAwaiter().GetResult().Sum();

                return Ok(result);
            }
        }
        
        [HttpGet("commit/details")]
        public IActionResult GetConsumerCommitDetails(string clusterId, string topicId, string consumerId)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("commit/total")]
        public IActionResult GetConsumerTotalCommit(string clusterId, string topicId, string consumerId)
        {
            var clusterConfig = _configuration.GetCluster(clusterId);
            if (clusterConfig == null)
                return NotFound();

            using (var topic = new KafkaTopicConsumerWrapper(clusterConfig, topicId, consumerId))
            {
                return Ok(topic.TotalCommitted);
            }
        }
        
    }
}