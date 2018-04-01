using System.Linq;
using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}")]
    public class RedisConnectionController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisConnectionController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetConnection(string connectionId)
        {
            return Redirect($"{connectionId}/status");
        }
        
        [HttpGet("servers")]
        [HttpGet("servers.{format}")]
        public IActionResult GetServers(string connectionId)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var endPoints = redis.GetEndPoints();
                var result = endPoints.Select(ep => new
                {
                    EndPoint = ep.ToString()
                }).ToList();
                
                return Ok(result);
            }
        }

        [HttpGet("status")]
        [HttpGet("status.{format}")]
        public IActionResult GetStatus(string connectionId)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var status = redis.GetStatus();
                return Ok(status);
            }
        }
    }
}