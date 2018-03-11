using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/server/{serverId}/db/{dbId}/key/{key}")]
    public class RedisKeyController : Controller
    {
        [HttpGet("exists")]
        public IActionResult GetKeyExists(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("dump")]
        public IActionResult GetKeyDump(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/refcount")]
        public IActionResult GetObjectRefCount(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/encoding")]
        public IActionResult GetObjectEncoding(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/idletime")]
        public IActionResult GetObjectIdleTime(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/freq")]
        public IActionResult GetObjectFreq(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("object/help")]
        public IActionResult GetObjectHelp(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("ttl")]
        public IActionResult GetKeyTimeToLive(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("type")]
        public IActionResult GetKeyType(string serverId, int dbId, string key)
        {
            return Ok("Not implemented yet.");
        }
    }
}