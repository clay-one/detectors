using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/db/{dbId}")]
    public class RedisDatabaseController : Controller
    {
        [HttpGet("keys/{keyPattern}")]
        [HttpGet("keys/{keyPattern}.{format}")]
        public IActionResult GetKeys(string connectionId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("random-key")]
        [HttpGet("random-key.{format}")]
        public IActionResult GetRandomKey(string connectionId, int dbId, string keyPattern)
        {
            return Ok("Not implemented yet.");
        }

    }
}