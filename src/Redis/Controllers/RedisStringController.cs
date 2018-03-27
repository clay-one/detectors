using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/string/{key}")]
    public class RedisStringController : Controller
    {
        [HttpGet("")]
        public IActionResult GetValue(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit/{offset}")]
        public IActionResult GetBit(string connectionId, int dbId, string key, int offset)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count")]
        public IActionResult GetBitCount(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count/from/{from}/to/{to}")]
        public IActionResult GetBitCountWithRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-position/{bit}")]
        [HttpGet("bit-position/{bit}/from/{from}")]
        [HttpGet("bit-position/{bit}/to/{to}")]
        [HttpGet("bit-position/{bit}/from/{from}/to/{to}")]
        public IActionResult GetBitPosition(string connectionId, int dbId, string key, bool bit, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        public IActionResult GetRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("length")]
        public IActionResult GetLength(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}