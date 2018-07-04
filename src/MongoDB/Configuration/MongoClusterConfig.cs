using MongoDB.Bson;
using MongoDB.Driver;

namespace Detectors.MongoDB.Configuration
{
    public class MongoClusterConfig
    {
        public string Id { get; set; }
        public string ConnectionString { get; set; }

        public MongoClient BuildClient()
        {
            return new MongoClient(ConnectionString);
        }

        public IMongoDatabase GetDatabase(string dbName)
        {
            var client = BuildClient();
            return client?.GetDatabase(dbName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string dbName, string collectionName)
        {
            var database = GetDatabase(dbName);
            return database.GetCollection<BsonDocument>(collectionName);
        }
    }
}