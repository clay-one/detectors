using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/key/{key}")]
    [Route("redis/connection/{connectionId}/key/{key}")]
    public class RedisKeyController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisKeyController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("exists")]
        [HttpGet("exists.{format}")]
        public IActionResult GetKeyExists(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).KeyExists(key);
                return Ok(result);
            }
        }

        [HttpGet("dump")]
        [HttpGet("dump.{format}")]
        public IActionResult GetKeyDump(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).KeyDump(key);
                return result == null ? (IActionResult) NotFound() : Ok(result);
            }
        }

        [HttpGet("object/refcount")]
        [HttpGet("object/refcount.{format}")]
        public IActionResult GetObjectRefCount(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                // Not supported by StackExchange.Redis library (yet?)
                return Ok("Not implemented yet.");
            }
        }

        [HttpGet("object/encoding")]
        [HttpGet("object/encoding.{format}")]
        public IActionResult GetObjectEncoding(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                // Not supported by StackExchange.Redis library (yet?)
                return Ok("Not implemented yet.");
            }
        }

        [HttpGet("object/idletime")]
        [HttpGet("object/idletime.{format}")]
        public IActionResult GetObjectIdleTime(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                // Not supported by StackExchange.Redis library (yet?)
                return Ok("Not implemented yet.");
            }
        }

        [HttpGet("object/freq")]
        [HttpGet("object/freq.{format}")]
        public IActionResult GetObjectFreq(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                // Not supported by StackExchange.Redis library (yet?)
                return Ok("Not implemented yet.");
            }
        }

        [HttpGet("object/help")]
        [HttpGet("object/help.{format}")]
        public IActionResult GetObjectHelp(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                // Not supported by StackExchange.Redis library (yet?)
                return Ok("Not implemented yet.");
            }
        }

        [HttpGet("ttl")]
        [HttpGet("ttl.{format}")]
        public IActionResult GetKeyTimeToLive(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).KeyTimeToLive(key);
                return Ok(result.HasValue ? result.Value.ToString() : "(null)");
            }
        }

        [HttpGet("type")]
        [HttpGet("type.{format}")]
        public IActionResult GetKeyType(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).KeyType(key);
                return Ok(result.ToString());
            }
        }
    }
}