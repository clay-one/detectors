using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Controllers
{
    [Route("mongodb")]
    public class MongoQueryController : Controller
    {
        [HttpPost("test")]
        [HttpPost("test.{format}")]
        public IActionResult Test([FromBody]JObject o)
        {
            var client = new MongoClient("mongodb://tesseract01-a.appson.local:27018");
            var db = client.GetDatabase("tesseract-test");
            var coll = db.GetCollection<BsonDocument>("AccountData");

            var queryString = "{ \n    \"AccountId\" : \"demo-account\", \n    \"TenantId\" : \"\"\n}";
            var queryDoc = BsonSerializer.Deserialize<BsonDocument>(queryString);

            var cursor = coll.FindSync(queryDoc, new FindOptions<BsonDocument> { });
            var result = cursor.ToList();

            var stage1String = "{$match: { \"AccountId\" : \"demo-account\", \"TenantId\" : \"\"}}";
            var stage2String = "{$group: {  \"_id\": \"$AccountId\", \"count\": {\"$sum\": 1}}},";
            var stage3String = "{$count: \"count\"}";

            var stage1Doc = BsonSerializer.Deserialize<BsonDocument>(stage1String);
            var stage2Doc = BsonSerializer.Deserialize<BsonDocument>(stage2String);
            var stage3Doc = BsonSerializer.Deserialize<BsonDocument>(stage3String);

            var aggResult = coll.Aggregate(PipelineDefinition<BsonDocument, BsonDocument>.Create(stage1Doc, stage2Doc, stage3Doc));
            result = aggResult.ToList();

            var p = o.GetValue("pipeline");

            dynamic d = o;
            var p2 = d.pipeline;
            var p3 = d.alaki;
            
            var x1 = result[0];
            var x2 = x1.AsInt64;

            return Ok(result);
        }
    }
}
