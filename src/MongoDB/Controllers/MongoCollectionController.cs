using System.Linq;
using System.Threading.Tasks;
using Detectors.MongoDB.Configuration;
using Detectors.MongoDB.Controllers.Dto;
using Detectors.MongoDB.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

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
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var indexes = (await collection.Indexes.ListAsync()).ToEnumerable().Select(i => i.ToJObject()).ToList();
            return Ok(indexes);
        }

        [HttpGet("count/exact")]
        [HttpGet("count/exact.{format}")]
        public async Task<IActionResult> CountAllDocumentsExact(string clusterId, string dbName, string collectionName)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var count = await collection.CountDocumentsAsync(new BsonDocument());

            return Ok(count);
        }
        
        [HttpGet("count/estimate")]
        [HttpGet("count/estimate.{format}")]
        public async Task<IActionResult> CountAllDocumentsEstimated(string clusterId, string dbName, string collectionName)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var count = await collection.EstimatedDocumentCountAsync();

            return Ok(count);
        }
        
        [HttpPost("count")]
        [HttpPost("count.{format}")]
        public async Task<IActionResult> CountDocuments(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoCountRequest body)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var filter = BsonSerializer.Deserialize<BsonDocument>(body.Filter.ToString());

            var count = await collection.CountDocumentsAsync(filter, new CountOptions {});
            return Ok(count);
        }
    }
}