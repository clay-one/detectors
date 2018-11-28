using System.Linq;
using System.Threading.Tasks;
using Detectors.MongoDB.Configuration;
using Detectors.MongoDB.Models;
using Detectors.MongoDB.Controllers.Dto;
using Detectors.MongoDB.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Detectors.MongoDB.Controllers
{
    [Route("mongodb/cluster/{clusterId}")]
    public class MongoClusterController : Controller
    {
        private readonly MongoClusterConfigCollection _configuration;

        public MongoClusterController(MongoClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("databases")]
        [HttpGet("databases.{format}")]
        public async Task<IActionResult> GetKeys(string clusterId)
        {
            var client = _configuration.GetClient(clusterId);
            var databases = (await client.ListDatabasesAsync()).ToEnumerable().Select(d => d.ToJObject()).ToList();

            return Ok(databases);
        }
        
        [HttpGet("replicaset/status")]
        [HttpGet("replicaset/status.{format}")]
        public async Task<IActionResult> GetReplicaSetStatus(string clusterId)
        {
            var client = _configuration.GetDirectClient(clusterId);
            ReplicaSetStatus replSetStatus;

            try
            {
                replSetStatus = await client.GetDatabase("admin").RunCommandAsync(new ObjectCommand<ReplicaSetStatus>(new { replSetGetStatus = true }));
            }
            catch (MongoCommandException e)
            {
                return BadRequest(new MongoCommandError
                {
                    Code = e.Code,
                    CodeName = e.CodeName,
                    Message = e.ErrorMessage
                });
            }

            return Ok(replSetStatus);            
        }
    }
}