using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/server/{serverId}/db/{dbId}")]
    public class RedisDatabaseController : Controller
    {
        [HttpGet("keys/{keyPattern}")]
        public IActionResult GetKeys(string serverId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("random-key")]
        public IActionResult GetRandomKey(string serverId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

    }
}