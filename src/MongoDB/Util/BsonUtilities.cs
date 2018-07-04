using System;
using System.Linq;
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
    }
}