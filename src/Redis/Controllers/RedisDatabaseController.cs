using Detectors.Redis.Configuration;
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
        public IActionResult GetKeys(string connectionId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
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

    }
}