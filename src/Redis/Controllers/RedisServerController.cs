using System.Linq;
using Detectors.Redis.Configuration;
using Detectors.Redis.Util;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.Redis.Controllers
{
    [Route("redis/connection/{connectionId}")]
    [Route("redis/connection/{connectionId}/server/{hostAndPort}")]
    public class RedisServerController : Controller
    {
        private readonly RedisConnectionConfigCollection _configuration;
        public RedisServerController(RedisConnectionConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ping")]
        [HttpGet("ping.{format}")]
        public IActionResult GetPingResponse(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var response = server.Ping();
                return Ok(response.TotalSeconds);
            }
        }

        [HttpGet("echo/{message}")]
        [HttpGet("echo/{message}.{format}")]
        public IActionResult GetEchoResponse(string connectionId, string message, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var response = server.Echo(message);
                return Ok(response.ToString());
            }
        }

        [HttpGet("client-list")]
        [HttpGet("client-list.{format}")]
        public IActionResult GetClientList(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var clientList = server.ClientList();
                var result = clientList.Select(c => new
                {
                    c.Id,
                    c.Name,
                    Address = c.Address.ToString(),
                    Type = c.ClientType.ToString(),
                    c.AgeSeconds,
                    c.IdleSeconds,
                    Flags = c.FlagsRaw,
                    c.LastCommand,
                    SubCount = c.SubscriptionCount,
                    TxCmdLength = c.TransactionCommandLength
                }).ToList();
                return Ok(result);
            }
        }
        
        [HttpGet("raw-client-list")]
        [HttpGet("raw-client-list.{format}")]
        public IActionResult GetRawClientList(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var clientList = server.ClientList();
                return Ok(clientList.Select(c => c.Raw).ToList());
            }
        }

        [HttpGet("config")]
        [HttpGet("config.{format}")]
        public IActionResult GetConfig(string connectionId, string pattern = null, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var response = string.IsNullOrWhiteSpace(pattern) ? server.ConfigGet() : server.ConfigGet(pattern);
                return Ok(response);
            }
        }

        [HttpGet("info/{section?}")]
        [HttpGet("info/{section}.{format}")]
        [HttpGet("info.{format}")]
        public IActionResult GetInfo(string connectionId, string section = null, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var info = server.Info(section);
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
        public IActionResult GetLastSave(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var response = server.LastSave();
                return Ok(response.ToString("O"));
            }
        }

        [HttpGet("lastsave/elapsed")]
        [HttpGet("lastsave/elapsed.{format}")]
        public IActionResult GetElapsedSinceLastSave(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var lastSave = server.LastSave();
                var time = server.Time();
                return Ok((time - lastSave).TotalSeconds);
            }
        }

        [HttpGet("slowlog")]
        [HttpGet("slowlog.{format}")]
        public IActionResult GetSlowLog(string connectionId, int count = 10, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var slowLog = server.SlowlogGet(count);
                var result = slowLog.Select(l => new
                {
                    l.UniqueId,
                    Time = l.Time.ToString("O"),
                    Duration = l.Duration.TotalSeconds,
                    Command = string.Join(" ", l.Arguments.Select(a => a.ToString())).Truncate(40)
                }).ToList();
                
                return Ok(result);
            }
        }

        [HttpGet("time")]
        [HttpGet("time.{format}")]
        public IActionResult GetTime(string connectionId, string hostAndPort = null)
        {
            using (var redis = _configuration.BuildMultiplexer(connectionId))
            {
                var server = redis.GetServerOrDefault(hostAndPort);
                if (server == null)
                    return NotFound();

                var response = server.Time();
                return Ok(response.ToString("O"));
            }
        }
    }
}