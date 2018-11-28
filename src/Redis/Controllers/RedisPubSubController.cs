using System.Linq;
using Detectors.Redis.Configuration;
using Detectors.Redis.Util;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}/server/{hostAndPort}/pubsub")]
    [Route("redis/connection/{connectionId}/pubsub")]
    public class RedisPubSubController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisPubSubController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("channels")]
        [HttpGet("channels.{format}")]
        public IActionResult GetChannelList(string connectionId, string pattern = null, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var channels = redis.GetServerOrDefault(hostAndPort).SubscriptionChannels(pattern ?? default(RedisChannel));
                return Ok(channels.Select(c => c.ToString()));
            }
        }
        
        [HttpGet("channel/{channel}/numsub")]
        [HttpGet("channel/{channel}/numsub.{format}")]
        public IActionResult GetChannelNumSub(string connectionId, string channel, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var count = redis.GetServerOrDefault(hostAndPort).SubscriptionSubscriberCount(channel);
                return Ok(count);
            }
        }
        
        [HttpGet("numpat")]
        [HttpGet("numpat.{format}")]
        public IActionResult GetNumPat(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                if (redis == null)
                    return NotFound();

                var count = redis.GetServerOrDefault(hostAndPort).SubscriptionPatternCount();
                return Ok(count);
            }
        }
        
    }
}