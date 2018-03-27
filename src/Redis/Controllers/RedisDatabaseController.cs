using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}")]
    public class RedisDatabaseController : Controller
    {
        [HttpGet("keys/{keyPattern}")]
        public IActionResult GetKeys(string connectionId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("random-key")]
        public IActionResult GetRandomKey(string connectionId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

    }
}