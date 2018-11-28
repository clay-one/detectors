using System.Linq;
using Detectors.Redis.Configuration;
using Detectors.Redis.Util;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}")]
    [Route("redis/connection/{connectionId}")]
    public class RedisDatabaseController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisDatabaseController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("keys")]
        [HttpGet("keys.{format}")]
        public IActionResult GetKeys(string connectionId, string pattern = null, int count = int.MaxValue, int dbId = 0)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetFirstServer();
                if (server == null)
                    return NotFound();

                var keys = server.Keys(dbId, pattern ?? default(RedisValue), count);
                return Ok(keys.Select(k => k.ToString()).ToList());
            }
        }
        
        [HttpGet("scan-keys")]
        [HttpGet("scan-keys.{format}")]
        public IActionResult ScanKeys(string connectionId, string pattern = null, int count = 10, long cursor = 0L, 
            int dbId = 0)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetFirstServer();
                if (server == null)
                    return NotFound();

                var keys = server.Keys(dbId, pattern ?? default(RedisValue), count, cursor);
                var result = new
                {
                    (keys as IScanningCursor)?.Cursor,
                    (keys as IScanningCursor)?.PageOffset,
                    (keys as IScanningCursor)?.PageSize,
                    Keys = keys.Select(k => k.ToString()).ToList()
                };
                
                return Ok(result);
            }
        }

        [HttpGet("random-key")]
        [HttpGet("random-key.{format}")]
        public IActionResult GetRandomKey(string connectionId, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var key = redis.GetDatabase(dbId).KeyRandom();
                return Ok(key.ToString());
            }
        }

        [HttpGet("size")]
        [HttpGet("size.{format}")]
        public IActionResult GetSize(string connectionId, int dbId = 0)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetFirstServer();
                if (server == null)
                    return NotFound();

                var response = server.DatabaseSize(dbId);
                return Ok(response.ToString());
            }
        }

    }
}