using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Util
{
    public static class BsonUtilities
    {
        public static JObject ToJObject(this BsonDocument document)
        {
            return JObject.Parse(document.ToJson());
        }
    }
}