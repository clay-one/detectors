using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/list/{key}")]
    public class RedisListController : Controller
    {
        [HttpGet("length")]
        [HttpGet("length.{format}")]
        public IActionResult GetLength(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("index/{index}")]
        [HttpGet("index/{index}.{format}")]
        public IActionResult GetIndex(string connectionId, int dbId, string key, int index)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        [HttpGet("range/from/{from}/to/{to}.{format}")]
        public IActionResult GetRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        
    }
}