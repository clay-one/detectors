using System;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;

namespace Detectors.MongoDB.Util
{
    public static class BsonUtilities
    {
        public static JObject ToJObject(this BsonDocument document)
        {
            var json = document.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict});
            return JObject.Parse(json);
        }

        public static long? ToInteger(this BsonDocument document)
        {
            if (document == null)
                return null;
            
            try
            {
                return document.ToInt64();
            }
            catch (Exception)
            {
                // Do nothing, try other methods to get an integer
            }

            foreach (var value in document.Values)
            {
                try
                {
                    return value.ToInt64();
                }
                catch (Exception)
                {
                    // Do nothing, try other methods to get an integer
                }
            }

            return null;
        }

        public static BsonDocument ToBsonDocument(this JObject obj)
        {
            return obj == null ? null : BsonSerializer.Deserialize<BsonDocument>(obj.ToString());
        }
    }
}