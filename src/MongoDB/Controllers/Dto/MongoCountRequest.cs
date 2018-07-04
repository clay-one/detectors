using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Controllers.Dto
{
    public class MongoCountRequest
    {
        public JObject Filter { get; set; }
    }
}