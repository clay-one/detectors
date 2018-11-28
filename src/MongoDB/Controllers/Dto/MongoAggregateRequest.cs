using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Controllers.Dto
{
    public class MongoAggregateRequest
    {
        public List<JObject> Stages { get; set; }
    }
}