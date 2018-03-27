using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}")]
    public class RedisServerController : Controller
    {
        private readonly IConfiguration _configuration;
        public RedisServerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ping")]
        public IActionResult GetPingResponse(string connectionId)
        {
            var connectionConfig = _configuration.GetRedisConnection(connectionId);
            if (connectionConfig == null)
                return NotFound();

            using (var redis = connectionConfig.BuildMultiplexer())
            {
                var response = redis.GetDatabase().Ping();
                return Ok(response.TotalSeconds);
            }
        }

        [HttpGet("echo/{message}")]
        public IActionResult GetEchoResponse(string connectionId, string message)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("client-list")]
        public IActionResult GetClientList(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("config/{pattern}")]
        public IActionResult GetConfig(string connectionId, string pattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("size")]
        public IActionResult GetSize(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("info/{section?}")]
        public IActionResult GetInfo(string connectionId, string section = null)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("lastsave")]
        public IActionResult GetLastSave(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("slowlog/{count?}")]
        public IActionResult GetSlowLog(string connectionId, int count = 50)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("time")]
        public IActionResult GetTime(string connectionId)
        {
            return Ok("Not implemented yet.");
        }
    }
}