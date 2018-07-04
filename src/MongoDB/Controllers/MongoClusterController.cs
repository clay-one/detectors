using System.Linq;
using System.Threading.Tasks;
using Detectors.MongoDB.Configuration;
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
    
    }
}