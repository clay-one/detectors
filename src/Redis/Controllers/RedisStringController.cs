using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/string/{key}")]
    [Route("redis/connection/{connectionId}/string/{key}")]
    public class RedisStringController : Controller
    {
        [HttpGet("value")]
        [HttpGet("value.{format}")]
        public IActionResult GetValue(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit/{offset}")]
        [HttpGet("bit/{offset}.{format}")]
        public IActionResult GetBit(string connectionId, string key, int offset, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count")]
        [HttpGet("bit-count.{format}")]
        public IActionResult GetBitCount(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count/from/{from}/to/{to}")]
        [HttpGet("bit-count/from/{from}/to/{to}.{format}")]
        public IActionResult GetBitCountWithRange(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-position/{bit}")]
        [HttpGet("bit-position/{bit}.{format}")]
        [HttpGet("bit-position/{bit}/from/{from}.{format?}")]
        [HttpGet("bit-position/{bit}/to/{to}.{format?}")]
        [HttpGet("bit-position/{bit}/from/{from}/to/{to}.{format?}")]
        public IActionResult GetBitPosition(string connectionId, string key, bool bit, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        [HttpGet("range/from/{from}/to/{to}.{format}")]
        public IActionResult GetRange(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("length")]
        [HttpGet("length.{format}")]
        public IActionResult GetLength(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
    }
}