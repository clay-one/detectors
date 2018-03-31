using System.Linq;
using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}")]
    public class RedisServerController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisServerController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ping")]
        [HttpGet("ping.{format}")]
        public IActionResult GetPingResponse(string connectionId)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();
                
                var response = redis.GetDatabase().Ping();
                return Ok(response.TotalSeconds);
            }
        }

        [HttpGet("echo/{message}")]
        [HttpGet("echo/{message}.{format}")]
        public IActionResult GetEchoResponse(string connectionId, string message)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("client-list")]
        [HttpGet("client-list.{format}")]
        public IActionResult GetClientList(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("config/{pattern}")]
        [HttpGet("config/{pattern}.{format}")]
        public IActionResult GetConfig(string connectionId, string pattern)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("size")]
        [HttpGet("size.{format}")]
        public IActionResult GetSize(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("info/{section?}")]
        [HttpGet("info/{section}.{format}")]
        [HttpGet("info.{format}")]
        public IActionResult GetInfo(string connectionId, string section = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var endpoint = redis.GetEndPoints().FirstOrDefault();
                if (endpoint == null)
                    return NotFound();

                var info = redis.GetServer(endpoint).Info(section);
                var result = info.SelectMany(g => g.Select(gg => new
                {
                    Group = g.Key,
                    gg.Key,
                    gg.Value
                })).ToList();
                
                return Ok(result);
            }
        }

        [HttpGet("lastsave")]
        [HttpGet("lastsave.{format}")]
        public IActionResult GetLastSave(string connectionId)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("slowlog/{count?}")]
        [HttpGet("slowlog/{count}.{format}")]
        [HttpGet("slowlog.{format}")]
        public IActionResult GetSlowLog(string connectionId, int count = 50)
        {
            return Ok("Not implemented yet.");
        }

        [HttpGet("time")]
        [HttpGet("time.{format}")]
        public IActionResult GetTime(string connectionId)
        {
            return Ok("Not implemented yet.");
        }
    }
}