using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Controllers.Dto
{
    public class MongoFindRequest
    {
        public JObject Filter { get; set; }
        public JObject Sort { get; set; }
        public JObject Projection { get; set; }
        
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }
}