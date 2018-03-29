using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}/key/{key}")]
    public class RedisKeyController : Controller
    {
        [HttpGet("exists")]
        [HttpGet("exists.{format}")]
        public IActionResult GetKeyExists(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("dump")]
        [HttpGet("dump.{format}")]
        public IActionResult GetKeyDump(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/refcount")]
        [HttpGet("object/refcount.{format}")]
        public IActionResult GetObjectRefCount(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/encoding")]
        [HttpGet("object/encoding.{format}")]
        public IActionResult GetObjectEncoding(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/idletime")]
        [HttpGet("object/idletime.{format}")]
        public IActionResult GetObjectIdleTime(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/freq")]
        [HttpGet("object/freq.{format}")]
        public IActionResult GetObjectFreq(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/help")]
        [HttpGet("object/help.{format}")]
        public IActionResult GetObjectHelp(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("ttl")]
        [HttpGet("ttl.{format}")]
        public IActionResult GetKeyTimeToLive(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("type")]
        [HttpGet("type.{format}")]
        public IActionResult GetKeyType(string connectionId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}