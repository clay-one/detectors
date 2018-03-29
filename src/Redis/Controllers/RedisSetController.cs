using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/set/{key}")]
    public class RedisSetController : Controller
    {
        [HttpGet("card")]
        [HttpGet("card.{format}")]
        public IActionResult GetCardinality(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("ismember/{member}")]
        [HttpGet("ismember/{member}.{format}")]
        public IActionResult GetIsMember(string connectionId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("members")]
        [HttpGet("members.{format}")]
        public IActionResult GetMembers(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("random-member")]
        [HttpGet("random-member.{format}")]
        public IActionResult GetRandomMember(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}