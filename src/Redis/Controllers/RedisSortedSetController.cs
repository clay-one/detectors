using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/sorted-set/{key}")]
    [Route("redis/connection/{connectionId}/sorted-set/{key}")]
    public class RedisSortedSetController : Controller
    {
        [HttpGet("card")]
        [HttpGet("card.{format}")]
        public IActionResult GetCardinality(string connectionId, string key, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("count/from/{from}/to/{to}")]
        [HttpGet("count/from/{from}/to/{to}.{format}")]
        public IActionResult GetCountByScore(string connectionId, string key, string from, string to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/score/from/{from}/to/{to}")]
        [HttpGet("range/score/from/{from}/to/{to}.{format}")]
        public IActionResult GetRangeByScore(string connectionId, string key, string from, string to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/score/from/{from}/to/{to}")]
        [HttpGet("reverse-range/score/from/{from}/to/{to}.{format}")]
        public IActionResult GetReverseRangeByScore(string connectionId, string key, string from, string to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/order/from/{from}/to/{to}")]
        [HttpGet("range/order/from/{from}/to/{to}.{format}")]
        public IActionResult GetRange(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/order/from/{from}/to/{to}")]
        [HttpGet("reverse-range/order/from/{from}/to/{to}.{format}")]
        public IActionResult GetReverseRange(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("range/order/from/{from}/to/{to}/with-scores")]
        [HttpGet("range/order/from/{from}/to/{to}/with-scores.{format}")]
        public IActionResult GetRangeWithScores(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-range/order/from/{from}/to/{to}/with-scores")]
        [HttpGet("reverse-range/order/from/{from}/to/{to}/with-scores.{format}")]
        public IActionResult GetReverseRangeWithScores(string connectionId, string key, int from, int to, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("rank/{member}")]
        [HttpGet("rank/{member}.{format}")]
        public IActionResult GetRank(string connectionId, string key, string member, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("reverse-rank/{member}")]
        [HttpGet("reverse-rank/{member}.{format}")]
        public IActionResult GetReverseRank(string connectionId, string key, string member, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
        [HttpGet("score/{member}")]
        [HttpGet("score/{member}.{format}")]
        public IActionResult GetScore(string connectionId, string key, string member, int dbId = -1)
        {
            return Ok("Not implemented yet.");
        }
        
    }
}