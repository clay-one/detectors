using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/string/{key}")]
    public class RedisStringController : Controller
    {
        [HttpGet("value")]
        [HttpGet("value.{format}")]
        public IActionResult GetValue(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit/{offset}")]
        [HttpGet("bit/{offset}.{format}")]
        public IActionResult GetBit(string connectionId, int dbId, string key, int offset)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count")]
        [HttpGet("bit-count.{format}")]
        public IActionResult GetBitCount(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-count/from/{from}/to/{to}")]
        [HttpGet("bit-count/from/{from}/to/{to}.{format}")]
        public IActionResult GetBitCountWithRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("bit-position/{bit}")]
        [HttpGet("bit-position/{bit}.{format}")]
        [HttpGet("bit-position/{bit}/from/{from}.{format?}")]
        [HttpGet("bit-position/{bit}/to/{to}.{format?}")]
        [HttpGet("bit-position/{bit}/from/{from}/to/{to}.{format?}")]
        public IActionResult GetBitPosition(string connectionId, int dbId, string key, bool bit, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/from/{from}/to/{to}")]
        [HttpGet("range/from/{from}/to/{to}.{format}")]
        public IActionResult GetRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("length")]
        [HttpGet("length.{format}")]
        public IActionResult GetLength(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}