using System.Linq;
using System.Threading.Tasks;
using Detectors.MongoDB.Configuration;
using Detectors.MongoDB.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Detectors.MongoDB.Controllers
{
    [Route("mongodb/cluster/{clusterId}/database/{dbName}")]
    public class MongoDatabaseController : Controller
    {
        private readonly MongoClusterConfigCollection _configuration;
        public MongoDatabaseController(MongoClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("collections")]
        [HttpGet("collections.{format}")]
        public async Task<IActionResult> GetCollectionNames(string clusterId, string dbName)
        {
            var client = _configuration.BuildClient(clusterId);
            var database = client.GetDatabase(dbName);

            var names = (await database.ListCollectionsAsync()).ToEnumerable().Select(d => d.ToJObject()).ToList();
            return Ok(names);
        }
        
    }
}