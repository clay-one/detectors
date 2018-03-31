using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/pubsub")]
    [Route("redis/connection/{connectionId}/pubsub")]
    public class RedisPubSubController : Controller
    {
        [HttpGet("channels/{pattern}")]
        [HttpGet("channels/{pattern}.{format}")]
        public IActionResult GetChannelList(string connectionId, string pattern, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("channel/{channel}/numsub")]
        [HttpGet("channel/{channel}/numsub.{format}")]
        public IActionResult GetChannelNumSub(string connectionId, string channel, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("numpat")]
        [HttpGet("numpat.{format}")]
        public IActionResult GetNumPat(string connectionId, string channel, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}