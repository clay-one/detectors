using System.Linq;
using Detectors.Redis.Configuration;
using Detectors.Redis.Util;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("keys/{keyPattern}")]
        [HttpGet("keys/{keyPattern}.{format}")]
        public IActionResult GetKeys(string connectionId, string keyPattern, int dbId = 0)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetFirstServer();
                if (server == null)
                    return NotFound();

                var keys = server.Keys(dbId, keyPattern);
                return Ok(keys.Select(k => k.ToString()).ToList());
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