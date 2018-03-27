using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/key/{key}")]
    public class RedisKeyController : Controller
    {
        [HttpGet("exists")]
        public IActionResult GetKeyExists(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("dump")]
        public IActionResult GetKeyDump(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/refcount")]
        public IActionResult GetObjectRefCount(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/encoding")]
        public IActionResult GetObjectEncoding(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/idletime")]
        public IActionResult GetObjectIdleTime(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/freq")]
        public IActionResult GetObjectFreq(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/help")]
        public IActionResult GetObjectHelp(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("ttl")]
        public IActionResult GetKeyTimeToLive(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("type")]
        public IActionResult GetKeyType(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}