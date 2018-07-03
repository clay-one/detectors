using System.Linq;
using System.Threading.Tasks;
using Detectors.MongoDB.Configuration;
using Detectors.MongoDB.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Controllers
{
    [Route("mongodb/cluster/{clusterId}/database/{dbName}/collection/{collectionName}")]
    public class MongoCollectionController : Controller
    {
        private readonly MongoClusterConfigCollection _configuration;
        public MongoCollectionController(MongoClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("indexes")]
        [HttpGet("indexes.{format}")]
        public async Task<IActionResult> GetIndexes(string clusterId, string dbName, string collectionName)
        {
            var client = _configuration.BuildClient(clusterId);
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            var indexes = (await collection.Indexes.ListAsync()).ToEnumerable().Select(i => i.ToJObject()).ToList();
            return Ok(indexes);
        }
        
    }
}