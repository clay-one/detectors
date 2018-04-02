using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/string/{key}")]
    [Route("redis/connection/{connectionId}/string/{key}")]
    public class RedisStringController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisStringController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("value")]
        [HttpGet("value.{format}")]
        public IActionResult GetValue(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringGet(key);
                return Ok(result.ToString());
            }
        }
        
        [HttpGet("bit/{offset}")]
        [HttpGet("bit/{offset}.{format}")]
        public IActionResult GetBit(string connectionId, string key, long offset, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringGetBit(key, offset);
                return Ok(result.ToString());
            }
        }
        
        [HttpGet("bit-count")]
        [HttpGet("bit-count.{format}")]
        public IActionResult GetBitCount(string connectionId, string key, 
            long start = 0L, long end = -1L, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringBitCount(key, start, end);
                return Ok(result);
            }
        }
                
        [HttpGet("bit-position/{bit}")]
        [HttpGet("bit-position/{bit}.{format}")]
        public IActionResult GetBitPosition(string connectionId, string key, bool bit, 
            long start = 0L, long end = -1L, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringBitPosition(key, bit, start, end);
                return Ok(result);
            }
        }
        
        [HttpGet("range")]
        [HttpGet("range.{format}")]
        public IActionResult GetRange(string connectionId, string key, long start = 0, long end = -1, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringGetRange(key, start, end);
                return Ok(result.ToString());
            }
        }
        
        [HttpGet("length")]
        [HttpGet("length.{format}")]
        public IActionResult GetLength(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).StringLength(key);
                return Ok(result);
            }
        }
    }
}