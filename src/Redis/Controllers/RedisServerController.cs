using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("server/{serverId}")]
    public class RedisServerController : Controller
    {
        [HttpGet("ping")]
        public IActionResult GetPingResponse(string serverId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("echo/{message}")]
        public IActionResult GetEchoResponse(string serverId, string message)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("client-list")]
        public IActionResult GetClientList(string serverId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("config/{pattern}")]
        public IActionResult GetConfig(string serverId, string pattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("size")]
        public IActionResult GetSize(string serverId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("info/{section?}")]
        public IActionResult GetInfo(string serverId, string section = null)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("lastsave")]
        public IActionResult GetLastSave(string serverId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("slowlog/{count?}")]
        public IActionResult GetSlowLog(string serverId, int count = 50)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("time")]
        public IActionResult GetTime(string serverId)
        {
            return Ok("Not implemented yet.");
        }
    }
}