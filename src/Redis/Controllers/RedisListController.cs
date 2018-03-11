using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/server/{serverId}/db/{dbId}/list/{key}")]
    public class RedisListController : Controller
    {
        [HttpGet("length")]
        public IActionResult GetLength(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("index/{index}")]
        public IActionResult GetIndex(string serverId, int dbId, string key, int index)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        public IActionResult GetRange(string serverId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        
    }
}