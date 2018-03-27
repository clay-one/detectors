using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/sorted-set/{key}")]
    public class RedisSortedSetController : Controller
    {
        [HttpGet("card")]
        public IActionResult GetCardinality(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("count/from/{from}/to/{to}")]
        public IActionResult GetCountByScore(string connectionId, int dbId, string key, string from, string to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/score/from/{from}/to/{to}")]
        public IActionResult GetRangeByScore(string connectionId, int dbId, string key, string from, string to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/score/from/{from}/to/{to}")]
        public IActionResult GetReverseRangeByScore(string connectionId, int dbId, string key, string from, string to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/order/from/{from}/to/{to}")]
        public IActionResult GetRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/order/from/{from}/to/{to}")]
        public IActionResult GetReverseRange(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/order/from/{from}/to/{to}/with-scores")]
        public IActionResult GetRangeWithScores(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/order/from/{from}/to/{to}/with-scores")]
        public IActionResult GetReverseRangeWithScores(string connectionId, int dbId, string key, int from, int to)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("rank/{member}")]
        public IActionResult GetRank(string connectionId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-rank/{member}")]
        public IActionResult GetReverseRank(string connectionId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("score/{member}")]
        public IActionResult GetScore(string connectionId, int dbId, string key, string member)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}