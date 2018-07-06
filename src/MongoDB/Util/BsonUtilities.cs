using System;
using System.Linq;
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

        public static BsonDocument Preprocess(this BsonDocument bson)
        {
            if (bson == null || bson.Elements == null || bson.ElementCount < 1)
                return bson;
            
            foreach (var element in bson.Elements.ToList())
            {
                if (element.Value is BsonArray array)
                {
                    for (var i = 0; i < array.Count; i++)
                    {
                        if (!(array[i] is BsonDocument arrayDocument))
                            continue;

                        if (arrayDocument.ElementCount == 1 && 
                            arrayDocument.ElementAt(0).Name == "$nowPlusSeconds" && 
                            arrayDocument.ElementAt(0).Value is BsonInt32 arrayDeltaSeconds)
                        {
                            array[i] = new BsonDateTime(DateTime.UtcNow + TimeSpan.FromSeconds(arrayDeltaSeconds.Value));
                        }
                        else
                        {
                            arrayDocument.Preprocess();
                        }
                    }
                }
                
                if (!(element.Value is BsonDocument document))
                    continue;

                if (document.ElementCount == 1 && 
                    document.ElementAt(0).Name == "$nowPlusSeconds" && 
                    document.ElementAt(0).Value is BsonInt32 deltaSeconds)
                {
                    bson.Set(element.Name, new BsonDateTime(DateTime.UtcNow + TimeSpan.FromSeconds(deltaSeconds.Value)));
                }
                else
                {
                    document.Preprocess();
                }
            }

            return bson;
        }
    }
}