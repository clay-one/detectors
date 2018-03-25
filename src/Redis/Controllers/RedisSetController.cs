using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/set/{key}")]
    public class RedisSetController : Controller
    {
        [HttpGet("card")]
        public IActionResult GetCardinality(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("ismember/{member}")]
        public IActionResult GetIsMember(string connectionId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("members")]
        public IActionResult GetMembers(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("random-member")]
        public IActionResult GetRandomMember(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}