using System.Linq;
using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/sorted-set/{key}")]
    [Route("redis/connection/{connectionId}/sorted-set/{key}")]
    public class RedisSortedSetController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisSortedSetController(RedisConnectionConfigCollection configuration)
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

                var result = redis.GetDatabase(dbId).SortedSetLength(key);
                return Ok(result);
            }
        }
        
        [HttpGet("count")]
        [HttpGet("count.{format}")]
        public IActionResult GetCountByScore(string connectionId, string key, double? min = null, double? max = null, 
            int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).SortedSetLength(key, 
                    min ?? double.NegativeInfinity, max ?? double.PositiveInfinity);
                return Ok(result);
            }
        }
        
        [HttpGet("range-by-score")]
        [HttpGet("range-by-score.{format}")]
        public IActionResult GetRangeByScore(string connectionId, string key, double? start = null, double? stop = null,
            long? skip = null, long? take = null, bool reverse = false, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var elements = redis.GetDatabase(dbId).SortedSetRangeByScoreWithScores(key,
                    start ?? double.NegativeInfinity, stop ?? double.PositiveInfinity, Exclude.None, 
                    reverse ? Order.Ascending : Order.Descending, skip ?? 0L, take ?? -1L);

                var result = elements.Select(e => new
                {
                    Member = e.Element.ToString(),
                    e.Score
                }).ToList();
                return Ok(result);
            }
        }
                
        [HttpGet("range-by-rank")]
        [HttpGet("range-by-rank.{format}")]
        public IActionResult GetRangeByRank(string connectionId, string key, long? start = null, long? stop = null,
            bool reverse = false, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var elements = redis.GetDatabase(dbId).SortedSetRangeByRankWithScores(key,
                    start ?? 0L, stop ?? -1L, reverse ? Order.Ascending : Order.Descending);

                var result = elements.Select(e => new
                {
                    Member = e.Element.ToString(),
                    e.Score
                }).ToList();
                return Ok(result);
            }
        }
        
        [HttpGet("range-by-value")]
        [HttpGet("range-by-value.{format}")]
        public IActionResult GetRangeByValue(string connectionId, string key, string start = null, string stop = null,
            long? skip = null, long? take = null, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var elements = redis.GetDatabase(dbId).SortedSetRangeByValue(key,
                    start ?? default(RedisValue), stop ?? default(RedisValue), Exclude.None, skip ?? 0L, take ?? -1L);

                return Ok(elements.Select(e => e.ToString()).ToList());
            }
        }

        [HttpGet("rank/{member}.{format?}")]
        [HttpGet("member/{member}/rank")]
        [HttpGet("member/{member}/rank.{format}")]
        public IActionResult GetRank(string connectionId, string key, string member, bool reverse = false, 
            int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).SortedSetRank(key, member, 
                    reverse ? Order.Ascending : Order.Descending);
                return Ok(result ?? -1L);
            }
        }
                
        [HttpGet("score/{member}.{format?}")]
        [HttpGet("member/{member}/score")]
        [HttpGet("member/{member}/score.{format}")]
        public IActionResult GetScore(string connectionId, string key, string member, int dbId = -1)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var result = redis.GetDatabase(dbId).SortedSetScore(key, member);
                return Ok(result);
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

                var members = redis.GetDatabase(dbId).SortedSetScan(key, pattern ?? default(RedisValue), count, cursor);
                var result = new
                {
                    (members as IScanningCursor)?.Cursor,
                    (members as IScanningCursor)?.PageOffset,
                    (members as IScanningCursor)?.PageSize,
                    Members = members.Select(e => new
                    {
                        Member = e.Element.ToString(),
                        e.Score
                    }).ToList()
                };

                return Ok(result);
            }
        }
    }
}