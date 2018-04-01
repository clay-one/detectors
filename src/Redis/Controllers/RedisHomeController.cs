using System.Linq;
using Detectors.Redis.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis")]
    public class RedisHomeController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisHomeController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetHomepage()
        {
            return Redirect("/api/redis/connections");
        }
        
        [HttpGet("connections")]
        [HttpGet("connections.{format}")]
        public IActionResult GetClusterList()
        {
            return Ok(_configuration.GetAllRedisConnectionConfigs().Select(c => new {c.Id}).ToList());
        }
        
    }
}