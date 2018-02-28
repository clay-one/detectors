using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("server/{serverId}/db/{dbId}/string/{key}")]
    public class RedisStringController : Controller
    {
        [HttpGet("")]
        public IActionResult GetValue(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit/{offset}")]
        public IActionResult GetBit(string serverId, int dbId, string key, int offset)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count")]
        public IActionResult GetBitCount(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count/from/{from}/to/{to}")]
        public IActionResult GetBitCountWithRange(string serverId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-position/{bit}")]
        [HttpGet("bit-position/{bit}/from/{from}")]
        [HttpGet("bit-position/{bit}/to/{to}")]
        [HttpGet("bit-position/{bit}/from/{from}/to/{to}")]
        public IActionResult GetBitPosition(string serverId, int dbId, string key, bool bit, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        public IActionResult GetRange(string serverId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("length")]
        public IActionResult GetLength(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}