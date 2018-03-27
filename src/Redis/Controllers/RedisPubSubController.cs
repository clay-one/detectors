using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/pubsub")]
    public class RedisPubSubController : Controller
    {
        [HttpGet("channels/{pattern}")]
        public IActionResult GetChannelList(string connectionId, int dbId, string pattern)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("channel/{channel}/numsub")]
        public IActionResult GetChannelNumSub(string connectionId, int dbId, string channel)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("numpat")]
        public IActionResult GetNumPat(string connectionId, int dbId, string channel)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}