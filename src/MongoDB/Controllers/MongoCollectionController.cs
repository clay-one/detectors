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
        public IActionResult CountDocuments(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoCountRequest body)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            

//            var queryString = "{ \n    \"AccountId\" : \"demo-account\", \n    \"TenantId\" : \"\"\n}";
//            var queryDoc = BsonSerializer.Deserialize<BsonDocument>(queryString);
//
//            var cursor = coll.FindSync(queryDoc, new FindOptions<BsonDocument> { });
//            var result = cursor.ToList();
//
//            var stage1String = "{$match: { \"AccountId\" : \"demo-account\", \"TenantId\" : \"\"}}";
//            var stage2String = "{$group: {  \"_id\": \"$AccountId\", \"count\": {\"$sum\": 1}}},";
//            var stage3String = "{$count: \"count\"}";
//
//            var stage1Doc = BsonSerializer.Deserialize<BsonDocument>(stage1String);
//            var stage2Doc = BsonSerializer.Deserialize<BsonDocument>(stage2String);
//            var stage3Doc = BsonSerializer.Deserialize<BsonDocument>(stage3String);
//
//            var aggResult = coll.Aggregate(PipelineDefinition<BsonDocument, BsonDocument>.Create(stage1Doc, stage2Doc, stage3Doc));
//            result = aggResult.ToList();
//
//            var p = o.GetValue("pipeline");
//
//            dynamic d = o;
//            var p2 = d.pipeline;
//            var p3 = d.alaki;
//            
//            var x1 = result[0];
//            var x2 = x1.AsInt64;
//
            return Ok();
        }
    }
}