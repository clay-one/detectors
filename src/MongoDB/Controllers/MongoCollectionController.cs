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

        [HttpPost("aggregate")]
        [HttpPost("aggregate.{format}")]
        public async Task<IActionResult> Aggregate(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoAggregateRequest body)
        {
            var aggregateCursor = await AggregateInternal(clusterId, dbName, collectionName, body);
            
            var aggregate = await aggregateCursor.ToListAsync();
            var result = aggregate.Select(a => a.ToJObject()).ToList();
            return Ok(result);
        }

        [HttpPost("aggregate/object")]
        [HttpPost("aggregate/object.{format}")]
        public async Task<IActionResult> AggregateToObject(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoAggregateRequest body)
        {
            var aggregateCursor = await AggregateInternal(clusterId, dbName, collectionName, body);
            var resultBson = await aggregateCursor.SingleOrDefaultAsync();
            if (resultBson == null)
                return NotFound();
            
            return Ok(resultBson.ToJObject());
        }

        [HttpPost("aggregate/integer")]
        [HttpPost("aggregate/integer.{format}")]
        public async Task<IActionResult> AggregateToInteger(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoAggregateRequest body)
        {
            var aggregateCursor = await AggregateInternal(clusterId, dbName, collectionName, body);
            var resultBson = await aggregateCursor.SingleOrDefaultAsync();
            if (resultBson == null)
                return NotFound();

            var result = resultBson.ToInteger();
            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPost("find")]
        [HttpPost("find.{format}")]
        public async Task<IActionResult> Find(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoFindRequest body)
        {
            var findCursor = await FindInternal(clusterId, dbName, collectionName, body);
            
            var aggregate = await findCursor.ToListAsync();
            var result = aggregate.Select(a => a.ToJObject()).ToList();
            return Ok(result);
        }
        
        [HttpPost("find/object")]
        [HttpPost("find/object.{format}")]
        public async Task<IActionResult> FindObject(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoFindRequest body)
        {
            var findCursor = await FindInternal(clusterId, dbName, collectionName, body);
            var resultBson = await findCursor.SingleOrDefaultAsync();
            if (resultBson == null)
                return NotFound();
            
            return Ok(resultBson.ToJObject());
        }
        
        [HttpPost("find/integer")]
        [HttpPost("find/integer.{format}")]
        public async Task<IActionResult> FindInteger(string clusterId, string dbName, string collectionName, 
            [FromBody]MongoFindRequest body)
        {
            var findCursor = await FindInternal(clusterId, dbName, collectionName, body);
            var resultBson = await findCursor.SingleOrDefaultAsync();
            if (resultBson == null)
                return NotFound();

            var result = resultBson.ToInteger();
            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        private async Task<IAsyncCursor<BsonDocument>> AggregateInternal(
            string clusterId, string dbName, string collectionName, MongoAggregateRequest body)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var stages = body.Stages?.Select(s => BsonSerializer.Deserialize<BsonDocument>(s.ToString()));

            var pipelineDefinition = PipelineDefinition<BsonDocument, BsonDocument>.Create(stages);
            var aggregateOptions = new AggregateOptions {};
            return await collection.AggregateAsync(pipelineDefinition, aggregateOptions);
        }

        private async Task<IAsyncCursor<BsonDocument>> FindInternal(
            string clusterId, string dbName, string collectionName, MongoFindRequest body)
        {
            var collection = _configuration.GetCollection(clusterId, dbName, collectionName);
            var filter = body.Filter.ToBsonDocument();
            var sort = body.Sort.ToBsonDocument();
            var projection = body.Projection.ToBsonDocument();

            var findOptions = new FindOptions<BsonDocument, BsonDocument>()
            {
                Limit = body.Limit,
                Skip = body.Skip,
                Sort = sort,
                Projection = projection
            };
            
            return await collection.FindAsync(filter, findOptions);
        }
    }
}