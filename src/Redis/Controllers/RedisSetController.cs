using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/set/{key}")]
    [Route("redis/connection/{connectionId}/set/{key}")]
    public class RedisSetController : Controller
    {
        [HttpGet("card")]
        [HttpGet("card.{format}")]
        public IActionResult GetCardinality(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("ismember/{member}")]
        [HttpGet("ismember/{member}.{format}")]
        public IActionResult GetIsMember(string connectionId, string key, string member, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("members")]
        [HttpGet("members.{format}")]
        public IActionResult GetMembers(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("random-member")]
        [HttpGet("random-member.{format}")]
        public IActionResult GetRandomMember(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}