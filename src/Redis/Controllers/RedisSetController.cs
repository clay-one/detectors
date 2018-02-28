using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("server/{serverId}/db/{dbId}/set/{key}")]
    public class RedisSetController : Controller
    {
        [HttpGet("card")]
        public IActionResult GetCardinality(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("ismember/{member}")]
        public IActionResult GetIsMember(string serverId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("members")]
        public IActionResult GetMembers(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("random-member")]
        public IActionResult GetRandomMember(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}