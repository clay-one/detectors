using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("server/{serverId}/db/{dbId}/pubsub")]
    public class RedisPubSubController : Controller
    {
        [HttpGet("channels/{pattern}")]
        public IActionResult GetChannelList(string serverId, int dbId, string pattern)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("channel/{channel}/numsub")]
        public IActionResult GetChannelNumSub(string serverId, int dbId, string channel)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("numpat")]
        public IActionResult GetNumPat(string serverId, int dbId, string channel)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}