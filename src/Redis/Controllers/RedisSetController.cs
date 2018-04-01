using System.Linq;
using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/set/{key}")]
    [Route("redis/connection/{connectionId}/set/{key}")]
    public class RedisSetController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisSetController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("card")]
        [HttpGet("card.{format}")]
        public IActionResult GetCardinality(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).SetLength(key);
                return Ok(result);
            }
        }
        
        [HttpGet("ismember/{member}")]
        [HttpGet("ismember/{member}.{format}")]
        public IActionResult GetIsMember(string connectionId, string key, string member, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).SetContains(key, member);
                return Ok(result);
            }
        }
        
        [HttpGet("all-members")]
        [HttpGet("all-members.{format}")]
        public IActionResult GetAllMembers(string connectionId, string key, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var members = redis.GetDatabase(dbId).SetMembers(key);
                return Ok(members.Select(m => m.ToString()).ToList());
            }
        }

        [HttpGet("scan-members")]
        [HttpGet("scan-members.{format}")]
        public IActionResult ScanMembers(string connectionId, string key, string pattern = null, int count = 10, 
            long cursor = 0L, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var members = redis.GetDatabase(dbId).SetScan(key, pattern ?? default(RedisValue), count, cursor);
                var result = new
                {
                    (members as IScanningCursor)?.Cursor,
                    (members as IScanningCursor)?.PageOffset,
                    (members as IScanningCursor)?.PageSize,
                    Members = members.Select(k => k.ToString()).ToList()
                };

                return Ok(result);
            }
        }
        
        [HttpGet("random-members")]
        [HttpGet("random-members.{format}")]
        public IActionResult GetRandomMembers(string connectionId, string key, int count = 1, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var members = redis.GetDatabase(dbId).SetRandomMembers(key, count);
                return Ok(members.Select(m => m.ToString()).ToList());
            }
        }
        
    }
}