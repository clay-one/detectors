using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/list/{key}")]
    [Route("redis/connection/{connectionId}/list/{key}")]
    public class RedisListController : Controller
    {
        [HttpGet("length")]
        [HttpGet("length.{format}")]
        public IActionResult GetLength(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("index/{index}")]
        [HttpGet("index/{index}.{format}")]
        public IActionResult GetIndex(string connectionId, string key, int index, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        [HttpGet("range/from/{from}/to/{to}.{format}")]
        public IActionResult GetRange(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        
    }
}